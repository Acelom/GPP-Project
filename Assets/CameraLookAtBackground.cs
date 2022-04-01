using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtBackground : MonoBehaviour
{
    public Transform target;
    public Transform camPos;
    private Camera cam;
    private CameraControls camScript;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>(); 
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            camScript.camTarget = target;
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos.position, Time.deltaTime * 5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            camScript.camTarget = camScript.playerTarget; 
        }
    }
}
