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
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObstacle(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        pools[prefab].Enqueue(obj);
    }
}
