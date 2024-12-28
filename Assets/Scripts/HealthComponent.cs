using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health
    private int currentHealth;

    private ShieldComponent shieldComponent;

    public delegate void OnHealthChanged(int current, int max);
    public event OnHealthChanged HealthChanged;

    public delegate void OnDeath();
    public event OnDeath Died;

    private void Start()
    {
        currentHealth = maxHealth;

        // Attempt to get a ShieldComponent if one exists
        shieldComponent = GetComponent<ShieldComponent>();

        if (shieldComponent == null)
        {
            Debug.Log($"{gameObject.name} does not have a ShieldComponent. Health will take all damage directly.");
        }

        HealthChanged?.Invoke(currentHealth, maxHealth); // Notify listeners of initial health value
    }

    public void TakeDamage(int damage)
    {
        // Apply damage to the shield first, if available
        if (shieldComponent != null)
        {
            damage = shieldComponent.AbsorbDamage(damage);
        }

        // Apply remaining damage to health
        if (damage > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            HealthChanged?.Invoke(currentHealth, maxHealth);

            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Died?.Invoke();
    }
}
