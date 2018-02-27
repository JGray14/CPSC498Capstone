using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniknightController : MonoBehaviour {

    private Rigidbody2D body;
    private LayerMask groundLayer;
    private Animator animator;
    private GameObject player;

    // Use this for initialization
    void Start () {
        //body = gameObject.GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        //groundLayer = LayerMask.GetMask( "Ground" );
        //player = GameObject.FindGameObjectWithTag( "Player" );
    }
	
	// Update is called once per frame
	void Update () {
        //string animCall = "None";
        //nav.SetDestination( player.transform.position );
    }
}
