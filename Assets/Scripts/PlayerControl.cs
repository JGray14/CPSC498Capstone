using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public int playerSpeed = 10;
    private bool facingRight = true;
    private int playerJumpNum = 0;
    public int playerJumpPower = 1250;
    public int DashCooldown = 0;
    public int DashImpulse = 0;
    public int playerDashPower = 1000;
    private float moveX;

    private Animator animator;
    private Animation playerAnimation;
    public float speed = 1f;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove();
    }
    //comment

    void PlayerMove() {
        moveX = Input.GetAxis( "Horizontal" );
        
        //we can change priority of animations in PlayerWarrior animator
        //as well as interupts
        //currently this complicates things as the run animation will always take over
        //even if another animation is trying to play

        //if (playerIsMoving())
        //{
        //    animator.Play("PlayerWarrior_Move");
        //}
        

        if ( DashCooldown > 0 ) {
            DashCooldown--;
        }
        if ( DashImpulse > 0 ) {
            GetComponent<Rigidbody2D>().AddForce( new Vector2( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse--;
        } else if ( DashImpulse < 0 ) {
            GetComponent<Rigidbody2D>().AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse++;
        }

        if ( (Input.GetButtonDown( "Jump" ) || Input.GetButtonDown( "Jump (Controller)" )) && playerJumpNum != 2) {
            Jump();
            playerJumpNum++;
        }

        if (isGrounded() == true)
        {
            playerJumpNum = 0;
        }


        if ( ( Input.GetButtonDown( "Dash" ) || Input.GetAxis( "Dash (Controller)" ) == 1 ) && DashCooldown == 0 ) {
            Dash();
            
        }

        if ( moveX > 0.0f && facingRight == false ) {
            FlipPlayer();
        } else if ( moveX < 0.0f && facingRight == true ) {
            FlipPlayer();
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );

        if (Input.GetMouseButtonDown(0))
        {
            Attack1();
        }

    }

    //funct to check if char is grounded. not done/working
    bool isGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.0f).collider != null)
        {
            return true;
        }

        return false;
    }

    void Jump() {
        GetComponent<Rigidbody2D>().AddForce( Vector2.up * playerJumpPower );
        animator.Play("PlayerWarrior_Jump");
    }

    void Dash() {
        DashCooldown = 50;
        if ( facingRight ) {
            GetComponent<Rigidbody2D>().AddForce( new Vector2 ( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = 10;

        } else {
            GetComponent<Rigidbody2D>().AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = -10;
            
            
        }
    }

    void Attack1()
    {
        animator.Play("HeroWarrior_Attack_part1");
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    bool playerIsMoving()
    {
        if (moveX != 0)
        {
            return true;
        }
        return false;
    }
}
