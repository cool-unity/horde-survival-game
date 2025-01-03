using UnityEngine;

public class BasicPlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 3f; // Effective range of the player's attack
    [SerializeField] private int attackDamage = 10; // Damage dealt per attack
    [SerializeField] private float fireRate = 1f; // Time in seconds between consecutive attacks

    [Header("Visual Settings")]
    [SerializeField] private LineRenderer lineRenderer; // Reference to the LineRenderer component
    [SerializeField] private int circleSegments = 50; // Number of segments in the circle
    [SerializeField] private float shrinkDuration = 0.5f; // Time for the circle to shrink completely
    [SerializeField] private float startLineWidth = 0.1f; // Initial width of the line
    [SerializeField] private float endLineWidth = 0.01f; // Final width of the line

    private float nextAttackTime = 0f; // Time when the player can attack again

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer is not assigned or found on the Player GameObject!");
            }
        }

        // Configure the LineRenderer
        lineRenderer.loop = true;
        lineRenderer.positionCount = circleSegments + 1;
        lineRenderer.enabled = false; // Hide the LineRenderer initially
    }

    private void FixedUpdate()
    {
        // Continuously try to attack within the specified fire rate
        TryAttack();
    }

    /// <summary>
    /// Attempts to attack all enemies within range.
    /// </summary>
    private void TryAttack()
    {
        // Check if the player can attack
        if (Time.time < nextAttackTime)
            return;

        // Get all enemies within range
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);

        bool enemiesHit = false;

        foreach (Collider2D collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy")) // Ensure the object is tagged as an enemy
            {
                // Apply damage to the enemy
                ApplyDamage(collider);
                enemiesHit = true;
            }
        }

        if (enemiesHit)
        {
            Debug.Log($"Player attacked all enemies in range for {attackDamage} damage.");
        }
        else
        {
            Debug.Log("No enemies in range to attack.");
        }

        // Display and animate the attack range using the LineRenderer
        StartCoroutine(AnimateAttackCircle());

        // Set the next attack time
        nextAttackTime = Time.time + fireRate;
    }

    /// <summary>
    /// Applies damage to the specified enemy.
    /// </summary>
    /// <param name="enemy">The Collider2D of the enemy to damage.</param>
    private void ApplyDamage(Collider2D enemy)
    {
        // Get the HealthComponent on the enemy and apply damage
        HealthComponent health = enemy.GetComponent<HealthComponent>();

        if (health != null)
        {
            health.TakeDamage(attackDamage);
            Debug.Log($"Attacked {enemy.name} for {attackDamage} damage.");
        }
        else
        {
            Debug.LogWarning($"Enemy {enemy.name} does not have a HealthComponent!");
        }
    }

    /// <summary>
    /// Animates a shrinking circular range using the LineRenderer.
    /// </summary>
    private System.Collections.IEnumerator AnimateAttackCircle()
    {
        lineRenderer.enabled = true;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            float currentRange = Mathf.Lerp(attackRange, 0f, elapsedTime / shrinkDuration);
            float currentLineWidth = Mathf.Lerp(startLineWidth, endLineWidth, elapsedTime / shrinkDuration);

            // Update the circle's position and size dynamically
            DrawCircle(currentRange);
            lineRenderer.startWidth = currentLineWidth;
            lineRenderer.endWidth = currentLineWidth;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    /// <summary>
    /// Draws a circular range using the LineRenderer.
    /// </summary>
    /// <param name="radius">The radius of the circle.</param>
    private void DrawCircle(float radius)
    {
        float angleStep = 360f / circleSegments;
        float angle = 0f;

        for (int i = 0; i <= circleSegments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x + transform.position.x, y + transform.position.y, 0f));
            angle += angleStep;
        }
    }
}
