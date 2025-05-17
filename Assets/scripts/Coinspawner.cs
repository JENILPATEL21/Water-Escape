using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;  // The coin prefab to spawn
    public float spawnInterval = 2f; // Interval between spawns (in seconds)
    public float minX = -2f, maxX = 2f; // X position range
    public float spawnY = 7f; // Y spawn position
    public float destroyY = -5f; // Y position where coins are destroyed
    public float moveSpeed = 3f; // Speed at which the coins move down
    public float verticalGap = 1.5f; // Vertical gap between coins in the same group
    public float horizontalOffsetMin = -1f; // Minimum horizontal offset for each group
    public float horizontalOffsetMax = 1f; // Maximum horizontal offset for each group

    private float timer;
    private bool gameStarted = true;

    void Update()
    {
        if (!gameStarted) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnCoins();
        }

        MoveAndDestroyCoins();
    }

    public void SpawnCoins()
    {
        // Randomly determine the number of coins in this group (1 to 4 coins)
        int numberOfCoins = Random.Range(1, 5); 

        // Determine a random horizontal offset for this group
        float horizontalOffset = Random.Range(horizontalOffsetMin, horizontalOffsetMax); 

        // Spawn the coins in a vertical line with a gap between them
        for (int i = 0; i < numberOfCoins; i++)
        {
            float x = horizontalOffset; // Keep the X position the same for all coins in the group
            float y = spawnY - i * verticalGap; // Y position with a gap between coins

            Vector3 spawnPosition = new Vector3(x, y, 0f); // Set the spawn position

            // Instantiate the coin at the spawn position
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void MoveAndDestroyCoins()
    {
        // Find all the coin game objects in the scene and move them down
        foreach (var coin in GameObject.FindGameObjectsWithTag("Coin"))  // Use the "Coin" tag to find coins
        {
            coin.transform.position += Vector3.down * moveSpeed * Time.deltaTime;

            // If the coin falls below the destroyY value, destroy it
            if (coin.transform.position.y < destroyY)
            {
                Destroy(coin);
            }
        }
    }
}
