using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple object‑pooling manager for 2D obstacles that fall downward.
/// Attach this script to an empty GameObject in your scene and assign 5 obstacle prefabs in the inspector.
/// Obstacles are recycled when they drop below a configurable Y position.
/// </summary>
public class ObstaclePoolmanager : MonoBehaviour
{
    [Header("Pool Settings")]
    [Tooltip("Drag your 5 obstacle prefabs here (different shapes, sizes, etc.)")] 
    public GameObject[] obstaclePrefabs;

    [Tooltip("Number of pooled instances to create for *each* prefab at start.")]
    [Min(1)] public int poolSizePerType = 10;

    [Header("Spawn Settings")] 
    public float spawnInterval = 1.25f; // seconds between spawns
    public Vector2 spawnXRange = new Vector2(-2.5f, 2.5f); // horizontal span where obstacles appear
    public float spawnY = 6f;   // Y position where obstacles are spawned (just above camera)

    [Header("Movement & Deactivation")] 
    public float fallSpeed = 4f;    // constant downward speed
    public float deactivateY = -6f; // when an obstacle drops below this Y, it is recycled

    private readonly List<GameObject> pool = new();
    private float timer;

    #region Unity Lifecycle
    private void Awake()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("ObstaclePool: No prefabs assigned!");
            enabled = false;
            return;
        }

        // Pre‑instantiate pool objects and keep them inactive.
        BuildPool();
    }

    private void Update()
    {
        // 1. Periodically spawn a pooled obstacle.
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }

        // 2. Move active obstacles downward and recycle when off‑screen.
        for (int i = 0; i < pool.Count; i++)
        {
            var obj = pool[i];
            if (!obj.activeInHierarchy) continue;

            obj.transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            if (obj.transform.position.y <= deactivateY)
            {
                obj.SetActive(false);
            }
        }
    }
    #endregion

    #region Pool Logic
    /// <summary>
    /// Creates the pooled instances (poolSizePerType for each prefab) and stores them inactive as children.
    /// </summary>
    private void BuildPool()
    {
        foreach (var prefab in obstaclePrefabs)
        {
            for (int i = 0; i < poolSizePerType; i++)
            {
                GameObject obj = Instantiate(prefab, Vector3.one * 1000f, Quaternion.identity, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
    }

    /// <summary>
    /// Retrieves the first inactive object from the pool, if any.
    /// </summary>
    /// <summary>
/// Randomly picks a prefab type and returns an inactive object from that group, if available.
/// </summary>
    private GameObject GetPooledObject()
    {
    // Shuffle prefab indices
    List<int> prefabIndices = new List<int>();
    for (int i = 0; i < obstaclePrefabs.Length; i++) prefabIndices.Add(i);
    int attempts = obstaclePrefabs.Length * poolSizePerType;

    while (attempts-- > 0)
    {
        int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy && pool[i].name.StartsWith(obstaclePrefabs[prefabIndex].name))
                return pool[i];
        }
    }

    return null; // All are active
}


    /// <summary>
    /// Activates a pooled obstacle at a random X within spawnXRange.
    /// </summary>
    private void SpawnObstacle()
    {
        GameObject obj = GetPooledObject();
        if (obj == null) return;

        float x = Random.Range(spawnXRange.x, spawnXRange.y);
        obj.transform.position = new Vector3(x, spawnY, 0f);
        obj.SetActive(true);
    }
    #endregion
}
