using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayerComponent : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator animator;

    string currentAnimation;

    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_RUN = "PlayerRun";

    [SerializeField] float runSpeed = 10f;
    
    // Player Components
    protected HealthComponent playerHealth;
    protected AttackComponent playerBasicAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<HealthComponent>();
        playerBasicAttack = GetComponent<AttackComponent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }

    public void ChangeAnimationState(string newAnimation){
        if(currentAnimation == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    void Run()
    {
        playerRigidbody.linearVelocity = new Vector2(moveInput.x*runSpeed, moveInput.y*runSpeed);
        if((Mathf.Abs(playerRigidbody.linearVelocity.x)>Mathf.Epsilon) ||
           (Mathf.Abs(playerRigidbody.linearVelocity.y)>Mathf.Epsilon)) //Mathf.Epsilon is an infintesimally small number. This is to avoid glitches with the value being really small but not 0.
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else{
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.linearVelocity.x)>Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.linearVelocity.x),transform.localScale.y);
        }
    }
}