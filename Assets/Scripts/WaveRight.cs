﻿using UnityEngine;

public class WaveRight : MonoBehaviour {

    private Rigidbody2D body;
    private int deathTimer;

    // Use this for initialization
    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        deathTimer = 80;
        body.AddForce( Vector2.right * 1600 );
    }

    // Update is called once per frame
    void Update() {
        deathTimer--;
        if ( deathTimer == 0 ) {
            DestroyObject( gameObject );
        }
    }
}
