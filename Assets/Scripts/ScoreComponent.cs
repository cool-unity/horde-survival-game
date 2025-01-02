using UnityEngine;
using UnityEngine.UI;

public class ScoreComponent : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private Text scoreText;

    private int score = 0;

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
        Debug.Log($"Score Updated: {score}");
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
        Debug.Log("Score Reset");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}