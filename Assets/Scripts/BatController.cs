using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BatController : MonoBehaviour, Enemy {

    public Rigidbody2D body;
    private Animator animator;
    private GameObject player;
    public GameObject heartPickupPrefab;
    private LayerMask groundLayer;
    private Vector2 currentPos;
    private Vector2 origin;

    private Color normalColor;

    public int health;
    public  int speed = 2;
    private string animCall;
    public int iFrames = 0;

    private bool facingRight = false;
    private float distanceFromPlayer;
    private bool moveRight, moveLeft;
    private bool die;
    private bool hunting;
    private bool roosting;

    //Damage/knockback values
    private int meleeAtkDamage = 1;
    private int meleeKnockback = 500;
    private int rangedAtkDamage = 1;
    private int rangedKnockback = 500;

    private float RotateSpeed = 1f;
    private float Radius = 2f;

    private Vector2 _centre;
    private float angle;

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag( "Player" );
        groundLayer = LayerMask.GetMask( "Ground" );
        normalColor = GetComponent<Renderer>().material.color;
        origin = transform.position;
        health = 3;
        angle = 0;
        hunting = false;
        roosting = true;
    }

    // Update is called once per frame
    void Update() {
        currentPos = transform.position;
        if ( iFrames > 0 ) {
            iFrames--;
        }
        if ( health < 1 ) {
            die = true;
        }

        if ( facingRight ) {
            faceRight();
        } else {
            faceLeft();
        }

        if ( hunting ) {
            body.AddForce( ( new Vector2( player.transform.position.x, player.transform.position.y + 1f ) - new Vector2( transform.position.x, transform.position.y ) ).normalized * 4 * speed );
            //transform.position = Vector2.MoveTowards( transform.position, new Vector2(player.transform.position.x, player.transform.position.y + 1) , 3*speed * Time.deltaTime );
        } else if ( roosting ) {
            hunting = false;
            Vector2 pos = body.transform.position;
            float distance = 10.0f;
            RaycastHit2D cieling = Physics2D.Raycast( pos, Vector2.up, distance, groundLayer );
            if ( cieling ) {
                transform.position = Vector2.MoveTowards( pos, cieling.transform.position, speed * Time.deltaTime );
            }
        }

        animator.Play( "batGrayFly" );
    }

    void LateUpdate() {
        if ( die ) {
            StartCoroutine( Kill() );
        }
    }

    [Task]
    bool playerNearby() {
        if ( player != null ) {
            distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, body.position );
            return ( distanceFromPlayer < 8 || health < 3 );
        }
        return ( false );
    }

    [Task]
    void hunt() {
        roosting = false;
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
        } else {
            faceRight();
        }
        hunting = true;
    }

    [Task]
    bool roost() {
        hunting = false;
        Vector2 pos = body.transform.position;
        float distance = 10.0f;
        RaycastHit2D cieling = Physics2D.Raycast( pos, Vector2.up, distance, groundLayer );
        if ( cieling ) {
            roosting = true;
            //transform.position = Vector2.MoveTowards( pos, cieling.transform.position, speed * Time.deltaTime );
            return ( true );
        }
        return ( false );
    }

    //Vector between 2 points
    //Vector3 newVector = targetPoint - initialPoint;
    [Task]
    void patrol() {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float distanceFromOrigin = Vector2.Distance( origin, body.position );
        if ( distanceFromOrigin > 20 ) {
            body.AddForce( ( origin - new Vector2( posX, posY ) ).normalized * speed );
        } else {
            if ( posX < origin.x && posY > origin.y ) { //Upper Left
                body.AddForce( ( Vector2.right * 2 + Vector2.down ) * speed );
            } else if ( posX > origin.x && posY > origin.y ) { //Upper Right
                body.AddForce( ( Vector2.right + Vector2.down * 2 ) * speed );
            } else if ( posX < origin.x && posY < origin.y ) { //Bottom Left
                body.AddForce( ( Vector2.left * 2 + Vector2.up ) * speed );
            } else if ( posX > origin.x && posY < origin.y ) { //Bottom Right
                body.AddForce( ( Vector2.left + Vector2.up * 2 ) * speed );
            }
        }
    }

    void temp() {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float distanceFromOrigin = Vector2.Distance( origin, body.position );
        if ( distanceFromOrigin > 20 ) {
            body.AddForce( ( origin - new Vector2( posX, posY ) ).normalized * speed );
        } else {
            if ( posX < origin.x && posY > origin.y ) { //Upper Left
                body.AddForce( ( Vector2.right * 2 + Vector2.down ) * speed );
            } else if ( posX > origin.x && posY > origin.y ) { //Upper Right
                body.AddForce( ( Vector2.right + Vector2.down * 2 ) * speed );
            } else if ( posX < origin.x && posY < origin.y ) { //Bottom Left
                body.AddForce( ( Vector2.left * 2 + Vector2.up ) * speed );
            } else if ( posX > origin.x && posY < origin.y ) { //Bottom Right
                body.AddForce( ( Vector2.left + Vector2.up * 2 ) * speed );
            }
        }
    }

    void temp2() {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float distanceFromOrigin = Vector2.Distance( origin, body.position );
        if ( distanceFromOrigin > 20 ) {
            body.AddForce( ( origin - new Vector2( posX, posY ) ).normalized * speed );
        } else {
            angle += RotateSpeed * Time.deltaTime;
            var offset = new Vector2( Mathf.Sin( angle ), Mathf.Cos( angle ) ) * Radius;
            transform.position = origin + offset;
        }
    }

    void faceRight() {
        if ( !facingRight ) {
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            facingRight = !facingRight;
        }
    }

    void faceLeft() {
        if ( facingRight ) {
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            facingRight = !facingRight;
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
                damage( meleeAtkDamage, meleeKnockback, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                damage( rangedAtkDamage, rangedKnockback, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            }
        }
        if ( other.gameObject.tag == "Spikes" ) {
            StartCoroutine( Kill() );
        }
    }
}
