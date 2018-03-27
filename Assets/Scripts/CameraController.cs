using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;

    public float playerX;
    public float playerY;
    public float xMin = float.MinValue;
    public float xMax = float.MaxValue;
    public float yMin = float.MinValue;
    public float yMax = float.MaxValue;
    private float yMove;

    public float moveY;
    public float Ydiff;

    public float smoothTime = .001F;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag( "Player" );
        Ydiff = 0;
	}

    void temp() {
        if ( player != null ) {
            moveY = Input.GetAxis( "Vertical" );
            yMove = 0;
            playerX = player.transform.position.x;
            playerY = player.transform.position.y + 3.1f;
            Vector3 targetPosition = new Vector3( playerX, playerY, -15.05f );
            if ( moveY < 0.0f ) { //Down
                targetPosition += new Vector3( 0, -4, 0 );
            } else if ( moveY > 0.0f ) { //Up
                targetPosition += new Vector3( 0, 4, 0 );
            }
            transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref velocity, smoothTime );
        }
    }

    void Original() {
        if ( player != null ) {
            float x = player.transform.position.x; //Mathf.Clamp( player.transform.position.x, xMin, xMax );
            float y = player.transform.position.y + 3.1f; //Mathf.Clamp( player.transform.position.y + 3.1f, yMin, yMax );
            gameObject.transform.position = new Vector3( x, y, gameObject.transform.position.z );
        }
    }
    
	void LateUpdate () {
        if ( player != null ) {
            moveY = Input.GetAxis( "Vertical" );
            float x = player.transform.position.x; //Mathf.Clamp( player.transform.position.x, xMin, xMax );
            float y = player.transform.position.y + 3.1f; //Mathf.Clamp( player.transform.position.y + 3.1f, yMin, yMax );
            if ( moveY < 0.0f && Ydiff > -3 ) { //Down
                Ydiff -= .3f;
            } else if ( moveY > 0.0f && Ydiff < 3 ) { //Up
                Ydiff += .3f;
            } else if ( Ydiff > 0 && moveY <= 0 ) {
                Ydiff -= .3f;
            } else if ( Ydiff < 0 && moveY >= 0 ) {
                Ydiff += .3f;
            }
            if ( y - (y + Ydiff) < .15f && y - ( y + Ydiff ) > -.15f ) {
                Ydiff = 0;
            }
            gameObject.transform.position = new Vector3( x, y + Ydiff, gameObject.transform.position.z );
        }
    }
}