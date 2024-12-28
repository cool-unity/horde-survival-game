using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    protected Rigidbody2D myRigidbody;
    protected Animator animator;
    public GameObject experienceTokenPrefab;

    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 5f; // Default movement speed
    [SerializeField] protected float followRange = 10f; // Range within which the enemy follows
    [SerializeField] protected bool ignoreFollowRange = true; // Default to true to always follow the player
    [SerializeField] private float circlingSpeed = 1f; // Speed of circling around the player
    [SerializeField] private float circlingRadius = 2f; // Radius of circling around the player
    [SerializeField] private float separationDistance = 1.5f; // Distance to maintain from other enemies
    [SerializeField] private float separationStrength = 2f; // Strength of separation force

    private bool isFollowing = false; // Tracks whether the enemy is currently following the player
    private bool hasLoggedWarning = false; // Ensures the player-null warning is logged only once

    protected enum EnemyState { Idle, Following, Attacking, Dead }
    protected EnemyState currentState = EnemyState.Idle; // Enemy's current state

    private HealthComponent healthComponent;

    // New field for spawner reference
    private EnemySpawner spawner;

    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthComponent = GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
            healthComponent.Died += OnDeath; // Subscribe to the death event
        }

        // Log initialization for the enemy
        Debug.Log($"BaseEnemy initialized for {gameObject.name} with moveSpeed: {moveSpeed}, followRange: {followRange}, ignoreFollowRange: {ignoreFollowRange}");
    }

    protected virtual void Update()
    {
        if (currentState != EnemyState.Dead)
        {
            HandleState();
        }
    }

    protected virtual void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Following:
                FollowPlayerWithCirclingAndSeparation();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
            case EnemyState.Dead:
                StopMovement(); // Ensure the enemy stops all activity
                break;
        }
    }

    public virtual void Initialize(EnemySpawner spawnerInstance, Transform playerTransform)
    {
        spawner = spawnerInstance;
        player = playerTransform;
        Debug.Log($"{gameObject.name} initialized with spawner and player reference.");
    }

    protected virtual void Idle()
    {
        StopMovement(); // Default idle behavior
    }

    protected void FollowPlayerWithCirclingAndSeparation()
    {
        if (player == null)
        {
            if (!hasLoggedWarning)
            {
                // Log a warning if the player is null, but only once
                Debug.LogWarning($"{gameObject.name} has no player assigned! Please assign a player Transform.");
                hasLoggedWarning = true;
            }
            return;
        }

        // Calculate distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check whether the enemy should follow the player
        if (ignoreFollowRange || distanceToPlayer <= followRange)
        {
            if (!isFollowing)
            {
                // Log once when the enemy starts following the player
                Debug.Log($"{gameObject.name} started following the player.");
                isFollowing = true;
            }

            // Circling and avoidance logic
            Vector2 circlingVelocity = MoveTowardsPlayerWithCircling(circlingSpeed, circlingRadius);
            Vector2 avoidanceVelocity = AvoidOtherEnemies(separationDistance, separationStrength);

            // Combine velocities
            myRigidbody.linearVelocity = circlingVelocity + avoidanceVelocity;
        }
        else
        {
            if (isFollowing)
            {
                // Log once when the enemy stops following the player
                Debug.Log($"{gameObject.name} stopped following the player.");
                isFollowing = false;
            }
            SetState(EnemyState.Idle); // Switch to idle state
        }

        FlipSprite();
    }

    protected virtual Vector2 MoveTowardsPlayerWithCircling(float circlingSpeed, float circlingRadius)
    {
        // Direction toward the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Calculate target position for circling
        Vector2 targetPosition = (Vector2)player.position + (Vector2.Perpendicular(direction) * circlingRadius);

        // Velocity for circling
        Vector2 circlingVelocity = (targetPosition - (Vector2)transform.position).normalized * moveSpeed;

        // Add rotation effect for circling
        circlingVelocity += new Vector2(-direction.y, direction.x) * circlingSpeed;

        return circlingVelocity;
    }

    protected virtual Vector2 AvoidOtherEnemies(float separationDistance, float separationStrength)
    {
        Vector2 avoidance = Vector2.zero;

        // Find nearby enemies
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy.gameObject != gameObject) // Ignore self
            {
                Vector2 diff = (Vector2)transform.position - (Vector2)enemy.transform.position;
                avoidance += diff.normalized / diff.magnitude; // Repelling force
            }
        }

        return avoidance * separationStrength;
    }

    protected virtual void StopMovement()
    {
        // Stop moving
        myRigidbody.linearVelocity = Vector2.zero;
    }

    protected virtual void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking the player!");
        // Implement attack logic here
    }

    protected virtual void OnDeath()
    {
        Debug.Log($"{gameObject.name} is dead.");
        SetState(EnemyState.Dead);

        // Notify the spawner that the enemy is destroyed
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed();
        }

        // Spawn experience token
        if (experienceTokenPrefab != null)
        {
            Instantiate(experienceTokenPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 2f); // Adjust delay as needed depending on animation
    }


    protected void FlipSprite()
    {
        if (myRigidbody.linearVelocity.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), transform.localScale.y);
        }
    }

    protected void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"{gameObject.name} changed state from {currentState} to {newState}");
        currentState = newState;
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
        Debug.Log($"{gameObject.name} assigned player: {playerTransform.name}");
    }

    // New method to set the spawner reference
    public void SetSpawner(EnemySpawner spawnerInstance)
    {
        spawner = spawnerInstance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followRange); // Follow range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationDistance); // Separation distance
    }
}
