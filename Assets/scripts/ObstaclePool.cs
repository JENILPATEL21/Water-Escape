using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleType
    {
        public GameObject prefab;
        public int poolSize = 10;
    }

    public ObstacleType[] obstacleTypes;

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    void Start()
    {
        foreach (var type in obstacleTypes)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < type.poolSize; i++)
            {
                GameObject obj = Instantiate(type.prefab);

                // Assign originalPrefab on the Obstacle component
                Obstacle obstacleScript = obj.GetComponent<Obstacle>();
                if (obstacleScript != null)
                {
                    obstacleScript.originalPrefab = type.prefab;
                }

                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            pools[type.prefab] = queue;
        }
    }

    public GameObject GetRandomObstacle()
    {
        if (obstacleTypes.Length == 0) return null;

        ObstacleType randomType = obstacleTypes[Random.Range(0, obstacleTypes.Length)];
        Queue<GameObject> queue = pools[randomType.prefab];

        GameObject obj;
        if (queue.Count > 0)
        {
            obj = queue.Dequeue();
        }
        else
        {
            obj = Instantiate(randomType.prefab);

            // Assign originalPrefab on the new instance too
            Obstacle obstacleScript = obj.GetComponent<Obstacle>();
            if (obstacleScript != null)
            {
                obstacleScript.originalPrefab = randomType.prefab;
            }
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObstacle(GameObject obj)
    {
        if (obj == null) return;

        Obstacle obstacleScript = obj.GetComponent<Obstacle>();
        if (obstacleScript == null)
        {
            Debug.LogWarning("Returned object does not have Obstacle component; destroying.");
            Destroy(obj);
            return;
        }

        GameObject prefab = obstacleScript.originalPrefab;
        if (!pools.ContainsKey(prefab))
        {
            Debug.LogWarning("No pool found for prefab: " + prefab.name + "; destroying.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[prefab].Enqueue(obj);
    }
}
