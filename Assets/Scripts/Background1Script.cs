using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background1Script : MonoBehaviour {

    private GameObject mainCamera;
    private GameObject background;
    public  float speed;
    private float pos;

    // Use this for initialization
    void Start() {
        mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" );
        background = gameObject;
    }

    // Update is called once per frame
    void Update() {
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2( mainCamera.transform.position.x/500, 0 );
    }

    void LateUpdate() {
        background.transform.position = new Vector3( mainCamera.transform.position.x, mainCamera.transform.position.y, 50 );
    }
}
