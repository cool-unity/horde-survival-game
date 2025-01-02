using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    private Rigidbody2D myRigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask avoidanceLayer;

    private Transform target;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        myRigidbody.velocity = direction * moveSpeed;
    }

    public void MoveInDirection(Vector2 direction)
    {
        myRigidbody.velocity = direction.normalized * moveSpeed;
    }

    public void StopMovement()
    {
        myRigidbody.velocity = Vector2.zero;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void FollowTarget()
    {
        if (target != null)
        {
            MoveTowards(target.position);
        }
    }

    public bool IsAtTarget(Vector2 targetPosition, float threshold = 0.1f)
    {
        return Vector2.Distance(transform.position, targetPosition) <= threshold;
    }

    public void CircleAroundTarget(float circlingSpeed, float circlingRadius)
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 circlingOffset = Vector2.Perpendicular(direction) * circlingRadius;
        Vector2 targetPosition = (Vector2)target.position + circlingOffset;

        MoveTowards(targetPosition);
    }

    public void AvoidCollisions(float separationDistance, float separationStrength)
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, separationDistance, avoidanceLayer);

        Vector2 avoidanceForce = Vector2.zero;

        foreach (var obj in nearbyObjects)
        {
            if (obj.gameObject != gameObject)
            {
                Vector2 diff = (Vector2)transform.position - (Vector2)obj.transform.position;
                avoidanceForce += diff.normalized / diff.magnitude;
            }
        }

        myRigidbody.velocity += avoidanceForce * separationStrength;
    }

    public void FlipSpriteBasedOnDirection(Vector2 velocity)
    {
        if (velocity.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(velocity.x), transform.localScale.y);
        }
    }

    public Vector2 GetVelocity()
    {
        return myRigidbody.velocity;
    }

    private void OnDrawGizmosSelected()
    {
        // Example debug visualization
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f); // Example range
    }
}
