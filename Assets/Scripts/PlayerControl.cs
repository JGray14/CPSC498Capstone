using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private Rigidbody2D playerBody;
    private LayerMask groundLayer;
    public int playerSpeed = 10;
    private bool facingRight = true;
    private int playerJumpNum = 0;
    private int playerJumpPower = 1250;
    public int DashCooldown = 0;
    private int DashImpulse = 0;
    public int playerDashPower = 1000;
    private float moveX;
    public int attackLength;
    public bool GroundTest;

    private Animator animator;
    private Animation playerAnimation;
    public float speed = 1f;

    // Use this for initialization
    void Start() {
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<Animation>();
        groundLayer = LayerMask.GetMask( "Ground" );
    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
    }

    //we can change priority of animations in PlayerWarrior animator
    //as well as interupts
    //currently this complicates things as the run animation will always take over
    //even if another animation is trying to play
    void PlayerMove() {
        moveX = Input.GetAxis( "Horizontal" );

        if ( isGrounded() == true ) {
            playerJumpNum = 0;

            if ( playerIsMoving() && DashImpulse == 0 ) {
                animator.Play( "PlayerWarrior_Move" );
            } else {
                animator.Play( "PlayerWarrior_Idle" );
            }
        }

        if ( DashCooldown > 0 ) {
            DashCooldown--;
        }

        if ( DashImpulse > 0 ) {
            playerBody.AddForce( new Vector2( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse--;
        } else if ( DashImpulse < 0 ) {
            playerBody.AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse++;
        }

        if ( ( Input.GetButtonDown( "Jump" ) || Input.GetButtonDown( "Jump (Controller)" ) ) && playerJumpNum < 1 ) {
            Jump();
        }

        if ( ( Input.GetButtonDown( "Dash" ) || Input.GetAxis( "Dash (Controller)" ) == 1 ) && DashCooldown == 0 ) {
            Dash();
        }

        if ( moveX > 0.0f && facingRight == false ) {
            FlipPlayer();
        } else if ( moveX < 0.0f && facingRight == true ) {
            FlipPlayer();
        }
        playerBody.velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );

        if ( Input.GetMouseButtonDown( 0 ) ) {
            Attack1();
        }

        if ( DashImpulse != 0 ) {
            animator.Play( "PlayerWarrior_Skill2" );
        }

    }
    
    //funct to check if char is grounded. not done/working
    bool isGrounded() {
        Vector2 pos = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast( pos, direction, distance, groundLayer );
        //Debug.DrawRay( pos, direction, Color.green );
        return ( GroundTest = hit.collider != null );

    }

    void Jump() {
        playerBody.AddForce( Vector2.up * playerJumpPower );
        animator.Play( "PlayerWarrior_Jump" );
        playerJumpNum++;
    }

    void Dash() {
        DashCooldown = 50;
        if ( facingRight ) {
            playerBody.AddForce( new Vector2( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = 10;

        } else {
            playerBody.AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = -10;
        }
        animator.Play( "PlayerWarrior_Skill2" );
    }

    void Attack1() {
        animator.Play( "HeroWarrior_Attack_part3" );
        attackLength = 20;
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    bool playerIsMoving() {
        return ( moveX != 0 );
    }

    void animate( string animation ) {

    }
}
