using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToStart : MonoBehaviour {
    public void ReturntoStart()
    {
        SceneManager.LoadScene("Start_Up", LoadSceneMode.Single);
        Debug.Log("This is working");
    }
}
