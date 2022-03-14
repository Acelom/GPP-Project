using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    private GameObject player;

    public Transform camTarget;
    public float turnSpeed;
    public float followSpeed; 

    void Update()
    {
        float camXAxis = Input.GetAxis("RHorizontal");
        float camYAxis = Input.GetAxis("RVertical");

        if (Mathf.Abs(camXAxis) > 0.1 || Mathf.Abs(camYAxis) > 0.1)
        {
            transform.Rotate(new Vector3(camYAxis * Time.deltaTime * turnSpeed, camXAxis * Time.deltaTime * turnSpeed, 0));
        }

        transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles.x,  transform.rotation.eulerAngles.y, 0);
         transform.position = Vector3.Lerp( transform.position, camTarget.transform.position + (- transform.forward * 5), Mathf.SmoothStep(0, 1, followSpeed * Time.deltaTime));


    }
}
