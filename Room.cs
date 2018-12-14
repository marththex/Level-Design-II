using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 gridPos;
    public int type;
    public bool doorTop, doorBot, doorLeft, doorRight;
    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
    }
}