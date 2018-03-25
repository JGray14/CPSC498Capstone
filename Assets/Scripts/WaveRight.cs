using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveRight : MonoBehaviour {

    private Rigidbody2D body;
    private bool triggered;
    private int deathTimer;

    // Use this for initialization
    void Start() {
        body = gameObject.GetComponent<Rigidbody2D>();
        deathTimer = 80;
        triggered = false;
    }

    // Update is called once per frame
    void Update() {
        deathTimer--;
        //float x = transform.position.x;
        //float y = transform.position.y;
        if ( deathTimer == 0 ) {
            DestroyObject( gameObject );
        }
        if ( !triggered ) {
            body.AddForce( Vector2.right * 1600 );
            triggered = true;
        }
        //transform.position = new Vector2( x + .2f, y );
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        //SceneManager.LoadScene( 0 );
        //other.gameObject.GetComponent<Enemy>().damage( 1, 800, Vector2.right );
    }
}
