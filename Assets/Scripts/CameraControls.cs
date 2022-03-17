using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    private GameObject player;
    private float currentDistance;
    private RaycastHit[] hits;
    private List<GameObject> hitList;
    private List<GameObject> prevList;
    private Transform camTarget;

    public Transform playerTarget;
    public Transform lockOnTarget;
    public bool lockedOn;
    public float turnSpeed;
    public float followSpeed;
    public float normalDistance;
    public float minDistance;
    public float moveSpeed;
    public Transform cutscenePos;
    public bool cutscene;


    private void Awake()
    {
        camTarget = playerTarget;
        currentDistance = 5;
        hitList = new List<GameObject>();
        prevList = new List<GameObject>();
    }


    private void FixedUpdate()
    {
        if (lockedOn)
        {
            camTarget = lockOnTarget;
            currentDistance = 5 + Vector3.Distance(playerTarget.position, lockOnTarget.position) * 1.5f;
        }
        else
        {
            camTarget = playerTarget;
            currentDistance = 5;
        }

        if (cutscene)
        {
            transform.parent = cutscenePos;
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.parent = null;
            List<GameObject> hitList = new List<GameObject>();

            float camXAxis = Input.GetAxis("RHorizontal");
            float camYAxis = Input.GetAxis("RVertical");


            if (lockedOn)
            {

            }
            else
            {
                if (Mathf.Abs(camXAxis) > 0.1 || Mathf.Abs(camYAxis) > 0.1)
                {
                    transform.Rotate(new Vector3(camYAxis * Time.deltaTime * turnSpeed, camXAxis * Time.deltaTime * turnSpeed, 0));
                }
            }



            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            transform.position = Vector3.Lerp(transform.position, camTarget.transform.position + (-transform.forward * currentDistance), Mathf.SmoothStep(0, 1, followSpeed * Time.deltaTime));

            hits = Physics.RaycastAll(transform.position, transform.forward, currentDistance - 0.5f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<Renderer>() != null)
                {
                    hitList.Add(hit.transform.gameObject);
                }
            }

            if (prevList.Count > 0)
            {
                foreach (GameObject thing in prevList)
                {
                    if (!hitList.Contains(thing))
                    {
                        Material temp = thing.GetComponent<Renderer>().material;
                        temp.SetFloat("_Mode", 0);
                        temp.renderQueue = 2000;
                    }
                }
            }

            if (hits.Length > 0)
            {
                foreach (GameObject thing in hitList)
                {
                    if (prevList.Contains(thing))
                    {
                        continue;
                    }
                    else
                    {
                        Material temp = thing.GetComponent<Renderer>().material;
                        temp.SetFloat("_Mode", 3);
                        temp.renderQueue = 3001;
                    }
                }
            }
            prevList = new List<GameObject>(hitList);
            hitList.Clear();
        }
    }
}
