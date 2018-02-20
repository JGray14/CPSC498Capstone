﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    private Rigidbody2D playerBody;
    private LayerMask groundLayer;
    public  int playerSpeed = 10;
    private bool facingRight = true;
    private int playerJumpNum = 0;
    private int jumpBuffer;
    private int playerJumpPower = 1500;
    public  int DashCooldown = 0;
    private int DashImpulse = 0;
    public  int playerDashPower = 1000;
    private float moveX;
    public  int attack1Length;
    public  int attack2Length;
    public  bool GroundTest;
    private string animCall;

    private Animator animator;
    //public  float speed = 1f;
    public  GameObject swordwaveRightPrefab;
    public  GameObject swordwaveLeftPrefab;

    // Use this for initialization
    void Start() {
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask( "Ground" );
    }

    // Update is called once per frame
    void Update() {
        PlayerMove();
    }

    void PlayerMove() {
        moveX = Input.GetAxis( "Horizontal" );
        animCall = "None";
        if ( playerBody.position.y < -20 ) {
            SceneManager.LoadScene( "Testing" );
        }
        if ( isGrounded() == true ) {
            playerJumpNum = 0;

            if ( !playerIsMoving() && DashImpulse == 0 && attack1Length == 0 && attack2Length == 0 && jumpBuffer == 0 ) {
                animCall = "PlayerWarrior_Idle";
            } else if ( DashImpulse == 0 && attack1Length == 0 && attack2Length == 0 && jumpBuffer == 0 ) {
                animCall = "PlayerWarrior_Move";
            }
        } else if ( DashImpulse == 0 && attack1Length == 0 && attack2Length == 0 ) {
            animCall = "PlayerWarrior_Jump";
        }

        if ( jumpBuffer > 0 ) {
            jumpBuffer--;
        }
        if ( attack1Length > 0 ) {
            attack1Length--;
        }
        if ( attack2Length > 0 ) {
            attack2Length--;
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

        if ( ( Input.GetButtonDown( "Jump" ) ) && playerJumpNum < 1 && attack1Length == 0 && attack2Length == 0 ) {
            Jump();
            jumpBuffer = 5;
        }

        if ( ( Input.GetButtonDown( "Dash" ) || Input.GetAxis( "Dash (Controller)" ) == 1 ) && DashCooldown == 0 && attack1Length == 0 && attack2Length == 0 ) {
            Dash();
        }

        if ( moveX > 0.0f && facingRight == false ) {
            FlipPlayer();
        } else if ( moveX < 0.0f && facingRight == true ) {
            FlipPlayer();
        }
        playerBody.velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );

        if ( ( Input.GetButtonDown( "Attack2" ) || Input.GetAxis( "Attack2" ) == 1 ) && attack2Length == 0 && attack1Length == 0 ) {
            //Attack2();
            animCall = "HeroWarrior_Attack_part1";
            Invoke( "Attack2", .35f );
            attack2Length = 30;
        }

        if ( Input.GetButtonDown( "Attack1" ) && attack1Length == 0 && attack2Length == 0 ) {
            Attack1();
            attack1Length = 35;
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
    }

    void Attack2() {
        if ( facingRight ) {
            Vector2 pos = new Vector2( playerBody.transform.position.x + .3f, playerBody.transform.position.y + .8f );
            GameObject bullet = (GameObject)Instantiate( swordwaveRightPrefab, pos, Quaternion.identity );
            bullet.AddComponent<WaveRight>();
        } else {
            Vector2 pos = new Vector2( playerBody.transform.position.x - .3f, playerBody.transform.position.y + .8f );
            GameObject bullet = (GameObject)Instantiate( swordwaveLeftPrefab, pos, Quaternion.identity );
            bullet.AddComponent<WaveLeft>();
        }
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
