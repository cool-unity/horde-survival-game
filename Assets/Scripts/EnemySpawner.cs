using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab; // Enemy type to spawn
    [SerializeField] private float spawnRate = 1f;   // Enemies per second
    [SerializeField] private int maxEnemies = 10;   // Maximum number of active enemies

    [Header("Spawn Area Settings")]
    [SerializeField] private Transform player;      // Reference to the player's transform
    [SerializeField] private float spawnRadius = 10f; // Distance around the player where enemies can spawn

    private float spawnTimer;                       // Timer to track spawn rate
    private int activeEnemyCount;                  // Current number of active enemies

    private void Start()
    {
        spawnTimer = 1f / spawnRate;
    }

    private void Update()
    {
        if (activeEnemyCount < maxEnemies)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                spawnTimer = 1f / spawnRate; // Reset the spawn timer
            }
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Initialize the enemy with the spawner and player reference
        BaseEnemy baseEnemy = newEnemy.GetComponent<BaseEnemy>();
        if (baseEnemy != null)
        {
            baseEnemy.Initialize(this, player);
        }

        activeEnemyCount++;
    }

    private Vector3 GetSpawnPosition()
    {
        // Generate a random position around the player within the spawn radius
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = player.position + (Vector3)(randomDirection * spawnRadius);

        return spawnPosition;
    }

    public void OnEnemyDestroyed()
    {
        activeEnemyCount--;
    }

    public void UpdateSpawnRate(float newSpawnRate)
    {
        spawnRate = newSpawnRate;
        Debug.Log($"Spawn rate for {enemyPrefab.name} updated to {spawnRate} enemies per second.");
    }

    public void UpdateMaxEnemies(int newMax)
    {
        maxEnemies = newMax;
        Debug.Log($"Max enemies for {enemyPrefab.name} updated to {maxEnemies}.");
    }
}
