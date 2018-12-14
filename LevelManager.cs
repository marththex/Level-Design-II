using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int keysTotal;
    public int keysCollected = 0;
    public Text keysTotalText;
    public Text keysCollectedText;

    // Use this for initialization
    void Start()
    {
        CountTotalKeysAvailable();
    }
     

    void CountTotalKeysAvailable()
    {
        keysTotal = GameObject.FindGameObjectsWithTag("Key").Length;
        //keysTotalText.text = keysTotal.ToString();
    }

    public void KeysCollected()
    {
        keysCollected = keysCollected + 1;
        keysCollectedText.text = keysCollected.ToString();
    }
}