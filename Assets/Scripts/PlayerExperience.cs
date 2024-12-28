using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    private int experienceThisLevel = 0; // Experience collected in the current level

    public void CollectExperience(int amount)
    {
        experienceThisLevel += amount;
        Debug.Log($"Collected {amount} experience. Total: {experienceThisLevel}");
    }

    public int GetExperienceThisLevel()
    {
        return experienceThisLevel;
    }

    public void ResetExperience()
    {
        experienceThisLevel = 0;
    }
}
