using UnityEngine;

[RequireComponent(typeof(MovementComponent))]
public class AIComponent : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float patrolRange = 5f; // Range of patrolling
    [SerializeField] private float detectionRange = 10f; // Range for detecting the player
    [SerializeField] private float attackRange = 2f; // Range for attacking the player

    private Transform player;
    private Vector2 patrolTarget;
    private bool isChasing;

    private MovementComponent movementComponent;

    private void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();
    }

    private void Start()
    {
        ChooseNewPatrolTarget();
    }

    private void Update()
    {
        // Handle AI decision-making
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    /// <summary>
    /// Sets the player target for the AI.
    /// </summary>
    /// <param name="playerTransform">The player's Transform.</param>
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    /// <summary>
    /// Handles patrolling behavior.
    /// </summary>
    private void Patrol()
    {
        if (movementComponent.IsAtTarget(patrolTarget))
        {
            ChooseNewPatrolTarget();
        }

        movementComponent.MoveTowards(patrolTarget);
    }

    /// <summary>
    /// Handles chasing behavior.
    /// </summary>
    private void ChasePlayer()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            movementComponent.MoveTowards(player.position);
        }
        else
        {
            AttackPlayer();
        }
    }

    /// <summary>
    /// Handles attack behavior.
    /// </summary>
    private void AttackPlayer()
    {
        Debug.Log($"{gameObject.name} is attacking the player!");
        movementComponent.StopMovement();
        // Add attack logic (e.g., apply damage via a DamageComponent or trigger an attack animation)
    }

    /// <summary>
    /// Chooses a new random patrol target within the patrol range.
    /// </summary>
    private void ChooseNewPatrolTarget()
    {
        patrolTarget = (Vector2)transform.position + Random.insideUnitCircle * patrolRange;
        Debug.Log($"{gameObject.name} chose a new patrol target: {patrolTarget}");
    }
}
