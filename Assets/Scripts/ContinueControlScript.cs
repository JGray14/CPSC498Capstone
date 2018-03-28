using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueControlScript : MonoBehaviour {

    public GameObject ContinueButton;

	// Use this for initialization
	void Start () {
        if ( PlayerPrefs.HasKey( "Checkpoint" ) ) {
            ContinueButton.SetActive( true );
        } else {
            ContinueButton.SetActive( false );
            PlayerPrefs.SetInt( "Checkpoint", 0 );
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
