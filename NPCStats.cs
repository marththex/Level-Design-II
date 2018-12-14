using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{

    public int health = 0;
    List<string> items = new List<string>();
    List<string> grudge = new List<string>();
    public LogHandler myLog;

    // Use this for initialization
    void Start()
    {
        health = 10;
    }

}
