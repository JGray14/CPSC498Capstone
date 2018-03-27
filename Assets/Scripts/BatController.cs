using Panda;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour {

    public Rigidbody2D body;
    private bool facingRight = false;
    private float distanceFromPlayer;
    private Animator animator;
    private GameObject player;

    private Color normalColor;

    public int health;
    public  int speed = 200;
    private string animCall;

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag( "Player" );
        normalColor = GetComponent<Renderer>().material.color;
        health = 3;
        animCall = "batGrayFly";
    }

    // Update is called once per frame
    void Update() {
        animCall = "None";

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

    }

    [Task]
    void roost() {

    }
}
