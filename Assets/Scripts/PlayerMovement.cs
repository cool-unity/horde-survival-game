using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator animator;

    string currentAnimation;

    const string PLAYER_IDLE = "PlayerIdle";

    const string PLAYER_RUN = "PlayerRun";

    [SerializeField] float runSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
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
        myRigidbody.velocity = new Vector2(moveInput.x*runSpeed, moveInput.y*runSpeed);
        if((Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon) ||
            (Mathf.Abs(myRigidbody.velocity.y)>Mathf.Epsilon)) //Mathf.Epsilon is an infintesimally small number. This is to avoid glitches with the value being really small but not 0.
        {
            ChangeAnimationState(PLAYER_RUN);
        }
        else{
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),transform.localScale.y);
        }
    }


}
