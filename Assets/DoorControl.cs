using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour {

    GameObject door;
    bool closed;

	// Use this for initialization
	void Start () {
        door = transform.parent.gameObject;
        closed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator CloseDoor() {
        for ( float i = 0; i < 2.9; i += 0.1f ) {
            yield return new WaitForSeconds( 0.025f );
            door.transform.position = new Vector2(door.transform.position.x, door.transform.position.y - 0.1f );
        }
    }
    private void OnTriggerEnter2D( Collider2D other ) {
        if ( other.tag == "Player" && !closed ) {
            closed = true;
            StartCoroutine( CloseDoor() );
        }
    }
}
