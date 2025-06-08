using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public ObstaclePool pool;
    public float spawnInterval = 1.5f;
    public float minX = -2f, maxX = 2f;
    float startY = 7f;
    private bool gameStarted = false;
    private float timer;
    private uiManager ui;

    void Start()  // Fixed capitalization here!
    {
        ui = FindObjectOfType<uiManager>();
    }

    void Update()
    {
        if(!gameStarted || (ui != null && ui.gameEnded))
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        GameObject obj = pool.GetRandomObstacle();
        if (obj == null) return;

        float x = Random.Range(minX, maxX);
        obj.transform.position = new Vector3(x, startY, 0f);

        // Assign original prefab reference for recycling
        Obstacle obstacle = obj.GetComponent<Obstacle>();
        if (obstacle != null && obstacle.originalPrefab == null)
        {
            foreach (var type in pool.obstacleTypes)
            {
                if (type.prefab.name == obj.name.Replace("(Clone)", "").Trim())
                {
                    obstacle.originalPrefab = type.prefab;
                    break;
                }
            }
        }
    }

    public void StartSpawning()
    {
        gameStarted = true;
    }
}
