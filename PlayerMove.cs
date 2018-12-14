using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{
    public bool afterMove = false;
    public static bool doneAttackRoll;
    public static Tile attackingTile;
    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward);

        if (Dice.triggerPlayerEndTurn == true)
        {
            playerEndTurn();
            Dice.triggerPlayerEndTurn = false;
            Dice.attacked = false;
        }

        if (TurnManager.timer <= 0) {
            Move();
            turn = false;
        }

        if (!turn)
        {
            return;
        }

        else if (TurnManager.attackPhase)
        {
            TurnManager.displayText = "Select a Target";
            attackTargets();
            CheckAttack();
        }

        else if (!moving)
        {
            FindSelectableTiles();
            CheckMouse();

        }

        else
        {
            Move();
        }
    }

    void CheckMouse()
    {

        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("A"))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(ray);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {

                        MoveToTile(t);
                    }

                }
            }
        }
    }

    void CheckAttack()
    {

        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("A"))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.Log(ray);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    //attackPlayer(t);
                    if (t.attackable)
                    {
                        attacking = true;
                        attackingTile = t;
                    }
                        //TurnManager.attackPhase = false;
                       // playerEndTurn();
                        
                    

                }
            }
        }
    }

    /*  void CheckController() {
          Tile.input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

         if (Mathf.Abs(Tile.input.x) > Mathf.Abs(Tile.input.y))
         {
             Tile.input.y = 0;
         }
         else
         {
             Tile.input.x = 0;
         }


          if (Tile.input != Vector2.zero)
          {
              MoveToTile(t);
          }
      }*/
}
