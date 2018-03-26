using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    public float xMin = float.MinValue;
    public float xMax = float.MaxValue;
    public float yMin = float.MinValue;
    public float yMax = float.MaxValue;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag( "Player" );
	}
	
	// Update is called once per frame
	void Update () {
        if ( player != null ) {
            float x = Mathf.Clamp( player.transform.position.x, xMin, xMax );
            float y = Mathf.Clamp( player.transform.position.y + 3.1f, yMin, yMax );
            gameObject.transform.position = new Vector3( x, y, gameObject.transform.position.z );
        }
    }
}