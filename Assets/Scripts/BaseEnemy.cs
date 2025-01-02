using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("References")]
    protected MovementComponent movementComponent;
    protected Animator animator;
    protected HealthComponent healthComponent;

    [Header("Enemy Settings")]
    [SerializeField] protected float detectionRange = 10f; // Range to detect the player
    [SerializeField] protected float attackRange = 2f; // Range to perform an attack
    [SerializeField] protected GameObject experienceTokenPrefab; // Token to spawn on death

    protected Transform player;
    protected EnemyState currentState = EnemyState.Idle;

    protected enum EnemyState { Idle, Patrolling, Chasing, Attacking, Dead }

    protected virtual void Awake()
    {
        // Cache references to components
        movementComponent = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.Died += OnDeath; // Subscribe to the death event
        }
    }

    protected virtual void Start()
    {
        Debug.Log($"{gameObject.name} initialized.");
    }

    protected virtual void Update()
    {
        if (currentState == EnemyState.Dead) return;

        HandleState();
    }

    /// <summary>
    /// Handles the enemy's state machine.
    /// </summary>
    protected virtual void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBehavior();
                break;

            case EnemyState.Patrolling:
                PatrolBehavior();
                break;

            case EnemyState.Chasing:
                ChaseBehavior();
                break;

            case EnemyState.Attacking:
                AttackBehavior();
                break;
        }
    }

    /// <summary>
    /// Transitions the enemy to a new state.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    protected void TransitionToState(EnemyState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"{gameObject.name} transitioning from {currentState} to {newState}");
        currentState = newState;

        OnStateEnter(newState);
    }

    /// <summary>
    /// Logic to execute when entering a new state.
    /// </summary>
    /// <param name="state">The state being entered.</param>
    protected virtual void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                movementComponent.StopMovement();
                break;

            case EnemyState.Patrolling:
                StartPatrol();
                break;

            case EnemyState.Chasing:
                StartChase();
                break;

            case EnemyState.Attacking:
                StartAttack();
                break;
        }
    }

    /// <summary>
    /// Behavior when in the Idle state.
    /// </summary>
    protected virtual void IdleBehavior()
    {
        if (PlayerInDetectionRange())
        {
            TransitionToState(EnemyState.Chasing);
        }
    }

    /// <summary>
    /// Behavior when in the Patrolling state.
    /// </summary>
    protected virtual void PatrolBehavior()
    {
        // Example: Move between waypoints
        if (PlayerInDetectionRange())
        {
            TransitionToState(EnemyState.Chasing);
        }
    }

    /// <summary>
    /// Behavior when in the Chasing state.
    /// </summary>
    protected virtual void ChaseBehavior()
    {
        if (PlayerInAttackRange())
        {
            TransitionToState(EnemyState.Attacking);
        }
        else if (!PlayerInDetectionRange())
        {
            TransitionToState(EnemyState.Patrolling);
        }
        else
        {
            movementComponent.MoveTowards(player.position);
        }
    }

    /// <summary>
    /// Behavior when in the Attacking state.
    /// </summary>
    protected virtual void AttackBehavior()
    {
        if (!PlayerInAttackRange())
        {
            TransitionToState(EnemyState.Chasing);
            return;
        }

        PerformAttack();
    }

    /// <summary>
    /// Starts patrolling logic.
    /// </summary>
    protected virtual void StartPatrol()
    {
        // Define patrolling behavior here
    }

    /// <summary>
    /// Starts chasing logic.
    /// </summary>
    protected virtual void StartChase()
    {
        if (player != null)
        {
            movementComponent.SetTarget(player);
        }
    }

    /// <summary>
    /// Starts attacking logic.
    /// </summary>
    protected virtual void StartAttack()
    {
        movementComponent.StopMovement();
    }

    /// <summary>
    /// Performs the attack.
    /// </summary>
    protected virtual void PerformAttack()
    {
        Debug.Log($"{gameObject.name} attacks the player!");
        // Implement attack logic here (e.g., apply damage to the player)
    }

    /// <summary>
    /// Handles the enemy's death logic.
    /// </summary>
    protected virtual void OnDeath()
    {
        Debug.Log($"{gameObject.name} has died.");
        TransitionToState(EnemyState.Dead);

        movementComponent.StopMovement();

        if (experienceTokenPrefab != null)
        {
            Instantiate(experienceTokenPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f); // Adjust delay for death animation or effects
    }

    /// <summary>
    /// Checks if the player is within the detection range.
    /// </summary>
    /// <returns>True if the player is within detection range, false otherwise.</returns>
    protected bool PlayerInDetectionRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    /// <summary>
    /// Checks if the player is within attack range.
    /// </summary>
    /// <returns>True if the player is within attack range, false otherwise.</returns>
    protected bool PlayerInAttackRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    /// <summary>
    /// Sets the player reference for the enemy.
    /// </summary>
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
}
