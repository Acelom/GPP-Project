using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    private GameObject target;
    private CameraControls camScript;
    private PlayerControls playerScript;
    private GameObject player;
    private bool targeting;
    private bool lockedOn;
    private float switchTimer;
    private float switchTime;

    public GameObject targeter; 
    public float lockOnSpeed; 
    public float maxDistance;
    public float targeterSpeed; 

    private void Awake()
    {
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControls>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerControls>();
        switchTime = 0.5f; 
    }

    private void Update()
    {

        bool LockOn = Input.GetButtonDown("LockOn");

        if (LockOn)
        {
            lockedOn = !lockedOn;
            targeting = !targeting;
            switchTimer = 0; 
        }

        if (lockedOn & targeting)
        {
            if (FindClosestEnemy() != null)
            {
                target = FindClosestEnemy();
                lockedOn = true;
                targeting = false;
                targeter.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 5, target.transform.position.z); 
            }
            else
            {
                lockedOn = false;
                targeting = false;
            }

        }

        if (lockedOn)
        {
            targeter.GetComponent<Renderer>().enabled = true;
            targeter.GetComponent<Light>().enabled = true; 
            Vector3 newPos = new Vector3(target.transform.position.x, target.transform.position.y + 5, target.transform.position.z);
            targeter.transform.rotation = Quaternion.Euler(targeter.transform.eulerAngles.x, targeter.transform.eulerAngles.y + Time.deltaTime * 200, targeter.transform.eulerAngles.z); 
            targeter.transform.position = Vector3.Lerp(targeter.transform.position, newPos, Time.deltaTime * targeterSpeed); 
              

            switchTimer += Time.deltaTime;

            if (Input.GetAxis("RHorizontal") != 0 && FindNextClosestEnemy() != null && switchTimer > switchTime)
            {
                target = FindNextClosestEnemy();
                switchTimer = 0; 
            }
            camScript.lockOnTarget.transform.position = Vector3.Lerp(camScript.lockOnTarget.transform.position, (camScript.playerTarget.transform.position + target.transform.position) / 2, Time.deltaTime * lockOnSpeed);

            Vector3 lookDirection = target.transform.position - player.transform.position;
            lookDirection.Normalize();
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(lookDirection), lockOnSpeed * Time.deltaTime);
            camScript.transform.rotation = Quaternion.Slerp(camScript.transform.rotation ,getRotation(), Time.deltaTime * lockOnSpeed);
            if (!CloseForTarget() && FindClosestEnemy() != null)
            {
                target = FindClosestEnemy();
            }
            else
            {
                lockedOn = CloseForTarget();
            }
    
            camScript.lockedOn = true;
            playerScript.lockedOn = true;
        }
        else
        {
            targeter.GetComponent<Renderer>().enabled = false;
            targeter.GetComponent<Light>().enabled = false; 
            camScript.lockedOn = false;
            playerScript.lockedOn = false;
        }
    }

    private Quaternion getRotation()
    {
        Quaternion rotation = Quaternion.Euler(5 + 5 * Mathf.Sqrt(Vector3.Distance(target.transform.position, player.transform.position)), player.transform.eulerAngles.y + 5 * Mathf.Sin(player.transform.eulerAngles.y * Mathf.Deg2Rad), 0);
        return rotation;
    }

    private bool CloseForTarget()
    {
        Vector3 diff = target.transform.position - transform.position;
        float Distance = diff.sqrMagnitude;

        return (Distance < maxDistance + 100);
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = maxDistance;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private GameObject FindNextClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        foreach (GameObject go in gos)
        {
            if (go == target)
            {
                break;
            }
            i++;
        }

        gos[i] = null;
        GameObject closest = null;
        float angle = 180;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go != null)
            {
                Vector3 diff = go.transform.position - position;
                float angleDiff = Vector3.SignedAngle(transform.forward, diff, transform.up);
                float curDistance = diff.sqrMagnitude;
                if (curDistance < maxDistance + 100)
                {
                    if (angleDiff * Mathf.Sign(Input.GetAxis("RHorizontal")) > 0 && Mathf.Abs(angleDiff) < angle)
                    {
                        closest = go;
                        angle = Mathf.Abs(angleDiff); 
                    }
                }
            }
        }
        return closest;
    }
}
