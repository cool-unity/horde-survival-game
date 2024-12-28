using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int currentExperience;
    public int experienceToNextLevel;
}

public class SaveSystem : MonoBehaviour
{
    public static void SavePlayer(PlayerAccount playerAccount)
    {
        PlayerData data = new PlayerData
        {
            currentLevel = playerAccount.currentLevel,
            currentExperience = playerAccount.currentExperience,
            experienceToNextLevel = playerAccount.experienceToNextLevel
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
        Debug.Log("Player data saved.");
    }

    public static void LoadPlayer(PlayerAccount playerAccount)
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            playerAccount.currentLevel = data.currentLevel;
            playerAccount.currentExperience = data.currentExperience;
            playerAccount.experienceToNextLevel = data.experienceToNextLevel;

            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.Log("No saved player data found.");
        }
    }
}
