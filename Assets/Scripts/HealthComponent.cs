using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public delegate void HealthChangedDelegate(int currentHealth, int maxHealth);
    public event HealthChangedDelegate HealthChanged;

    public delegate void DiedDelegate();
    public event DiedDelegate Died;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces health by the specified damage amount.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        HealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage, current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Sets the maximum and current health values.
    /// </summary>
    public void SetHealth(int newHealth, int? newMaxHealth = null)
    {
        if (newMaxHealth.HasValue)
        {
            maxHealth = Mathf.Max(1, newMaxHealth.Value); // Ensure maxHealth is at least 1.
        }

        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

        HealthChanged?.Invoke(currentHealth, maxHealth);

        Debug.Log($"{gameObject.name}'s health has been set: {currentHealth}/{maxHealth}");
    }

    /// <summary>
    /// Triggers the death logic.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Died?.Invoke();
        Destroy(gameObject);
    }
}