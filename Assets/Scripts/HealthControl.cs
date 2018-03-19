using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour {

    public Sprite health0, health1, health2, health3, health4, health5, health6;
    private GameObject player;
    private int playerHealth;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag( "Player" );
    }

    // Update is called once per frame
    void Update() {
        playerHealth = player.GetComponent<PlayerControl>().playerHealth;
        if ( playerHealth == 0 ) {
            GetComponent<Image>().sprite = health0;
        } else if ( playerHealth == 1 ) {
            GetComponent<Image>().sprite = health1;
        } else if ( playerHealth == 2 ) {
            GetComponent<Image>().sprite = health2;
        } else if ( playerHealth == 3 ) {
            GetComponent<Image>().sprite = health3;
        } else if ( playerHealth == 4 ) {
            GetComponent<Image>().sprite = health4;
        } else if ( playerHealth == 5 ) {
            GetComponent<Image>().sprite = health5;
        } else if ( playerHealth == 6 ) {
            GetComponent<Image>().sprite = health6;
        }
    }
}