using Panda;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;
    private Rigidbody2D playerBody;

    public int health;
    public float speed = 100f;
    private bool die;
    public bool moving;
    public bool attacking;
    public bool evading;
    public bool buffed;
    public bool buffing;
    public bool hit;
    public bool facingRight;
    public bool timerRunning;
    public bool idleTimerBool;
    public int attackCooldown;
    public int evadeCooldown;
    public int buffCooldown;
    public int iFrames;
    public float playerDistance;

    private SpriteRenderer[] children;
    private Color normalColor;
    private Color buffColor;
    private Color hitColor;

    private GameObject eyes;

    void Start() {
        animator = this.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag( "Player" );
        playerBody = player.GetComponent<Rigidbody2D>();
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        eyes = GameObject.FindGameObjectWithTag( "BossEyes" );
        //eyes.SetActive( false );

        health = 30;
        moving = true;
        attacking = false;
        evading = false;
        buffed = false;
        hit = false;
        timerRunning = false;
        idleTimerBool = false;
        facingRight = true;
        attackCooldown = 0;
        evadeCooldown = 0;
        buffCooldown = 0;

        normalColor = children[0].material.color;
        buffColor = Color.gray;
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
        if ( buffCooldown > 0 ) {
            buffCooldown--;
        }

        if ( moving ) {
            animator.SetTrigger( "walk" );
        } else {
            animator.ResetTrigger( "walk" );
        }

        if ( evading ) {
            if ( player.transform.position.x < body.transform.position.x ) {
                body.AddForce( Vector2.right * speed * 4 );
            } else {
                body.AddForce( Vector2.left * speed * 4 );
            }
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

    //Task.current.Succeed();

    [Task]
    bool healthThreshold() {
        return( health > 15 );
    }
    [Task]
    bool towardCheck() {
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        return ( playerDistance > 5 );
    }

    [Task]
    void walkTowardPlayer() {
        animator.ResetTrigger( "idle_1" );
        animator.ResetTrigger( "evade_1" );
        animator.ResetTrigger( "skill_1" );
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
    bool awayCheck() {
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        return ( playerDistance < 15 );
    }

    [Task]
    void walkAwayPlayer() {
        moving = true;
        attacking = false;
        if ( player.transform.position.x > body.transform.position.x ) {
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
        attacking = true;
        animator.ResetTrigger( "idle_1" );
        animator.ResetTrigger( "walk" );
        StartCoroutine( Attack() );
    }

    [Task]
    bool notEvading() {
        return ( !evading );
    }

    [Task]
    void evade() {
        if ( evadeCooldown <= 0 ) {
            evadeCooldown = 100;
            evading = true;
            animator.ResetTrigger( "idle_1" );
            animator.ResetTrigger( "walk" );
            StartCoroutine( Evade() );
        }
    }

    [Task]
    bool notBuffing() {
        return( !buffing );
    }

    [Task]
    void buffSelf() {
        if ( buffCooldown <= 0 ) {
            buffCooldown = 250;
            buffing = true;
            animator.ResetTrigger( "idle_1" );
            animator.ResetTrigger( "walk" );
            StartCoroutine( Buff() );
        }
    }

    [Task]
    bool idleTimer() {
        if ( !timerRunning ) {
            StartCoroutine( idleTimerCoroutine() );
            idleTimerBool = true;
        }
        return ( idleTimerBool );
    }

    [Task]
    void idle() {
        animator.SetTrigger( "idle_1" );
        animator.ResetTrigger( "walk" );
    }

    [Task]
    bool notHit() {
        return( !hit );
    }

    public IEnumerator idleTimerCoroutine() {
        timerRunning = true;
        yield return new WaitForSeconds( 3f );
        idleTimerBool = false;
        yield return new WaitForSeconds( 0.1f );
        timerRunning = false;
    }
    public IEnumerator Hit() {
        hit = true;
        animator.SetTrigger( "hit_2" );
        //changeColor( hitColor );
        yield return new WaitForSeconds( 0.4f );
        //changeColor( normalColor );
        hit = false;
    }

    public IEnumerator Attack() {
        animator.SetTrigger( "skill_1" );
        yield return new WaitForSeconds( 1.5f );
        attacking = false;
    }

    public IEnumerator Evade() {
        animator.SetTrigger( "evade_1" );
        if ( player.transform.position.x < body.transform.position.x ) {
            faceRight();
        } else {
            faceLeft();
        }
        yield return new WaitForSeconds( 0.5f );
        evading = false;
        if ( player.transform.position.x < body.transform.position.x ) {
            faceLeft();
        } else {
            faceRight();
        }
    }

    public IEnumerator Buff() {
        animator.SetTrigger( "skill_2" );
        buffed = true;
        eyes.SetActive( true );
        playerDistance = Math.Abs( player.transform.position.x - body.transform.position.x );
        if ( playerDistance < 9 ) {
            playerBody.AddForce( Vector3.Normalize( playerBody.transform.position - body.transform.position ) * 2000 );
        }
        yield return new WaitForSeconds( 1.0f );
        buffing = false;
        yield return new WaitForSeconds( 10.0f );
        buffed = false;
        eyes.SetActive( false );
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
                iFrames = 30;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                health -= 1;
                StartCoroutine( Hit() );
                iFrames = 30;
            }
        }
    }
}
