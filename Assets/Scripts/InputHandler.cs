using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
public class InputHandler : MonoBehaviour
{
    private MovementComponent movementComponent;
    private Animator animator;

    [Header("Animation Settings")]
    [SerializeField] private string idleAnimation = "PlayerIdle";
    [SerializeField] private string runAnimation = "PlayerRun";

    private string currentAnimation;

    private void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        movementComponent.MoveInDirection(moveInput);
    }

    private void HandleAnimation()
    {
        Vector2 velocity = movementComponent.GetVelocity();

        if (velocity.magnitude > Mathf.Epsilon)
        {
            ChangeAnimationState(runAnimation);
        }
        else
        {
            ChangeAnimationState(idleAnimation);
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}