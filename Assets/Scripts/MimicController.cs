using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicController : MonoBehaviour {

    private Rigidbody2D body;
    private LayerMask playerLayer;
    public LayerMask groundLayer;
    private GameObject player;
    public GameObject heartPickupPrefab;
    private Color normalColor;
    private RaycastHit2D UpCheck, DownCheck, RightCheck, LeftCheck;
    private int health;
    private float speed;
    private int iFrames;
    private int cooldown;
    private bool moving;
    private bool movingUp, movingDown, movingRight, movingLeft;
    private bool die;

    private int meleeAtkDamage = 1;
    private int rangedAtkDamage = 1;

    // Use this for initialization
    void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.GetMask( "Player" );
        groundLayer = LayerMask.GetMask( "Ground" );
        player = GameObject.FindGameObjectWithTag( "Player" );
        normalColor = GetComponent<Renderer>().material.color;
        health = 3;
        speed = 0.25f;
        moving = false;
        movingUp = false;
        movingDown = false;
        movingRight = false;
        movingLeft = false;
        die = false;
        cooldown = 0;
	}
	
	// Update is called once per frame
	void Update () {
        body.velocity = new Vector3( 0, 0, 0 );
        if ( iFrames > 0 ) {
            iFrames--;
        }
        if ( health < 1 ) {
            die = true;
        }
        if ( cooldown > 0 ) {
            cooldown--;
        }

        Vector2 pos = transform.position;
        float distance = 10.0f;
        RaycastHit2D Up = Physics2D.Raycast( pos, Vector2.up, distance, playerLayer );
        RaycastHit2D Down = Physics2D.Raycast( pos, Vector2.down, distance, playerLayer );
        RaycastHit2D Right = Physics2D.Raycast( pos, Vector2.right, distance, playerLayer );
        RaycastHit2D Left = Physics2D.Raycast( pos, Vector2.left, distance, playerLayer );
        if ( Up.collider != null && !moving && cooldown <= 0 ) {
            movingUp = true;
            moving = true;
        } else if ( Down.collider != null && !moving && cooldown <= 0 ) {
            movingDown = true;
            moving = true;
        } else if ( Right.collider != null && !moving && cooldown <= 0 ) {
            movingRight = true;
            moving = true;
        } else if ( Left.collider != null && !moving && cooldown <= 0 ) {
            movingLeft = true;
            moving = true;
        }

        if ( movingUp ) {
            transform.position = new Vector2( transform.position.x, transform.position.y + speed );
            UpCheck = Physics2D.Raycast( transform.position, Vector2.up, 0.5f, groundLayer );
            if ( UpCheck.collider != null ) {
                movingUp = false;
                moving = false;
                cooldown = 30;
            }
        } else if ( movingDown ) {
            transform.position = new Vector2( transform.position.x, transform.position.y - speed );
            DownCheck = Physics2D.Raycast( transform.position, Vector2.down, 0.5f, groundLayer );
            if ( DownCheck.collider != null ) {
                movingDown = false;
                moving = false;
                cooldown = 30;
            }
        } else if( movingRight ) {
            transform.position = new Vector2( transform.position.x + speed, transform.position.y );
            RightCheck = Physics2D.Raycast( transform.position, Vector2.right, 0.5f, groundLayer );
            if ( RightCheck.collider != null ) {
                movingRight = false;
                moving = false;
                cooldown = 30;
            }
        } else if ( movingLeft ) {
            transform.position = new Vector2( transform.position.x - speed, transform.position.y );
            LeftCheck = Physics2D.Raycast( transform.position, Vector2.left, 0.5f, groundLayer );
            if ( LeftCheck.collider != null ) {
                movingLeft = false;
                moving = false;
                cooldown = 30;
            }
        }
    }

    void LateUpdate() {
        if ( die ) {
            StartCoroutine( Kill() );
        }
    }

    IEnumerator Hit() {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds( .1f );
        GetComponent<Renderer>().material.color = normalColor;
        yield return new WaitForSeconds( .1f );
    }

    IEnumerator Kill() {
        yield return new WaitForSeconds( .4f );
        Instantiate( heartPickupPrefab, gameObject.transform.position, Quaternion.identity );
        DestroyObject( gameObject );
    }

    public void damage( int value, int force, Vector2 direction ) {
        health -= value;
        body.AddForce( direction * force );
        StartCoroutine( Hit() );
    }

    //if enemy is hit
    private void OnTriggerEnter2D( Collider2D other ) {
        if ( iFrames <= 0 ) {
            if ( other.tag == "PlayerHurtbox" ) {
                damage( meleeAtkDamage, 0, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                damage( rangedAtkDamage, 0, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            }
        }
        if ( other.tag == "Spikes" ) {
            StartCoroutine( Kill() );
        }
    }
}
