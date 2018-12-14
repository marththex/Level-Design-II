using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMove : MonoBehaviour
{

    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    Stack<Tile> path = new Stack<Tile>();
    public Tile currentTile;

    public bool moving = false;
    public int move = Dice.diceValue;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;
    public PlayerStats player1;
    public PlayerStats player2;
    public PlayerStats player3;
    public PlayerStats player4;
    public NPCStats NPC;

    public static bool attacking;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        TurnManager.AddUnit(this);
    }

    void Update()
    {
        if (TurnManager.timer <= 0) {
            RemoveSelectableTiles();
            moving = false;

            //to-do end turn after player selects end turn instead of just after it moves
            Dice.diceValue = 0;

            if (TurnManager.NPCTurn == true)
            {
                npcAttack();
            }
            else
                TurnManager.attackPhase = true;
        }

    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        //Debug.Log(hit);

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(jumpHeight, target);
        }
    }

    //Breath-First Search uses a queue
    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        int count = 0;
        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? leave as null
        move = Dice.diceValue;
        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            count++;
            t.selectable = true;
            

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
        //Debug.Log(count);

    }

    public void MoveToTile(Tile tile)
    {

        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {

            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {

        if (path.Count > 0)
        {

            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //Calculate the unit's position on top of the target tile
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {

                bool jump = transform.position.y != target.y;

               /* if (jump)
                {
                    Jump(target);

                }

                else
                {*/

                    CalculateHeading(target);
                    SetHorizontalVelocity();
               // }
                //Locomotion
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;



            }

            else
            {

                //Tile center reached
                transform.position = target;
                path.Pop();
            }
        }

        else
        {

            RemoveSelectableTiles();
            moving = false;

            //to-do end turn after player selects end turn instead of just after it moves
            Dice.diceValue = 0;

            //Attack Phase
            //Debug.Log("Attack Phase");
            //attacking = true;
            if (TurnManager.NPCTurn == true)
            {
                npcAttack();
            }
            /*else
            {
                Debug.Log("Player Attack");
                playerAttack();
            }*/
            else
                //TurnManager.EndTurn();
                TurnManager.attackPhase = true;

        }
    }

    protected void RemoveSelectableTiles()
    {

        if (currentTile != null)
        {

            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {

            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {

        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {

        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        /*
        if (fallingDown)
        {

            FallingDownward(target);
        }

        else if (jumpingUp)
        {
            JumpUpward(target);

        }

        else if (movingEdge)
        {

            MoveToEdge();
        }

        else
        {

            PrepareJump(target);
        }*/
    }

    void PrepareJump(Vector3 target)
    {

        float targetY = target.y;

        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {

            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;

        }

        else
        {

            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallingDownward(Vector3 target)
    {

        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {

            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {

        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {

            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {

        if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f)
        {

            SetHorizontalVelocity();
        }

        else
        {

            movingEdge = false;
            fallingDown = true;

            velocity /= 4.0f;
            //makes the hop
            velocity.y = 1.5f;
        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {

        Tile lowest = list[0];

        foreach (Tile t in list)
        {

            if (t.f < lowest.f)
            {

                lowest = t;
            }
        }
        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {

        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {

            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {

            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= move; ++i)
        {

            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {

        move = Dice.diceValue;
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        //A* uses an open list and close list (a total of 2 lists)
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(currentTile);
        //currentTile.parent = ??

        //We could use square distance because these are just relative number and don't have to be exact, but we're using distance anyways
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {

            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {

                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {

                if (closedList.Contains(tile))
                {

                    //Do nothing, already processed
                }

                else if (openList.Contains(tile))
                {

                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    //We found a new parent, we found a faster way
                    if (tempG < tile.g)
                    {

                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }

                else
                {

                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    //We could you manhattan distance, but we're gonna use euclardian distance because it's easier
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //todo = what do you do if there is no path to the target tile
        Debug.Log("Path not found");

    }

    public void BeginTurn()
    {

        turn = true;
    }

    public void EndTurn()
    {
        RemoveSelectableTiles();
        turn = false;
    }

    public void playerEndTurn() {

        if (currentTile != null)
        {

            currentTile.current = false;
            currentTile = null;
        }

        GetCurrentTile();

        foreach (Tile tile in currentTile.adjacencyList)
        {

            tile.Reset();
        }
        RemoveSelectableTiles();
        TurnManager.EndTurn();
    }

    //Combat
    
    public void attackTargets()
    {
        
        GetCurrentTile();
        currentTile.FindEnemies(jumpHeight, currentTile);
        bool foundTarget = false;
        foreach (Tile tile in currentTile.adjacencyList) {

            RaycastHit hit;
            if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
            {
                //Cannot attack a key
                if (hit.collider.gameObject.name != "Key")
                {
                    tile.attackable = true;
                    foundTarget = true;
                }
            }

        }

        if (foundTarget == false) {
            //Debug.Log("No Target Found");
            TurnManager.attackPhase = false;
            TurnManager.EndTurn();
        }
        
    }

    public void attackPlayer(Tile t)
    {
        RaycastHit hit;
        if (Physics.Raycast(t.transform.position, Vector3.up, out hit, 1))
        {

             if (hit.collider.gameObject.name == "Jack")
             {
                if (Dice.attackValue > 3)
                {
                    Debug.Log("Attacked Jack for 2 health");
                    player1.health -= 2;
                }

                else
                    Debug.Log("Attack Missed");
            }

             if (hit.collider.gameObject.name == "Noah")
             {
                if (Dice.attackValue > 3)
                {
                    Debug.Log("Attacked Noah for 2 health");
                    player2.health -= 2;
                }
                else
                    Debug.Log("Attack Missed");

            }

             if (hit.collider.gameObject.name == "NPC")
             {
                if (Dice.attackValue > 3)
                {
                    Debug.Log("Attacked NPC for 5 health");
                    NPC.health -= 10;
                }
                else
                    Debug.Log("Attack Missed");
            }
        }
    }

    public void npcAttack() {
        GetCurrentTile();
        currentTile.FindEnemies(jumpHeight, currentTile);
        List<Tile> players = new List<Tile>();
        foreach (Tile tile in currentTile.adjacencyList)
        {

            RaycastHit hit;
            
            if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
            {
                players.Add(tile);
            }

        }

        int rand = Random.Range(0, players.Count);

        for (int i = 0; i < players.Count; ++i) {
            if (i == rand) {
                if(players[i] != null)
                attackPlayer(players[i]);
                break;
            }
        }

        //End turn
        //attacking = false;
        TurnManager.EndTurn();
    }
}
