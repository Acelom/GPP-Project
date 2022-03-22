using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerOnSpline : MonoBehaviour
{
    private GameObject player;
    private PlayerControls playerScript;

    public PathCreation.PathCreator pathCreator;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        GameObject other = collisionInfo.transform.gameObject; 
        if (other.tag = "Player")
        {
            playerScript.pathCreator = pathCreator; 
            playerScript.splineFollow = true;
        }
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        GameObject other = collisionInfo.transform.gameObject;
        if (other.tag = "Player")
        {
            playerScript.pathCreator = null; 
            playerScript.splineFollow = false; 
        }
    }
}
