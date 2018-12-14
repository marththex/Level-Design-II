using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int health = 0;
    public List<string> items = new List<string>();
    public List<string> grudge = new List<string>();
    public LogHandler myLog;


	// Use this for initialization
	void Start () {
        health = 100;
        grudge.Add("backstab");
	}
	

    public void AddKey() {
        items.Add("Key");
    }

    public void RemoveGrudge(string s) {
        grudge.Remove(s);
    }


}
