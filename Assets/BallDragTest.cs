using UnityEngine;

public class BallDragTest : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 dragOffset;

    public LayerMask ballLayer; // Assign the correct layer in Inspector
    public float minX = -2.4f;
    public float maxX = 2.4f;

    void Update()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPos, ballLayer);
            Debug.Log("Mouse down at: " + worldPos + " | Hit: " + (hit ? hit.name : "None"));

            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                dragOffset = transform.position - worldPos;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 target = worldPos + dragOffset;
            target.y = transform.position.y;
            target.x = Mathf.Clamp(target.x, minX, maxX);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 10f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
