using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player's movement
    private Rigidbody2D rb;     // Reference to the Rigidbody2D
    private Vector2 moveInput;  // Input vector for movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure Rigidbody2D is attached
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing!");
        }
    }

    void Update()
    {
        // Get movement input (WASD or arrow keys)
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D velocity
        rb.linearVelocity = moveInput.normalized * moveSpeed;

        // Debug log to verify position updates
        Debug.Log($"Player Position: {transform.position}");
    }
}