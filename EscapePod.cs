using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapePod : MonoBehaviour {

    private LevelManager lm;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("You entered the box");
        if (Exit.isCollected == true)
        {
            SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);

        }
    }
}
