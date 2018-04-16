using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    private Rigidbody2D playerBody;
    private LayerMask groundLayer;
    private Animator animator;
    public  GameObject swordwaveRightPrefab;
    public  GameObject swordwaveLeftPrefab;
    public  Canvas deathGUI;

    private Color normalColor;
    private Color dashColor;
    private Color hitColor;
    private Color healColor;

    private float moveX;
    public int playerHealth;
    public  int playerSpeed = 10;
    private int playerJumpNum = 0;
    private int jumpBuffer;
    private int playerJumpPower = 1200; //was 1500
    public bool hasDash;
    public  int DashCooldown = 0;
    private int DashImpulse = 0;
    public  int playerDashPower = 1000;
    private bool facingRight = true;
    public  bool Grounded;
    public  int attack1Length;
    public  int attack2Length;
    private string animCall;
    public int iFrames = 0;

    //Damage/knockback values
    private int meleeAtkDamage = 1;
    private int meleeKnockback = 1500;
    private int rangedAtkDamage = 1;
    private int rangedKnockback = 1000;


    // Use this for initialization
    void Start() {
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        playerHealth = 6;
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask( "Ground" );
        normalColor = GetComponent<Renderer>().material.color;
        dashColor = Color.cyan;
        hitColor = Color.red;
        healColor = Color.green;
        //hasDash = PlayerPrefs.GetInt( "hasDash" ) == 1;
        hasDash = true;
    }

    // Update is called once per frame
    void Update() {
        //TESTING SPACE
        //Vector2 diagLeft = new Vector2( -1, -1 );
        //Vector2 leftpos = new Vector2( playerBody.transform.position.x - 1, playerBody.transform.position.y );
        //RaycastHit2D ground = Physics2D.Raycast( leftpos, Vector2.down, 1.0f, groundLayer );
        //leftTesting = ground.collider == null;
        //TESTING SPACE

        //

        //Reset incase escape from map borders
        if ( playerBody.position.y < -20 ) {
            SceneManager.LoadScene( "Main" );
        }

        if ( playerHealth <= 0 ) {
            StartCoroutine( Kill() );
        }
        moveX = Input.GetAxis( "Horizontal" );
        animCall = "None";
        if ( DashCooldown == 1 ) {
            StartCoroutine( DashReset() );
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
        if ( iFrames > 0 ) {
            iFrames--;
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

        if ( ( Input.GetButtonDown( "Dash" ) || Input.GetAxis( "Dash (Controller)" ) == 1 ) && DashCooldown == 0 && attack1Length == 0 && attack2Length == 0 && hasDash ) {
            Dash();
        }

        if ( ( Input.GetButtonDown( "Attack2" ) || Input.GetAxis( "Attack2" ) == 1 ) && attack2Length == 0 && attack1Length == 0 ) {
            //Attack2();
            animCall = "HeroWarrior_Attack_part1";
            Invoke( "Attack2", .35f );
            attack2Length = 30;
        }

        if ( Input.GetButtonDown( "Attack1" ) && attack1Length == 0 && attack2Length == 0 ) {
            Attack1();
            attack1Length = 40;
        }

        //Priority should be handled by code order
        //seperate method turned out to not work well
        if ( animCall != "None" ) {
            animator.Play( animCall );
        }
    }

    void LateUpdate() {
        if ( moveX > 0.0f && facingRight == false ) {
            FlipPlayer();
        } else if ( moveX < 0.0f && facingRight == true ) {
            FlipPlayer();
        }
        playerBody.velocity = new Vector2( moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y );
    }

    bool isGrounded() {
        Vector2 pos = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        RaycastHit2D hit = Physics2D.Raycast( pos, direction, distance, groundLayer );
        //Debug.DrawRay( pos, direction, Color.green );
        return ( Grounded = hit.collider != null );

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
            Instantiate( swordwaveRightPrefab, pos, Quaternion.identity );
        } else {
            Vector2 pos = new Vector2( playerBody.transform.position.x - .3f, playerBody.transform.position.y + .8f );
            Instantiate( swordwaveLeftPrefab, pos, Quaternion.identity );
        }
    }

    void FlipPlayer() {
        if ( attack1Length == 0 && attack2Length == 0 ) {
            facingRight = !facingRight;
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    bool playerIsMoving() {
        if ( playerJumpNum > 0 ) {
            return ( true );
        }
        return ( moveX != 0 );
    }

    public void damage( int value ) {
        playerHealth -= value;
    }
    
    public void damage( int value, int force, Vector2 direction ) {
        playerHealth -= value;
        playerBody.AddForce( direction * force );
        StartCoroutine( Hit() );
    }

    //if player is hit
    private void OnTriggerEnter2D( Collider2D other ) {
        if ( iFrames <= 0 ) {
            if ( other.tag == "EnemyHurtbox" ) {
                damage(meleeAtkDamage, meleeKnockback, Vector3.Normalize(playerBody.transform.position - other.gameObject.transform.position));
                StartCoroutine( Hit() );
                iFrames = 30;
            }
            else if ( other.tag == "Spikes" ) {
                StartCoroutine( Kill() );
            }
        }
        if ( other.tag == "HeartPickup" ) {
            if ( playerHealth < 6 ) {
                playerHealth++;
                StartCoroutine( Heal() );
                Destroy( other.gameObject );
            }
        }
    }

    IEnumerator DashReset() {
        GetComponent<Renderer>().material.color = dashColor;
        yield return new WaitForSeconds( .1f );
        GetComponent<Renderer>().material.color = normalColor;
        yield return new WaitForSeconds( .1f );
    }

    IEnumerator Hit() {
        GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds( .1f );
        GetComponent<Renderer>().material.color = normalColor;
        yield return new WaitForSeconds( .1f );
    }

    IEnumerator Heal() {
            GetComponent<Renderer>().material.color = healColor;
            yield return new WaitForSeconds( .1f );
            GetComponent<Renderer>().material.color = normalColor;
            yield return new WaitForSeconds( .1f );
    }

    IEnumerator Kill() {
        GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds( .1f );
        gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds( .5f );
        deathGUI.gameObject.SetActive( true );
        //deathGUI.GetComponent<GameOverFadeIn>().dead = true;
        Destroy( gameObject );
    }
}
