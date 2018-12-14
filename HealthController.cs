using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

    public Slider healthBarSlider;
    public int currentHealth;
    public int maxHealth;
    public Text healthText, nameText;
    bool isDead;
    public TurnManager tm;
    public PlayerStats player1;
    public PlayerStats player2;
   // public PlayerStats player3;
   // public PlayerStats player4;
    public NPCStats NPC;
    public Text timerText;

    // Use this for initialization
    void Start () {

        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        getPlayerTurn();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;

        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        nameText.text = tm.getUIName();
        timerText.text = (TurnManager.timer).ToString();

        if (currentHealth <= 0){

            if (isDead)
            {
                return;
            }

            Dead();

        }

        //BackStab
        if (Input.GetKeyDown(KeyCode.F1)) {

            int rand = Random.Range(0, 2);

            //#Noah Turn
            if (tm.getUIName() == "Noah")
            {
                //If backstab is avaliable
                if (player1.grudge.Contains("backstab"))
                {

                    if (rand == 0)
                    {
                        player2.health -= 2;
                        Debug.Log("Jack took 10 Damage");

                    }

                    else
                    {
                        NPC.health -= 2;
                        Debug.Log("NPC took 10 Damage");
                    }

                    player1.grudge.Remove("backstab");
                }

                //Already been used
                else
                {
                    Debug.Log("Backstab has already been used");
                }
            }
                

            //Jack Turn
            else if (tm.getUIName() == "Jack")
            {
                //If backstab is avaliable
                if (player2.grudge.Contains("backstab"))
                {
                    if (rand == 0)
                    {
                        player1.health -= 2;
                        Debug.Log("Noah took 10 Damage");

                    }

                    else
                    {
                        NPC.health -= 2;
                        Debug.Log("NPC took 10 Damage");
                    }
                    player2.grudge.Remove("backstab");
                }

                //Already been used
                else
                {
                    Debug.Log("Backstab has already been used");
                }
            }

        }


		
	}

    void Dead() {
        isDead = true;
    }

    void getPlayerTurn() {
        if (tm.getUIName() == "Noah")
            currentHealth = player1.health;
        else
            currentHealth = player2.health;
     //   else if (tm.getUIName() == "Richard")
         //   currentHealth = player3.health;
        //else if (tm.getUIName() == "Marcus")
         //   currentHealth = player4.health;
        //else
            //currentHealth = NPC.health;
    }
}
