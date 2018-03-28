using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {
    
    public void newGame() {
        PlayerPrefs.SetInt( "Checkpoint", 0 );
        PlayerPrefs.SetInt( "hasDash", 0 );
        SceneManager.LoadScene( 1 );
    }
}
