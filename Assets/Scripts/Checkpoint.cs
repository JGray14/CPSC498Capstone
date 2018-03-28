using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public GameObject player;
    public int CheckpointNumber;
    public int buffer;
    public Sprite activated;
    public Sprite deActivated;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag( "Player" );
        if ( PlayerPrefs.GetInt( "Checkpoint" ) != CheckpointNumber ) {
            gameObject.GetComponent<SpriteRenderer>().sprite = deActivated;
        } else {
            player.transform.position = gameObject.transform.position;
        }
        buffer = 50;
    }
	
	// Update is called once per frame
	void Update () {
        if ( buffer == 0 ) {
            if ( PlayerPrefs.GetInt( "Checkpoint" ) != CheckpointNumber ) {
                gameObject.GetComponent<SpriteRenderer>().sprite = deActivated;
            } else {
                gameObject.GetComponent<SpriteRenderer>().sprite = activated;
            }
            buffer = 50;
        } else {
            buffer--;
        }
	}

    private void OnTriggerEnter2D( Collider2D other ) {
        if ( other.gameObject.tag == "Player" && PlayerPrefs.GetInt("Checkpoint") != CheckpointNumber ) {
            PlayerPrefs.SetInt( "Checkpoint", CheckpointNumber );
            gameObject.GetComponent<SpriteRenderer>().sprite = activated;
        }
    }
}
