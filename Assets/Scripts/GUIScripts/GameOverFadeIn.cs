using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFadeIn : MonoBehaviour {
    public bool dead = false;

	// Use this for initialization
	void Start() {
        StartCoroutine( FadeIn() );
	}

    IEnumerator FadeIn() {
        CanvasGroup deathCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        while ( deathCanvasGroup.alpha < 1 ) {
            deathCanvasGroup.alpha += .01f;
            yield return new WaitForSeconds( .003f );
        }
    }
}
