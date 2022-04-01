using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerOnSpline : MonoBehaviour
{
    private GameObject player;
    private PlayerControls playerScript;
    private CameraControls camScript; 
    private PathCreation.PathCreator pathCreator;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>(); 
        pathCreator = GameObject.FindGameObjectWithTag("Path").GetComponent<PathCreation.PathCreator>(); 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.pathCreator = pathCreator;
            camScript.pathCreator = pathCreator; 
            camScript.currMode = CameraControls.camMode.splineFollow;
            playerScript.splineFollow = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerScript.pathCreator = null;
            camScript.pathCreator = null;
            playerScript.splineFollow = false;
            camScript.currMode = CameraControls.camMode.follow; 
        }
    }
}
