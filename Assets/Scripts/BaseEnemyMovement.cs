using UnityEngine;

public class BaseEnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Base speed for movement
    [SerializeField] private float rotationalOffsetSpeed = 1f; // Strength of perpendicular movement
    [SerializeField] private float separationRadius = 1f; // Minimum distance to maintain from other enemies
    [SerializeField] private float separationStrength = 2f; // Strength of repulsion force
    [SerializeField] private bool enableRandomOffset = true; // Toggle for randomness
    [SerializeField] private float randomMultiplierRange = 0.2f; // Range of randomness for rotational offset

    private Rigidbody2D rb;
    private bool isMoving = false;
    private Vector2 targetPosition;
    private float randomOffsetMultiplier; // Random multiplier for rotational offset strength
    private int rotationDirection; // Randomized rotation direction: 1 for clockwise, -1 for counterclockwise

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is not assigned!");
        }

        // Randomize the rotational offset multiplier
        randomOffsetMultiplier = enableRandomOffset ? Random.Range(1f - randomMultiplierRange, 1f + randomMultiplierRange) : 1f;

        // Randomize the rotation direction (1 for clockwise, -1 for counterclockwise)
        rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;

        Debug.Log($"Random offset multiplier set to: {randomOffsetMultiplier}, Rotation direction: {rotationDirection}");
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 separationForce = CalculateSeparationForce();
            MoveTowardsWithOffsetAndSeparation(targetPosition, separationForce);
            // Debug.Log($"Moving towards position: {targetPosition}");
        }
    }

    /// <summary>
    /// Sets a new position to move towards.
    /// </summary>
    /// <param name="newPosition">The position to move towards.</param>
    public void SetTargetPosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
        isMoving = true;

        // Debug.Log($"New target position set: {targetPosition}");
    }

    /// <summary>
    /// Moves the object toward the specified position, with a perpendicular offset and separation force.
    /// </summary>
    /// <param name="position">The position to move towards.</param>
    /// <param name="separationForce">The calculated separation force.</param>
    private void MoveTowardsWithOffsetAndSeparation(Vector2 position, Vector2 separationForce)
    {
        // Calculate the main direction vector
        Vector2 direction = (position - rb.position).normalized;

        // Calculate the perpendicular offset vector, applying the rotation direction and randomness
        Vector2 perpendicularOffset = new Vector2(-direction.y, direction.x) * rotationalOffsetSpeed * randomOffsetMultiplier * rotationDirection;

        // Combine the main direction, perpendicular offset, and separation force
        Vector2 finalDirection = (direction + perpendicularOffset + separationForce).normalized;

        // Apply the combined direction to the Rigidbody
        rb.linearVelocity = finalDirection * moveSpeed;

        // Stop moving if close enough to the target position
        if (Vector2.Distance(rb.position, position) < 0.1f)
        {
            StopMovement();
        }

        Debug.Log($"Direction: {direction}, Perpendicular Offset: {perpendicularOffset}, Separation Force: {separationForce}, Final Direction: {finalDirection}, Velocity: {rb.linearVelocity}");
    }

    /// <summary>
    /// Calculates the separation force to maintain distance from nearby objects.
    /// </summary>
    /// <returns>A Vector2 representing the separation force.</returns>
    private Vector2 CalculateSeparationForce()
    {
        Vector2 separationForce = Vector2.zero;

        // Find all nearby objects within the separation radius
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (Collider2D collider in nearbyObjects)
        {
            // Ignore self
            if (collider.gameObject == gameObject)
                continue;

            // Calculate a repulsion vector away from the nearby object
            Vector2 directionAway = (rb.position - (Vector2)collider.transform.position).normalized;

            // Scale the force based on proximity (closer objects exert stronger repulsion)
            float distance = Vector2.Distance(rb.position, collider.transform.position);
            float forceMagnitude = separationStrength / Mathf.Max(distance, 0.1f); // Avoid division by zero

            separationForce += directionAway * forceMagnitude;
        }

        return separationForce.normalized; // Normalize the combined force
    }

    public void StartMovement()
    {
        if (isMoving == false)
        {
            isMoving = true;
        }
        else
        {
            Debug.Log("Object already in motion");
        }
    }

    /// <summary>
    /// Stops the object's movement.
    /// </summary>
    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        isMoving = false;

        Debug.Log("Movement stopped.");
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the separation radius in the Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
