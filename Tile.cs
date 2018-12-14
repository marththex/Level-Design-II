using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool attackable = false;

    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (Breath First Search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //For A*

    //g+h
    public float f = 0;
    //Cost from parent to the current tile
    public float g = 0;
    //Cost from the processed tile to the destination (heuristic)
    public float h = 0;

    public static Vector2 input;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (attackable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.clear;
        }



    }

    public void Reset()
    {
        adjacencyList.Clear();
        walkable = true;
        current = false;
        target = false;
        selectable = false;
        attackable = false;



        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;

    }

    public void FindNeighbors(float jumpHeight, Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, jumpHeight, target);
        CheckTile(-Vector3.forward, jumpHeight, target);
        CheckTile(Vector3.right, jumpHeight, target);
        CheckTile(-Vector3.right, jumpHeight, target);
    }

    public void CheckTile(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {

            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {

                RaycastHit hit;

                if (((!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1)) || (tile == target)))
                {
                    adjacencyList.Add(tile);
                }

                if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "Key")
                    {
                        adjacencyList.Add(tile);
                    }

                }

            }
        }

    }

    public void FindEnemies(float jumpHeight, Tile target)
    {
        Reset();

        CheckEnemy(Vector3.forward, jumpHeight, target);
        CheckEnemy(-Vector3.forward, jumpHeight, target);
        CheckEnemy(Vector3.right, jumpHeight, target);
        CheckEnemy(-Vector3.right, jumpHeight, target);
    }

    public void CheckEnemy(Vector3 direction, float jumpHeight, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {

            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {

                RaycastHit hit;

                if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "NPC")
                    {
                        adjacencyList.Add(tile);
                    }

                }

            }
        }
    }
}
