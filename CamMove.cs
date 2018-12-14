using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamMove : MonoBehaviour
{
    public float scrollSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        var translation = Vector3.zero;
        translation = new Vector3(-Input.GetAxis("Horizontal"), -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, -Input.GetAxis("Vertical"));
        GetComponent<Camera>().transform.position += translation;
    }
}