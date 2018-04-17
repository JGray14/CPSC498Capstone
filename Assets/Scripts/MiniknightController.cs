using System.Collections;
using Panda;
using UnityEngine;

public class MiniknightController : MonoBehaviour, Enemy {

    public Rigidbody2D body;
    private LayerMask groundLayer;
    private LayerMask playerLayer;
    private Animator animator;
    private GameObject player;
    public GameObject heartPickupPrefab;

    public int health;
    public  int speed = 200;
    public bool attackHit = false;
    public int iFrames = 0;

    public  bool Grounded;
    private bool facingRight = false;
    private float distanceFromPlayer;
    private bool right, left;
    private string animCall;
    private bool moveRight, moveLeft;
    private bool die;

    private Color normalColor;

    //Damage/knockback values
    private int meleeAtkDamage = 1;
    private int meleeKnockback = 1500;
    private int rangedAtkDamage = 1;
    private int rangedKnockback = 1000;


    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = 3;
        groundLayer = LayerMask.GetMask( "Ground" );
        playerLayer = LayerMask.GetMask( "Player" );
        player = GameObject.FindGameObjectWithTag( "Player" );
        moveRight = false;
        moveLeft = false;
        die = false;
        normalColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update() {
        animCall = "None";
        if ( health < 1 ) {
            die = true;
        }
        animCall = "Walk";
        //if ( isGrounded() == true ) {
        //    jumpNum = 0;
        //}

        if ( iFrames > 0 ) {
            iFrames--;
        }
        if ( moveRight ) {
            body.AddForce( Vector2.right * speed * 4 );
            moveRight = false;
            faceRight();
            animCall = "Walk";
        } else if ( moveLeft ) {
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

        Vector2 pos = body.transform.position;
        RaycastHit2D hit;
        if ( facingRight ) {
            hit = Physics2D.Raycast( pos, Vector2.right, 2.5f, playerLayer );
        } else {
            hit = Physics2D.Raycast( pos, Vector2.left, 2.5f, playerLayer );
        }
        if ( hit.collider != null ) {
            animCall = "Attack";
        }

        if ( animCall != "None" && die != true ) {
            animator.Play( animCall );
        }
    }

    void LateUpdate() {
        if ( die ) {
            StartCoroutine( Kill() );
        }
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
        float distance = 1.0f;
        RaycastHit2D wall = Physics2D.Raycast( pos, Vector2.right, distance, groundLayer );
        Vector2 rightpos = new Vector2( body.transform.position.x + 1, body.transform.position.y );
        RaycastHit2D ground = Physics2D.Raycast( rightpos, Vector2.down, 1.0f, groundLayer );
        return ( right = ( wall.collider != null || ground.collider == null ) );
    }

    [Task]
    bool playerNearby() {
        if ( player != null ) {
            distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, body.position );
            return ( distanceFromPlayer < 10 || health < 3 );
        }
        return ( false );
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

    IEnumerator Hit() {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds( .1f );
        GetComponent<Renderer>().material.color = normalColor;
        yield return new WaitForSeconds( .1f );
    }

    IEnumerator Kill() {
        animator.Play( "Die" );
        yield return new WaitForSeconds( .4f );
        Instantiate( heartPickupPrefab, gameObject.transform.position, Quaternion.identity );
        DestroyObject( gameObject );
    }

    IEnumerator WaitToHit() {
        yield return new WaitForSecondsRealtime(4);
        attackHit = false;
    }

    public void damage( int value, int force, Vector2 direction ) {
        health -= value;
        body.AddForce( direction * force );
        StartCoroutine( Hit() );
    }

    //if enemy is hit
    private void OnTriggerEnter2D(Collider2D other) {
        if ( iFrames <= 0 ) {
            if ( other.tag == "PlayerHurtbox" ) {
                damage( meleeAtkDamage, meleeKnockback, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                damage( rangedAtkDamage, rangedKnockback, Vector3.Normalize( body.transform.position - other.gameObject.transform.position ) );
                iFrames = 20;
            }
        }
        if ( other.tag == "Spikes" ) {
            StartCoroutine( Kill() );
        }
    }
}
