using UnityEngine;

public class ExperienceToken : MonoBehaviour
{
    public int experienceValue = 10; // Amount of experience this token provides
    private bool isCollected = false; // Prevent double collection

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.CollectExperience(experienceValue);
            }

            Destroy(gameObject); // Destroy token after pickup
        }
    }
}
