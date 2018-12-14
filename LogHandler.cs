using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogHandler : MonoBehaviour
{
    public Text logText;
    public List<string> log = new List<string>();
    string logstring;
    public TurnManager tm;

    // Update is called once per frame
    void Update()
    {

        logText.text = "It's " + tm.getUIName() + "'s Turn";
            
        /*foreach (string i in log) {
            logstring += i + "\n";
        }
        logText.text = logstring;
        */


    }

   public void addToLog(string s) {
        log.Add(s);
    }


    void resetLog() {

    }
}
