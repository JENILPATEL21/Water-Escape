using UnityEngine;
using System.Collections;

public class ballController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float ballSpeed = 10f;
    public float tiltSensitivity = 15f;
    public float touchSmoothness = 10f;
    private float minPos = -2.40f;
    private float maxPos = 2.40f;

    [Header("References")]
    public uiManager ui;
    public GameObject explosionEffectPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D ballCollider;

    [Header("Touch Control")]
    [SerializeField] private LayerMask ballLayer;
    private bool isDragging = false;
    private Vector3 dragOffset = Vector3.zero;

    [Header("Invincibility")]
    public float invincibleDuration = 2f;
    private bool isInvincible = false;

    private bool currentPlatformAndroid = false;
    private bool gameStarted = false;

    void Awake()
    {
        ui = FindObjectOfType<uiManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ballCollider = GetComponent<Collider2D>();

        #if UNITY_ANDROID
            currentPlatformAndroid = true;
        #else
            currentPlatformAndroid = false;
        #endif

        if (ui == null) Debug.LogError("uiManager not found in scene!");
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer missing on ball!");
    }

    void Start()
    {
        Input.gyro.enabled = true;
        gameStarted = true;
    }

    public void StartBall()
    {
        gameStarted = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!gameStarted) return;

        if (currentPlatformAndroid)
        {
            if (ControlManager.IsTilt)
                AccelerometerMove();
            else
                TouchMove();
        }
        else
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * ballSpeed, rb.velocity.y);
        }

        ClampPosition();
    }

// void DebugTouchDot(Vector3 pos)
// {
//     GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//     Destroy(dot.GetComponent<Collider>());
//     dot.transform.position = pos;
//     dot.transform.localScale = Vector3.one * 0.3f;
//     dot.GetComponent<Renderer>().material.color = Color.red;
//     Destroy(dot, 0.5f); // Auto-destroy for debugging
// }
#region Touch Control
void TouchMove()
{
    if (Input.touchCount == 0)
    {
        isDragging = false;
        return;
    }

    Touch touch = Input.GetTouch(0);
    Vector3 worldPos = GetWorldPositionFromScreen(touch.position);
    worldPos.z = 0;

    switch (touch.phase)
    {
        case TouchPhase.Began:
            // IMPORTANT: use OverlapPoint instead of Raycast for better touch accuracy
            Collider2D hit = Physics2D.OverlapPoint(worldPos, ballLayer);
            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                dragOffset = transform.position - worldPos;
            }
            break;

        case TouchPhase.Moved:
        case TouchPhase.Stationary:
            if (isDragging)
            {
                Vector3 target = worldPos + dragOffset;
                target.y = transform.position.y;
                target.x = Mathf.Clamp(target.x, minPos, maxPos);
                transform.position = Vector3.Lerp(transform.position, target, touchSmoothness * Time.deltaTime);
            }
            break;

        case TouchPhase.Ended:
        case TouchPhase.Canceled:
            isDragging = false;
            break;
    }
}
#endregion


Vector3 GetWorldPositionFromScreen(Vector2 screenPos)
{
    Ray ray = Camera.main.ScreenPointToRay(screenPos);
    Plane xyPlane = new Plane(Vector3.forward, Vector3.zero); // Plane at Z=0
    if (xyPlane.Raycast(ray, out float enter))
        return ray.GetPoint(enter);
    return Vector3.zero;
}



    #region Tilt Control
    void AccelerometerMove()
    {
        float tiltInput = Input.acceleration.x;
        if (Mathf.Abs(tiltInput) < 0.02f) return;

        float targetVelocityX = tiltInput * tiltSensitivity;
        float smoothVelocityX = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.deltaTime * 5f);
        rb.velocity = new Vector2(smoothVelocityX, rb.velocity.y);
    }
    #endregion

    #region Position Clamp
    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minPos, maxPos);
        pos.y = Mathf.Clamp(pos.y, -4.0f, 5.0f);
        transform.position = pos;
    }
    #endregion

    #region Collision Handling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle") || isInvincible) return;

        if (ui == null)
        {
            Debug.LogWarning("UI Manager reference missing!");
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCollideSFX();
        }

        if (ui.gameEnded) return;

        ui.ReduceLife();

        StartCoroutine(InvincibilityBlink());

        if (ui.playerLives > 0)
        {
            if (explosionEffectPrefab != null)
                Instantiate(explosionEffectPrefab, collision.transform.position, Quaternion.identity);

            collision.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
            ui.GameOverActivated();
        }
    }
    #endregion

    #region Invincibility Blink
    private IEnumerator InvincibilityBlink()
    {
        isInvincible = true;

        if (ballCollider != null)
            ballCollider.enabled = false;

        float elapsed = 0f;
        float blinkInterval = 0.2f;
        bool visible = true;

        while (elapsed < invincibleDuration)
        {
            visible = !visible;
            SetAlpha(visible ? 1f : 0.3f);

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        SetAlpha(1f);
        if (ballCollider != null)
            ballCollider.enabled = true;

        isInvincible = false;
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
    #endregion

    #region Coin Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (ui != null && !ui.gameEnded)
            {
                ui.IncreaseScore(20);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCoinSFX();
            }

            Destroy(other.gameObject);
        }
    }
    #endregion
}
