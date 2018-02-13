using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public int playerSpeed = 10;
    private bool facingRight = true;
    public int playerJumpPower = 1250;
    public int DashCooldown = 0;
    public int DashImpulse = 0;
    public int playerDashPower = 1000;
    private float moveX;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove();
    }
    //comment

    void PlayerMove() {
        moveX = Input.GetAxis( "Horizontal" );

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


        if ( Input.GetButtonDown( "Jump" ) || Input.GetButtonDown( "Jump (Controller)" ) ) {
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
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );

    }

    void Jump() {
        GetComponent<Rigidbody2D>().AddForce( Vector2.up * playerJumpPower );
    }

    //Dash cooldown is working fine, but can't get the actual dash to work to the extent it should, the 2 solutions below move you all of 3 pixels.
    void Dash() {
        DashCooldown = 50;
        if ( facingRight ) {
            GetComponent<Rigidbody2D>().AddForce( new Vector2 ( playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = 10;
            //GetComponent<Rigidbody2D>().AddForce( Vector2.right * playerDashPower );
            //GetComponent<Rigidbody2D>().velocity = new Vector2( -playerDashPower * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );
        } else {
            GetComponent<Rigidbody2D>().AddForce( new Vector2( -playerDashPower, 0 ), ForceMode2D.Force );
            DashImpulse = -10;
            //GetComponent<Rigidbody2D>().AddForce( Vector2.left * playerDashPower );
            //GetComponent<Rigidbody2D>().velocity = new Vector2( -playerDashPower * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );
        }
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
