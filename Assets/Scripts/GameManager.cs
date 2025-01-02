using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State Settings")]
    [SerializeField] private int playerLives = 3;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private int startingScore = 0;

    private bool isGameOver = false;
    private int score;
    private GameObject playerInstance;

    public delegate void GameStateChangeDelegate(bool isGameOver);
    public event GameStateChangeDelegate OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        isGameOver = false;
        playerLives = 3;
        score = startingScore;

        SpawnPlayer();
        Debug.Log("Game Started");

        OnGameStateChanged?.Invoke(isGameOver);
    }

    public void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game Over");

        OnGameStateChanged?.Invoke(isGameOver);
        // Optionally show UI or pause the game.
    }

    public void RestartLevel()
    {
        if (isGameOver)
        {
            StartGame();
        }
    }

    public void PlayerDied()
    {
        playerLives--;

        if (playerLives > 0)
        {
            SpawnPlayer();
        }
        else
        {
            EndGame();
        }
    }

    private void SpawnPlayer()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        if (playerPrefab != null && playerSpawnPoint != null)
        {
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            Debug.Log("Player Spawned");

            // Subscribe to player health death event
            var health = playerInstance.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Died += PlayerDied;
                health.SetHealth(100); // Example: Initialize player health
            }
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score: {score}");
    }

    public int GetScore()
    {
        return score;
    }
}
