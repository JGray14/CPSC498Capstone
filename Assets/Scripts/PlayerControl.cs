using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private Rigidbody2D playerBody;
    private LayerMask groundLayer;
    public int playerSpeed = 10;
    private bool facingRight = true;
    private int playerJumpNum = 0;
    private int playerJumpPower = 1500;
    public int DashCooldown = 0;
    private int DashImpulse = 0;
    public int playerDashPower = 1000;
    private float moveX;
    public int attackLength;
    public bool GroundTest;
    private string animCall;
    private int jumpBuffer;

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

    void PlayerMove() {
        moveX = Input.GetAxis( "Horizontal" );
        animCall = "None";
        if ( isGrounded() == true ) {
            playerJumpNum = 0;

            if ( !playerIsMoving() && DashImpulse == 0 && attackLength == 0 && jumpBuffer == 0 ) {
                animCall = "PlayerWarrior_Idle";
            } else if ( DashImpulse == 0 && attackLength == 0 && jumpBuffer == 0 ) {
                animCall = "PlayerWarrior_Move";
            }
        } else if ( DashImpulse == 0 && attackLength == 0 ) {
            animCall = "PlayerWarrior_Jump";
        }

        if ( jumpBuffer > 0 ) {
            jumpBuffer--;
        }
        if ( attackLength > 0 ) {
            attackLength--;
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

        if ( ( Input.GetButtonDown( "Jump" ) || Input.GetButtonDown( "Jump (Controller)" ) ) && playerJumpNum < 1 && attackLength == 0 ) {
            Jump();
            jumpBuffer = 5;
        }

        if ( ( Input.GetButtonDown( "Dash" ) || Input.GetAxis( "Dash (Controller)" ) == 1 ) && DashCooldown == 0 && attackLength == 0 ) {
            Dash();
        }

        if ( moveX > 0.0f && facingRight == false ) {
            FlipPlayer();
        } else if ( moveX < 0.0f && facingRight == true ) {
            FlipPlayer();
        }
        playerBody.velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );

        if ( Input.GetMouseButtonDown( 0 ) && attackLength == 0 ) {
            Attack1();
            attackLength = 35;
        }

        //Priority should be handled by code order
        //seperate method turned out to not work well
        if ( animCall != "None" ) {
            animator.Play( animCall );
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
        playerBody.velocity = new Vector3( playerBody.velocity.x, 0, 0 );
        playerBody.AddForce( Vector2.up * playerJumpPower );
        animCall = "PlayerWarrior_Jump";
        playerJumpNum++;
    }

    void Dash() {
        //Shouldn't be able to dash mid-attack
        //if ( prevAnimation == "HeroWarrior_Attack_part3" ) {
        //    return;
        //}

        DashCooldown = 50;
        if ( facingRight ) {
            playerBody.AddForce( new Vector2( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = 18;
        } else {
            playerBody.AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = -18;
        }
        animCall = "PlayerWarrior_Skill2";
    }

    void Attack1() {
        animCall = "HeroWarrior_Attack_part3";
        attackLength = 20;
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    bool playerIsMoving() {
        if ( playerJumpNum > 0 ) {
            return ( true );
        }
        return ( moveX != 0 );
    }
}
