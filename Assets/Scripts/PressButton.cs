using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private bool moving;
    private bool pressed;
    private ParticleSystem sys;
    private float timer;
    private Material mat;
    private float intensity;


    public Transform target;
    public GameObject door;
    public float movementSpeed;
    public float rotSpeed;
    public float minDistance;
    public float doorSpeed;
    public float doorWait;
    public float lightSpeed;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mat = door.GetComponent<Renderer>().material;
        sys = door.GetComponent<ParticleSystem>();
        sys.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact"))
        {
            moving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        moving = false;
    }

    private void Update()
    {
        if (moving)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, target.transform.position, Time.deltaTime * movementSpeed);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, target.rotation, Time.deltaTime * rotSpeed);
        }

        if (Vector3.Distance(player.transform.position, target.transform.position) < minDistance)
        {
            moving = false;
            pressed = true;
 sys.Play();
        }

        if (pressed & timer > doorWait)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, new Vector3(door.transform.position.x, -10, door.transform.position.z), Mathf.SmoothStep(0, doorSpeed, Time.deltaTime));
           
        }
        else if (pressed)
        {
            timer += Time.deltaTime;
            intensity = Mathf.Lerp(intensity, 2.5f, Time.deltaTime * lightSpeed);
            mat.SetVector("_EmissionColor", Color.yellow * intensity);
        }
    }
}
