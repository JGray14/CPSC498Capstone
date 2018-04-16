using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour {

    private GameObject[] buttons;

	// Use this for initialization
	void Start () {
        buttons = GameObject.FindGameObjectsWithTag("ESCMenu");
        foreach ( GameObject B in buttons ) {
            B.SetActive( false );
        }

    }
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetButtonDown( "Menu" ) ) {
            foreach ( GameObject B in buttons ) {
                if ( B.activeSelf ) {
                    B.SetActive( false );
                } else {
                    B.SetActive( true );
                }
            }

        }
	}
}
