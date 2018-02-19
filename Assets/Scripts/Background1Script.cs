using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background1Script : MonoBehaviour {

    private GameObject camera;
    private GameObject background;
    public  float speed;
    private float pos;

    // Use this for initialization
    void Start() {
        camera = GameObject.FindGameObjectWithTag( "MainCamera" );
        background = gameObject;
    }

    // Update is called once per frame
    void Update() {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2( camera.transform.position.x/500, 0 );
    }

    void LateUpdate() {
        background.transform.position = new Vector3( camera.transform.position.x, camera.transform.position.y, 50 );
    }
}
