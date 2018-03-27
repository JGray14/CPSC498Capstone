using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMKnight : MonoBehaviour {

    public  GameObject MiniKnightPrefab;
    private GameObject player;
    private float distanceFromPlayer;
    private Vector2 pos;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag( "Player" );
    }

    void Update() {
        if ( player != null ) {
            pos = gameObject.transform.position;
            distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, pos );
            if ( distanceFromPlayer < 20 ) {
                Instantiate( MiniKnightPrefab, pos, Quaternion.identity );
                Destroy( gameObject );
            }
        }
    }
}
