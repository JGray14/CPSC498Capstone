using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveLeft : MonoBehaviour {

    private bool triggered;
    private int deathTimer;

	// Use this for initialization
	void Start () {
        deathTimer = 80;
        triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
        deathTimer--;
        //float x = transform.position.x;
        //float y = transform.position.y;
        if ( deathTimer == 0 ) {
            DestroyObject( gameObject );
        }
        if ( !triggered ) {
            gameObject.GetComponent<Rigidbody2D>().AddForce( Vector2.left * 1600 );
            triggered = true;
        }
        //transform.position = new Vector2( x - .2f, y );
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        SceneManager.LoadScene( 0 );
        other.gameObject.GetComponent<Enemy>().damage( 1, 800, Vector2.left );
    }
}
