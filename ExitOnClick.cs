using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ExitOnClick : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("This button works");
    }
}