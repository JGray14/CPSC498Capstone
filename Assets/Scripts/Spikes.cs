using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spikes : MonoBehaviour {

    private GameObject obj;

    void OnTriggerEnter2D( Collider2D other ) {
        obj = other.gameObject;
        if ( obj.tag == "Player" ) {
            //PlayerIsKill( other );
            //SceneManager.LoadScene( "Testing" );
        } else {
           // Destroy( other.gameObject );
        }
        
    }

    IEnumerator PlayerIsKill( Collider2D other ) {
        Destroy( other.gameObject );
        yield return new WaitForSeconds( 3 );
        SceneManager.LoadScene( "Testing" );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
