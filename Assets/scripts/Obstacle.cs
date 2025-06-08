using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject originalPrefab; // Will be assigned by pool
    private ObstaclePool pool;
    float scrollSpeed = 5f;
    public float endY = -25f;

    void Start()
    {
        pool = FindObjectOfType<ObstaclePool>();
    }

    void Update()
    {
        transform.position += Vector3.down * scrollSpeed * Time.deltaTime;

        if (transform.position.y < endY)
        {
            pool.ReturnObstacle(gameObject);
        }
    }
}
