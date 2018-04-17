using Panda;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossControl : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private Rigidbody2D playerBody;

    public int health;
    public float speed;
    private bool die;
    private string prevAction;
    public bool moving;
    public bool attacking;
    public bool evading;
    public bool knockback;
    public bool hit;
    public bool facingRight;
    public bool idling;
    public bool healthThreshold;
    public bool thresholdAnim;
    public int walkTimer;
    public int attackCooldown;
    public int evadeCooldown;
    public int knockbackCooldown;
    public int iFrames;
    public float playerDistance;
    public float playerHeight;

    private SpriteRenderer[] children;

    private Color normalColor;
    private Color hitColor;
    

    void Start() {
        animator = this.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag( "Player" );
        playerBody = player.GetComponent<Rigidbody2D>();
        children = GetComponentsInChildren<SpriteRenderer>();
        //eyes.SetActive( false );

        health = 30;
        speed = 2000f;
        moving = true;
        attacking = false;
        evading = false;
        knockback = false;
        hit = false;
        idling = false;
        facingRight = false;
        healthThreshold = false;
        thresholdAnim = false;
        attackCooldown = 0;
        evadeCooldown = 0;
        knockbackCooldown = 0;
        walkTimer = 0;

        normalColor = children[0].material.color;
        hitColor = Color.red;
    }

    void Update() {
        body.velocity = new Vector2( 0, 0 );
        if ( iFrames > 0 ) {
            iFrames--;
        }
        if ( evadeCooldown > 0 ) {
            evadeCooldown--;
        }
        if ( knockbackCooldown > 0 ) {
            knockbackCooldown--;
        }
        if ( walkTimer > 0 ) {
            walkTimer--;
        }

        //if ( moving ) {
        //    animator.SetTrigger( "walk" );
        //} else {
        //    animator.ResetTrigger( "walk" );
        //}

        if ( evading ) {
            if ( player.transform.position.x < body.transform.position.x ) {
                body.AddForce( Vector2.right * speed * 4 );
            } else {
                body.AddForce( Vector2.left * speed * 4 );
            }
        }

        if (health < 1)
        {
            animator.Play("death");
            StartCoroutine(death());
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

    [Task]
    bool towardCheck() {
        if ( walkTimer == 0 ) {
            walkTimer = 150;
        } else if ( walkTimer == 1 ) {
            animator.ResetTrigger( "walk" );
            return ( false );
        }
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        return ( playerDistance > 6 );
    }

    [Task]
    void walkTowardPlayer() {
        prevAction = "Walk";
        animator.SetTrigger( "walk" );
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
            body.AddForce( Vector2.left * speed );
        } else {
            faceRight();
            body.AddForce( Vector2.right * speed );
        }
    }

    [Task]
    bool playerClose() {
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        return ( playerDistance < 6.5f );
    }

    [Task]
    bool notAttacking() {
        return ( !attacking );
    }

    [Task]
    void attack() {
        prevAction = "Attack";
        attacking = true;
        StartCoroutine( Attack() );
    }

    [Task]
    bool notEvading() {
        return ( !evading );
    }

    [Task]
    void evade() {
        prevAction = "Evade";
        if ( evadeCooldown <= 0 ) {
            StartCoroutine( Evade() );
            evadeCooldown = 300;
            evading = true;
        } else {
            Task.current.Fail();
        }
    }

    [Task]
    bool notKnockback() {
        return( !knockback );
    }

    [Task]
    void knockbackPlayer() {
        prevAction = "Knockback";
        if ( knockbackCooldown <= 0 ) {
            StartCoroutine( knockbackPlayerCoroutine() );
            knockbackCooldown = 300;
            knockback = true;
        } else {
            Task.current.Fail();
        }
    }

    [Task]
    bool notIdle() {
        return ( !idling );
    }

    [Task]
    void idle() {
        StartCoroutine( idleCoroutine() );
    }

    [Task]
    bool notThreshold() {
        return( !thresholdAnim );
    }

    [Task]
    void threshold() {
        if ( !healthThreshold && health < 16 ) {
            healthThreshold = true;
            StartCoroutine( ThresholdCoroutine() );
            Task.current.Succeed();
        } else {
            Task.current.Fail();
        }
    }

    [Task]
    bool notHit() {
        return( !hit );
    }

<<<<<<< HEAD
    public IEnumerator ThresholdCoroutine() {
        thresholdAnim = true;
        speed = 3000;
        animator.SetTrigger( "idle_2" );
        yield return new WaitForSeconds( 1.5f );
        thresholdAnim = false;
    }
=======
    public IEnumerator death()
    {
        yield return new WaitForSeconds(5f);
        DestroyObject(gameObject);
        //SceneManager.LoadScene(" ");
    }

>>>>>>> b9922134702ec1040c6d3abbbff7973f22dd7093
    public IEnumerator idleCoroutine() {
        idling = true;
        if ( prevAction != "Idle" ) {
            prevAction = "Idle";
            animator.SetTrigger( "idle_1" );
        }
        yield return new WaitForSeconds( 0.5f );
        idling = false;
    }

    public IEnumerator Hit() {
        //hit = true;
        changeColor( hitColor );
        //animator.ResetTrigger( "idle_1" );
        //animator.ResetTrigger( "walk" );
        //animator.SetTrigger( "hit_2" );
        yield return new WaitForSeconds( 0.2f );
        changeColor( normalColor );
        //hit = false;
    }

    public IEnumerator Attack() {
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
        } else {
            faceRight();
        }
        animator.ResetTrigger( "idle_1" );
        animator.ResetTrigger( "walk" );
        animator.SetTrigger( "skill_1" );
        yield return new WaitForSeconds( 1.75f );
        attacking = false;
    }

    public IEnumerator Evade() {
        if ( player.transform.position.x < body.transform.position.x ) {
            faceRight();
        } else {
            faceLeft();
        }
        animator.ResetTrigger( "idle_1" );
        animator.ResetTrigger( "skill_1" );
        animator.ResetTrigger( "skill_2" );
        animator.ResetTrigger( "walk" );
        animator.ResetTrigger( "hit_2" );
        animator.SetTrigger( "evade_1" );
        yield return new WaitForSeconds( 0.45f );
        evading = false;
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
        } else {
            faceRight();
        }
    }

    public IEnumerator knockbackPlayerCoroutine() {
        knockback = true;
        animator.ResetTrigger( "idle_1" );
        animator.ResetTrigger( "evade_1" );
        animator.ResetTrigger( "skill_1" );
        animator.ResetTrigger( "walk" );
        animator.ResetTrigger( "hit_2" );
        animator.SetTrigger( "skill_2" );
        yield return new WaitForSeconds( 0.5f );
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        playerHeight = player.transform.position.y - body.transform.position.y;
        if ( playerDistance < 13 && playerHeight < 2 ) {
            for ( int i = 0; i < 20; i++ ) {
                yield return new WaitForSeconds( 0.01f );
                playerBody.AddForce( Vector3.Normalize( playerBody.transform.position - body.transform.position ) * 1000 );
            }
        } else {
            for ( int i = 0; i < 20; i++ ) {
                yield return new WaitForSeconds( 0.01f );
                playerBody.AddForce( Vector3.Normalize( playerBody.transform.position - body.transform.position ) * 250 );
            }
        }
        yield return new WaitForSeconds( 0.5f );
        knockback = false;
    }

    private void changeColor( Color color ) {
        for ( int i = 0; i < children.Length; i++ ) {
            children[i].color = color;
        }
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        if ( iFrames <= 0 ) {
            if ( other.tag == "PlayerHurtbox" ) {
                health -= 2;
                StartCoroutine( Hit() );
                iFrames = 50;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                health -= 1;
                StartCoroutine( Hit() );
                iFrames = 50;
            }
        }
    }
}
