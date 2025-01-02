using UnityEngine;

public class ProjectileComponent : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f; // Speed of the projectile
    [SerializeField] private float lifespan = 5f; // Lifespan before the projectile is destroyed
    [SerializeField] private int damage = 10; // Damage dealt by the projectile

    private Vector2 direction;

    private void Start()
    {
        // Destroy the projectile after its lifespan
        Destroy(gameObject, lifespan);
    }

    private void Update()
    {
        Move();
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction.normalized; // Ensure the direction is normalized
    }

    private void Move()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var healthComponent = collision.gameObject.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage); // Apply damage to the target
        }

        // Destroy the projectile upon collision
        Destroy(gameObject);
    }
}