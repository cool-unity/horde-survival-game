using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab to spawn
    [SerializeField] private Transform[] spawnPoints; // Spawn locations
    [SerializeField] private float spawnInterval = 5f; // Time interval between spawns
    [SerializeField] private int maxEnemies = 10; // Maximum number of enemies allowed at once

    private List<GameObject> activeEnemies = new List<GameObject>(); // List to track active enemies
    private bool isSpawning = false;

    private void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the EnemySpawner!");
            enabled = false;
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in the EnemySpawner!");
            enabled = false;
            return;
        }

        StartSpawning();
    }

    /// <summary>
    /// Starts the spawning coroutine.
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    /// <summary>
    /// Stops the spawning coroutine.
    /// </summary>
    public void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            StopCoroutine(SpawnEnemies());
        }
    }

    /// <summary>
    /// Coroutine to spawn enemies at regular intervals.
    /// </summary>
    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Spawns an enemy at a random spawn point.
    /// </summary>
    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        activeEnemies.Add(newEnemy);

        // Subscribe to the enemy's death event
        var healthComponent = newEnemy.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.Died += () => OnEnemyDied(newEnemy);
        }

        Debug.Log($"Enemy spawned at {spawnPoint.position}. Total active enemies: {activeEnemies.Count}");
    }

    /// <summary>
    /// Handles cleanup when an enemy dies.
    /// </summary>
    /// <param name="enemy">The enemy that died.</param>
    private void OnEnemyDied(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Debug.Log($"Enemy died. Total active enemies: {activeEnemies.Count}");
        }
    }

    /// <summary>
    /// Clears all active enemies.
    /// </summary>
    public void ClearEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        activeEnemies.Clear();
    }
}
