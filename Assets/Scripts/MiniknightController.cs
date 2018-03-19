using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniknightController : MonoBehaviour {

    public Rigidbody2D body;
    private LayerMask groundLayer;
    private Animator animator;
    private GameObject player;
    public int health;
    public  int speed = 200;
    private int jumpNum;
    public  bool Grounded;
    private int jumpBuffer;
    private int jumpPower = 1500;
    private bool facingRight = false;
    public float distanceFromPlayer;
    public bool right, left;
    public string animCall;
    public string prevCall;
    private bool moveRight, moveLeft;
    private bool patrolMoveRight, patrolMoveLeft;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        health = 3;
        jumpNum = 0;
        groundLayer = LayerMask.GetMask( "Ground" );
        player = GameObject.FindGameObjectWithTag( "Player" );
        moveRight = false;
        moveLeft = false;
        prevCall = "None";
    }

    // Update is called once per frame
    void Update() {
        animCall = "Idle";
        if ( isGrounded() == true ) {
            jumpNum = 0;
        }

        if ( moveRight || patrolMoveRight ) {
            body.AddForce( Vector2.right * speed * 4 );
            moveRight = false;
            faceRight();
            animCall = "Walk";
        } else if ( moveLeft || patrolMoveLeft ) {
            body.AddForce( Vector2.left * speed * 4 );
            moveLeft = false;
            faceLeft();
            animCall = "Walk";
        }

        if ( facingRight ) {
            faceRight();
        } else {
            faceLeft();
        }

        //GetComponent<Rigidbody2D>().velocity = new Vector2( -speed, body.velocity.y );

        //if ( body.velocity.x > 0.0f && facingRight == false ) {
        //    Flip();
        //} else if ( body.velocity.x < 0.0f && facingRight == true ) {
        //    Flip();
        //}
        if ( animCall != prevCall ) {
            animator.Play( animCall );
        }
        prevCall = animCall;

        
        //if(health <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    bool isGrounded() {
        Vector2 pos = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast( pos, direction, distance, groundLayer );
        //Debug.DrawRay( pos, direction, Color.green );
        return ( Grounded = hit.collider != null );
    }

    void Flip() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
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
    
    [Task]
    void patrolLeft() {
        body.AddForce( Vector2.left * speed * 4 );
        faceLeft();
    }

    [Task]
    bool checkLeft() {
        Vector2 pos = body.transform.position;
        float distance = 1.0f;
        //Debug.DrawRay( pos, new Vector3( -1, -1, 0 ), Color.green, 10, false );
        RaycastHit2D wall = Physics2D.Raycast( pos, Vector2.left, distance, groundLayer );
        Vector2 leftpos = new Vector2( body.transform.position.x - 1, body.transform.position.y );
        RaycastHit2D ground = Physics2D.Raycast( leftpos, Vector2.down, 1.0f, groundLayer );
        return ( left = ( wall.collider != null || ground.collider == null ) );
    }
    
    [Task]
    void patrolRight() {
        body.AddForce( Vector2.right * speed * 4 );
        faceRight();
    }

    [Task]
    bool checkRight() {
        Vector2 pos = body.transform.position;
        Vector2 diagRight = new Vector2( 1, -1 );
        float distance = 1.0f;
        RaycastHit2D wall = Physics2D.Raycast( pos, Vector2.right, distance, groundLayer );
        Vector2 rightpos = new Vector2( body.transform.position.x + 1, body.transform.position.y );
        RaycastHit2D ground = Physics2D.Raycast( rightpos, Vector2.down, 1.0f, groundLayer );
        return ( right = ( wall.collider != null || ground.collider == null ) );
    }

    [Task]
    bool playerNearby() {
        distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, body.position );
        return ( distanceFromPlayer < 10 );
    }

    [Task]
    void hunt() {
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
            moveLeft = true;
        } else {
            faceRight();
            moveRight = true;
        }
    }

    //if enemy is hit
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            health--;
        }
    }
}
