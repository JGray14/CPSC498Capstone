using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if ( PlayerPrefs.GetInt( "hasDash" ) == 1 ) {
            Destroy( gameObject );
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround( transform.position, transform.up, Time.deltaTime * 90f );
    }

    public void OnTriggerEnter2D( Collider2D other ) {
        if ( other.gameObject.tag == "Player" ) {
            PlayerPrefs.SetInt("hasDash", 1);
            GameObject.FindGameObjectWithTag( "Player" ).GetComponent<PlayerControl>().hasDash = true;
            Destroy( gameObject );
        }
    }
}
