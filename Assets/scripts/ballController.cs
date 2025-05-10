using UnityEngine;

public class ballController : MonoBehaviour
{
    public float ballSpeed = 10f;
    public float tiltSensitivity = 15f;
    public float touchSmoothness = 10f;
    private float minPos = -2.40f;
    private float maxPos = 2.40f;
    public uiManager ui;
    private Rigidbody2D rb;
    private bool currentPlatformAndroid = false;
    private bool gameStarted = false;
    private float currentTilt = 30f; // Store last tilt input

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        #if UNITY_ANDROID
            currentPlatformAndroid = true;
        #else
            currentPlatformAndroid = false;
        #endif
    }

    void Start()
    {
        Debug.Log("Current Platform Android: " + currentPlatformAndroid);
        Input.gyro.enabled = true;
        gameStarted = true; // Set gameStarted to true to allow movement
    }

    public void StartBall()
    {
        gameStarted = true;
        gameObject.SetActive(true);
        Debug.Log("✅ Ball has started moving!");
    }

    void Update()
    {
    Debug.Log("📣 Update() is running");

    if (!gameStarted)
    {
        Debug.Log("⛔ gameStarted is FALSE — skipping movement");
        return;
    }

    Debug.Log("✅ Game Started");

    if (currentPlatformAndroid)
    {
        Debug.Log("📱 Platform: Android");

        if (ControlManager.IsTilt)
        {
            Debug.Log("🌀 Tilt Control ENABLED");
            AccelerometerMove();
            Debug.Log("✅ AccelerometerMove() called");
        }
        else
        {
            Debug.Log("👆 Touch Control ENABLED");
            TouchMove();
        }
    }
    else
    {
        Debug.Log("🖱️ Non-Android: Using keyboard");
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * ballSpeed, rb.velocity.y);
    }

    Debug.Log("📍 Calling ClampPosition()");
    ClampPosition();
}


   void TouchMove()
{
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        Debug.Log("Touch detected at: " + touch.position);

        // ✅ Calculate proper Z-depth based on camera-to-ball distance
        Vector3 touchPosition = touch.position;
        touchPosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(touchPosition);
        targetPos.z = 0f; // Keep 2D plane
        targetPos.y = transform.position.y; // Lock Y if needed

        transform.position = Vector3.Lerp(transform.position, targetPos, touchSmoothness * Time.deltaTime);
    }
}

void AccelerometerMove()
{
    float tiltInput = Input.acceleration.x;

   
    if (Mathf.Abs(tiltInput) < 0.02f) return;


    float targetVelocityX = tiltInput * tiltSensitivity;


    float smoothVelocityX = Mathf.Lerp(rb.velocity.x, targetVelocityX, Time.deltaTime * 5f);

    rb.velocity = new Vector2(smoothVelocityX, rb.velocity.y);

}

   void ClampPosition()
    {
    Vector3 pos = transform.position;
  
    pos.x = Mathf.Clamp(pos.x, minPos, maxPos);
    pos.y = Mathf.Clamp(pos.y, -4.0f, 5.0f);
    transform.position = pos;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (ui == null)
            {
                return;
            }
            AudioManager.Instance.PlayCollideSFX();

            if (ui.gameEnded) return; 

            ui.ReduceLife(); 

            if (ui.playerLives > 0) 
            {
                Destroy(collision.gameObject); 
            }
            else
            {
                Destroy(gameObject); // Destroy ball
                ui.GameOverActivated(); // Call Game Over
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Coin"))
    {
        if (ui != null && !ui.gameEnded)
        {
            ui.IncreaseScore(20); // ✅ Add score
        }

        AudioManager.Instance.PlayCoinSFX(); // ✅ Play sound
        Destroy(other.gameObject); // ✅ Remove coin
    }
}


}
