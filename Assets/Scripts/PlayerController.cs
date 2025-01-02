using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
public class PlayerController : MonoBehaviour
{
    private MovementComponent movementComponent;
    private PlayerInputActions playerInputActions;
    private Vector2 movementInput;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private string idleAnimation = "PlayerIdle";
    [SerializeField] private string runAnimation = "PlayerRun";

    private string currentAnimation;

    private void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();

        // Initialize the Input Actions
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the input action map
        playerInputActions.Player.Enable();

        // Subscribe to the Move action
        playerInputActions.Player.Move.performed += OnMovePerformed;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        // Disable the input action map
        playerInputActions.Player.Disable();

        // Unsubscribe from the Move action
        playerInputActions.Player.Move.performed -= OnMovePerformed;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;
    }

    private void Update()
    {
        // Use the movement input to move the player
        movementComponent.MoveInDirection(movementInput);

        // Handle animations based on movement input
        HandleAnimations();

        // Flip the sprite based on movement direction
        FlipSprite();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Read the movement input as a Vector2
        movementInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Reset the movement input when movement stops
        movementInput = Vector2.zero;
    }

    private void HandleAnimations()
    {
        if (movementInput.magnitude > Mathf.Epsilon) // Player is moving
        {
            ChangeAnimationState(runAnimation);
        }
        else // Player is idle
        {
            ChangeAnimationState(idleAnimation);
        }
    }

    private void FlipSprite()
    {
        // Flip the sprite horizontally based on movement direction
        if (movementInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movementInput.x), 1, 1);
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
