using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Camera cam;
    private GameObject player;

    public Material mat;
    public float speed; 
    public Transform destination;


    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player"); 
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.transform.position = destination.position;
            player.transform.rotation = destination.rotation;
            cam.transform.position = destination.position;
            cam.transform.rotation = destination.rotation;
        }
    }
}
