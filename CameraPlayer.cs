using UnityEngine;
using System.Collections;

public class CameraPlayer : MonoBehaviour
{
    public GameObject target;
    public GameObject Noah;
    public GameObject Jack;
    public float xOffset = 0f;
    //public float yOffset = 0f;
    public float zOffset = 0f;
    // GameManager gm;
    CharacterController cc;
    public float scrollSpeed = 5;
    public TurnManager tm;
    // Use this for initialization
    void Start()
    {
        //gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ResetCamera();
    }

    private void Update()
    {
        var translation = Vector3.zero;
        translation = new Vector3(-Input.GetAxis("Horizontal"), -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, -Input.GetAxis("Vertical"));
        GetComponent<Camera>().transform.position += translation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (!gm.playerIsDead)
        // {
        //if the player is alive, run the function for the camera to follow the player
        UpdatePosition();
        // }
    }

    public void ResetCamera()
    {
        //reset camera to new character after player has died (assuming more lives are available)
        //target = null;
        //target = GameObject.FindGameObjectWithTag("Player");
        //Check to make sure we have a target; if not we find the player
        if (tm.getUIName() == "Noah")
        {
            target = Noah;
        }
        else
        {
            target = Jack;
        }
    }

    void UpdatePosition()
    {
        //Check to make sure we have a target; if not we find the player
        if (tm.getUIName() == "Noah")
        {
            target = Noah;
            this.transform.position = new Vector3(target.transform.position.x + xOffset, this.transform.position.y, target.transform.position.z + zOffset);
        }
        else
        {
            target = Jack;
            this.transform.position = new Vector3(target.transform.position.x + xOffset, this.transform.position.y, target.transform.position.z + zOffset);
        }

       // if (target != null)
       // {
            //this.transform.position = new Vector3(target.transform.position.x + xOffset, this.transform.position.y, target.transform.position.z + zOffset);
       // }
    }
}
