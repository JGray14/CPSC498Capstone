using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;

    public int health;
    private float speed = 100f;
    private float dis = 0.1f;
    public bool isEvade = false;
    private bool die;
    public bool acting;
    public bool moving;
    public bool buffed;
    public bool buffing;
    public bool facingRight;
    public int action;
    public int attackCooldown;
    public int evadeCooldown;
    public int buffCooldown;
    public int iFrames;

    private Color normalColor;
    private Color buffColor;
    private Color hitColor;
    
    void Start() {
        animator = this.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag( "Player" );

        health = 30;
        acting = false;
        moving = true;
        buffed = false;
        facingRight = true;
        action = 0;
        attackCooldown = 0;
        evadeCooldown = 0;
        buffCooldown = 0;

        normalColor = GetComponent<Renderer>().material.color;
        buffColor = Color.gray;
        hitColor = Color.red;
    }
    
    void Update() {

    }

    [Task]
    void walkTowardPlayer() {
        int timer = 50;
        while ( player.transform.position.x - body.transform.position.x > 8 && timer > 0 ) {
            if ( player.transform.position.x < body.transform.position.x ) {
                body.AddForce( Vector2.left * speed );
            } else {
                body.AddForce( Vector2.right * speed );
            }
            timer--;
        }
    }

    [Task]
    void walkAwayPlayer() {
        int timer = 50;
        while ( player.transform.position.x - body.transform.position.x < 10 && timer > 0) {
            if ( player.transform.position.x > body.transform.position.x ) {
                body.AddForce( Vector2.left * speed );
            } else {
                body.AddForce( Vector2.right * speed );
            }
            timer--;
        }
    }

    [Task]
    void moveToAttack() {
        int timer = 150;
        while ( player.transform.position.x - body.transform.position.x > 5 && timer > 0 ) {
            if ( player.transform.position.x < body.transform.position.x ) {
                body.AddForce( Vector2.left * speed );
            } else {
                body.AddForce( Vector2.right * speed );
            }
            timer--;
        }
        StartCoroutine( Attack() );
        timer = 50;
        while ( timer > 0 ) {
            timer--;
        }
    }

    public IEnumerator Hit() {
        animator.SetTrigger( "hit_2" );
        GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds( 0.2f );
        GetComponent<Renderer>().material.color = normalColor;
    }

    public IEnumerator Attack() {
        animator.SetTrigger( "skill_1" );
        yield return new WaitForSeconds( 1.0f );
    }

    public IEnumerator Buff() {
        buffing = true;
        animator.SetTrigger( "skill_2" );
        buffed = true;
        GetComponent<Renderer>().material.color = buffColor;
        yield return new WaitForSeconds( 1.0f );
        buffing = false;
        yield return new WaitForSeconds( 10.0f );
        buffed = false;
        GetComponent<Renderer>().material.color = normalColor;
    }
}
