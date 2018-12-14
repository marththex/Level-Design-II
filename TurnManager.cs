using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();
    public static bool NPCTurn = false;
    public static bool attackPhase = false;
    private static string uiName = "";
    public static float timer = 30;
    public static string displayText = "";
    public Text tutorialText;

    // Update is called once per frame
    void Update()
    {
        tutorialText.text = displayText;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndTurn();
            timer = 30;
        }

        if (turnTeam.Count == 0)
        {

            InitTeamTurnQueue();
        }

    }

    public static void resetTimer() {
        timer = 30;
 
    }

    static void InitTeamTurnQueue()
    {

        List<TacticsMove> teamList = units[turnKey.Peek()];

        foreach (TacticsMove unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    public static void StartTurn()
    {
        //endTurn = false;
        resetTimer();
        TurnManager.displayText = "Press 'Space' to Roll the Dice";
        Dice.newTurn = true;
        if (turnTeam.Count > 0)
        {
            //Debug.Log("Current Tag is: " + turnTeam.Peek().transform.tag);
           // Debug.Log("Current Name is: " + turnTeam.Peek().transform.name);
            uiName = turnTeam.Peek().transform.name;
            if (turnTeam.Peek().transform.tag == "NPC")
            {
                //Debug.Log("NPCTurn is true");
                NPCTurn = true;
            }
            else
            {
                //Debug.Log("NPCTurn is false");
                NPCTurn = false;
            }
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        
        Debug.Log("Ending Turn");
        TacticsMove unit = turnTeam.Dequeue();
        unit.EndTurn();

        //Next Player
        if (turnTeam.Count > 0)
        {
            StartTurn();
        }


        //Next Team
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(TacticsMove unit)
    {

        List<TacticsMove> list;

        if (!units.ContainsKey(unit.tag))
        {

            list = new List<TacticsMove>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {

                turnKey.Enqueue(unit.tag);
            }
        }

        else
        {

            list = units[unit.tag];
        }

        list.Add(unit);
    }

    //todo: public static void RemoveUnit(TacticsMove unit)

    public string getUIName() {
        return uiName;
    }

}