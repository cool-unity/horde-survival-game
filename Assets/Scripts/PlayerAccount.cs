using UnityEngine;

public class PlayerAccount : MonoBehaviour
{
    [Header("Account Stats")]
    public int currentLevel = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    private PlayerExperience playerExperience;

    private void Start()
    {
        playerExperience = GetComponent<PlayerExperience>();
        if (playerExperience == null)
        {
            Debug.LogError("PlayerExperience component missing!");
        }
    }

    public void LevelCompleted()
    {
        if (playerExperience == null) return;

        int experienceGained = playerExperience.GetExperienceThisLevel();
        currentExperience += experienceGained;

        Debug.Log($"Level completed! Gained {experienceGained} experience. Total: {currentExperience}");

        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }

        playerExperience.ResetExperience(); // Reset level experience
    }

    private void LevelUp()
    {
        currentExperience -= experienceToNextLevel;
        currentLevel++;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.5f); // Increase requirement for next level

        Debug.Log($"Leveled up! Current Level: {currentLevel}, Experience to next level: {experienceToNextLevel}");
    }

    public void LevelFailed()
    {
        Debug.Log("Level failed. Experience not applied.");
        playerExperience.ResetExperience(); // Reset experience without applying it
    }
}
