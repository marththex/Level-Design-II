using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    Rigidbody rb;

    public static bool hasLanded;
    public static bool thrown;
    public static bool newTurn;

    Vector3 initPosition;

    public static int diceValue;
    public static int attackValue;

    public PlayerStats player1;
    public PlayerStats player2;
    public PlayerStats player3;
    public PlayerStats player4;

    public DiceSide[] diceSides;

    public static bool triggerPlayerEndTurn = false;
    public static bool attacked =  false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        rb.useGravity = false;
        newTurn = true;
        TurnManager.displayText = "Press 'Space' to Roll the Dice";
    }

    public void Update()
    {
        //Roll The Dice
        if ((Input.GetKeyDown(KeyCode.Space) && newTurn == true || Input.GetButtonUp("A") && newTurn == true) || TacticsMove.attacking == true)
        {
            RollDice();
        }


        if (rb.IsSleeping() && !hasLanded && thrown)
        {
            
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;

            //Attacking Roll
            if (TurnManager.attackPhase && TacticsMove.attacking == true)
            {
                AttackValueCheck();
                TacticsMove.attacking = false;
            }

            //Movement Roll
            else
            {
                SideValueCheck();
                newTurn = false;
                TurnManager.displayText = "Select a Tile to Move";
            }
        }

        //Reset Dice if timer is 0
        if (TurnManager.timer < 0) {
            Debug.Log("Timer ran out");
            diceValue = 0;
            Reset();
        }

        //Errors
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            if (TurnManager.NPCTurn == false)
            {
                Reset();
            }
            else
            {
                RollAgain();
            }
        }



    }

    void RollDice()
    {

        if (!thrown && !hasLanded)
        {

            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
        }

        else if (thrown && hasLanded)
        {

            Reset();
        }
    }

    void Reset()
    {
        transform.position = initPosition;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    void RollAgain()
    {

        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));

    }

    void SideValueCheck()
    {

        //diceValue = 0;
        foreach (DiceSide side in diceSides)
        {

            if (side.OnGround())
            {

                diceValue = side.sideValue;
                Debug.Log(diceValue + " has been rolled");
                Reset();

            }
        }
    }

    void AttackValueCheck()
    {
        attackValue = 0;
        foreach (DiceSide side in diceSides)
        {

            if (side.OnGround())
            {

                attackValue = side.sideValue;
                //Debug.Log(attackValue + " Attack");
                triggerPlayerEndTurn = false;

         
                    RaycastHit hit;
                    if (Physics.Raycast(PlayerMove.attackingTile.transform.position, Vector3.up, out hit, 1))
                    {

                        if (hit.collider.gameObject.name == "Noah" && attacked == false)
                        { 
                             Debug.Log("Attacked Jack for " + attackValue + " health");
                             player1.health -= attackValue;
                             attacked = true;


                        }

                        if (hit.collider.gameObject.name == "Jack" && attacked == false)
                        {
                             Debug.Log("Attacked Noah for " + attackValue + " health");
                             player2.health -= attackValue;
                             attacked = true;
                        }

                       /* if (hit.collider.gameObject.name == "NPC")
                        {
                            if (Dice.attackValue > 3)
                            {
                                Debug.Log("Attacked NPC for 5 health");
                                NPC.health -= 10;
                            }
                            else
                                Debug.Log("Attack Missed");
                        }*/
                    }
                

                TurnManager.attackPhase = false;
                triggerPlayerEndTurn = true;
                //playerEndTurn();


            }
        }
    }
}
