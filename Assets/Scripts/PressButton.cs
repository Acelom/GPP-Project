using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private CameraControls camScript;
    private PlayerControls playerScript;
    private bool moving;
    private bool pressing;
    private bool pressed;
    private ParticleSystem sys;
    private float timer;
    private Material mat;
    private float intensity;
    private Animator anim;
    private HashIDs hash;
    private float pressingTimer;
    private float endTimer;
    private bool stop;
    private Vector3 doorPos; 



    public Transform target;
    public GameObject door;
    public float movementSpeed;
    public float rotSpeed;
    public float minDistance;
    public float doorSpeed;
    public float doorWait;
    public float lightSpeed;
    public float pressingLimit;
    public Transform cutscenePos;
    public float indentTime;
    public float indentSpeed;
    public float endingTime; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mat = door.GetComponent<Renderer>().material;
        sys = door.GetComponent<ParticleSystem>();
        anim = player.GetComponent<Animator>(); 
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
        camScript = cam.gameObject.GetComponent<CameraControls>();
        playerScript = player.gameObject.GetComponent<PlayerControls>();
        sys.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && other.tag == "Player")
        {
            moving = true;
            GetComponent<BoxCollider>().enabled = false;
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

        if (Vector3.Distance(player.transform.position, target.transform.position) < minDistance && !stop && moving)
        {
            playerScript.cutscene = true;
            cutscenePos.LookAt((transform.position+ player.transform.position) / 2); 
            moving = false;
            pressing = true;
            stop = true; 
        }
        
        if (pressing && pressingTimer < pressingLimit)
        {

            anim.SetBool(hash.pressingState, pressing); 
            pressingTimer += Time.deltaTime;
        }
        else if (pressingTimer > pressingLimit && pressing)
        {

            pressing = false;
            pressed = true;
            sys.Play();
            anim.SetBool(hash.pressingState, pressing);
            doorPos = door.transform.position;
            cutscenePos.LookAt(doorPos);
            cutscenePos.position = new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z + 10);
            pressingTimer = 0; 
        }

        if (pressingTimer > indentTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0.1f, 0), indentSpeed * Time.deltaTime); 
        }
        
        if (pressed)
        {
            pressing = false; 
        }
       
        if (pressed & timer > doorWait)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, new Vector3(door.transform.position.x, -10, door.transform.position.z), Mathf.SmoothStep(0, doorSpeed, Time.deltaTime));
            endTimer += Time.deltaTime;
            cutscenePos.LookAt(doorPos);

        }
        else if (pressed)
        {

         
            timer += Time.deltaTime;
            intensity = Mathf.Lerp(intensity, 2.5f, Time.deltaTime * lightSpeed);
            mat.SetVector("_EmissionColor", Color.yellow * intensity);
            cutscenePos.LookAt(doorPos);
           
        }

        if (endTimer > endingTime)
        {
            playerScript.cutscene = false;
            camScript.currMode = CameraControls.camMode.follow; 
        }

        if (door.transform.position.y < -4.5)
        {
            sys.Stop(); 
            Destroy(this); 
        }

        if (playerScript.cutscene)
        {
            camScript.currMode = CameraControls.camMode.cutscene;
        }
    }
}
