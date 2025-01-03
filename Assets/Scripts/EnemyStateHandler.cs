using System;
using UnityEngine;

public class EnemyStateHandler : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Dead
    }

    [Header("State Settings")]
    [SerializeField] private EnemyState currentState = EnemyState.Chasing;
    [SerializeField] private GameObject target = null;
    [SerializeField] private float stoppingDistance = 0.02f;
    [SerializeField] private float chaseDistance = 5f;
    [SerializeField] private float attackDistance = 1f;

    private BaseEnemyMovement movement;
    private BaseEnemyAttack attack;

    private void Start()
    {
        movement = GetComponent<BaseEnemyMovement>();
        attack = GetComponent<BaseEnemyAttack>();
        target = GameObject.FindGameObjectWithTag("Player");

        if (target == null)
        {
            Debug.LogError("Player Game Object is not assigned!");
        }

        if (movement == null || attack == null)
        {
            Debug.LogError("Required components (EnemyMovement, EnemyAttack) are missing!");
        }
    }

    private void Update()
    {
        // switch (currentState)
        // {
        //     case EnemyState.Idle:
        //         HandleIdle();
        //         break;
        //     case EnemyState.Chasing:
        //         HandleChasing();
        //         break;
        //     case EnemyState.Attacking:
        //         HandleAttacking();
        //         break;
        //     case EnemyState.Dead:
        //         HandleDead();
        //         break;
        // }


        HandleChasing();
        UpdateState();
    }

    private void HandleIdle()
    {
        throw new NotImplementedException();
    }

    private void HandleChasing()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null. Cannot chase.");
            return;
        }

        // Get the player's current position
        Vector2 targetPosition = target.transform.position;

        // Calculate the distance to the player
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        
        // Debug.Log($"Chasing target. Distance to target: {distanceToTarget}");
        
        // Move towards the player if beyond stopping distance
        if (distanceToTarget > stoppingDistance)
        {
            movement.SetTargetPosition(targetPosition);
        }
        else
        {
            movement.StopMovement();
            Debug.Log("Player is within stopping distance.");
        }
    }

    private void HandleAttacking()
    {
        throw new NotImplementedException();
    }

    private void HandleDead()
    {
        throw new NotImplementedException();
    }

    private void UpdateState()
    {
        currentState = EnemyState.Chasing;
        // TODO: Implement logic to handle updating the remaining states
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}
