using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueControlScript : MonoBehaviour {

    public GameObject ContinueButton;

	// Use this for initialization
	void Start () {
        if ( PlayerPrefs.HasKey( "Checkpoint" ) ) {
            ContinueButton.SetActive( true );
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
