using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour {

    public  GameObject enemyPrefab;
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
            if ( distanceFromPlayer < 20 && enemyPrefab != null ) {
                Instantiate( enemyPrefab, pos, Quaternion.identity );
                Destroy( gameObject );
            }
        }
    }
}
