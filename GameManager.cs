using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static bool allKeysCollected = false;
    public static int numKeysCollected = 0;
    public TurnManager tm;

    //Cameras
    public Camera cam1;
    //public Camera cam3;
    //public Camera cam4;
    public Camera diceCam;

    //Players
    public GameObject player1;
    public GameObject player2;
   // public GameObject player3;
    //public GameObject player4;
   // public GameObject NPC;


    // Update is called once per frame
    void Update()
    {
        if (Dice.thrown == true && Dice.hasLanded == false)
        {
            cam1.enabled = false;
            diceCam.enabled = true;
        }

        else
        {
            cam1.enabled = true;
            diceCam.enabled = false;
        }

    }

    public static void keysCollected() {

        if (numKeysCollected == 2)
            allKeysCollected = true;
    }

    public static void addKey()
    {

        numKeysCollected++;
    }

}
