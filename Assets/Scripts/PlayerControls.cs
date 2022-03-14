using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private float playerSpeed;
    private float walkSpeed;
    private float runSpeed;
    private float superSpeed; 
    private float timer;
    private Camera cam;
    private Quaternion temp;
    private Quaternion yTemp;
    private Animator anim;
    private HashIDs hash;
    private RaycastHit hit;
    private Vector3 slope;
    private float jumpTimer;
    private float speedTimer;
    private bool canDoubleJump;
    private ParticleSystem jumpSys;
    private ParticleSystem speedSys;
    private bool controlled; 


    public float raycastLength;
    public float rotTime;
    public float turnSpeed;
    public float jumpForce;
    public float baseSpeed;
    public float walkMultiplier;
    public float runMultiplier;
    public float superSpeedMultiplier;
    public float airDivider;
    public float landingDivider;
    public float slideDivider; 
    public float slopeLimit;
    public float jumpTimeLimit;
    public float speedTimelimit;
    public bool jumpEnabled;
    public bool speedEnabled;
    public PhysicMaterial mat;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ParticleSystem>() != null)
            {
                if (child.name == "JumpEmitter")
                {
                    jumpSys = child.GetComponent<ParticleSystem>();
                }
                else
                {
                    speedSys = child.GetComponent<ParticleSystem>(); 
                }
            }
        }

        jumpSys.Stop();
        speedSys.Stop(); 
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
 
        if (Physics.Raycast(transform.position + transform.up, -transform.up, out hit, raycastLength))
        {
            anim.SetBool(hash.inAirState, false);
        }
        else
        {
            anim.SetBool(hash.inAirState, true);
        }

        Vector3 left = Vector3.Cross(hit.normal, Vector3.up);
        slope = Vector3.Cross(hit.normal, left);


        if (slope.magnitude * 100 > slopeLimit && anim.GetBool(hash.slidingState))
        {
            mat.dynamicFriction = 0;
            mat.staticFriction = 0;
            mat.frictionCombine = PhysicMaterialCombine.Minimum;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(slope, Vector3.up), rotTime * Time.deltaTime);
            anim.SetBool(hash.slidingState, true); 
        }
    }

    private void Powers()
    {

        if (jumpEnabled && jumpTimer == 0)
        {
            jumpTimer += Time.deltaTime;
            canDoubleJump = true;
            jumpSys.Play(); 
        }
        else if (jumpTimer < jumpTimeLimit && jumpEnabled)
        {
            jumpTimer += Time.deltaTime;
        }
        else
        {
            jumpEnabled = false;
            jumpTimer = 0;
            canDoubleJump = false;
            jumpSys.Stop(); 
        }

        if (speedEnabled && speedTimer == 0)
        {
            speedTimer += Time.deltaTime;
            anim.SetBool(hash.superSpeedState, true);
            speedSys.Play();
        }
        else if (speedTimer < speedTimelimit && speedEnabled)
        {
            speedTimer += Time.deltaTime;
            anim.SetBool(hash.superSpeedState, true);
        }
        else
        {
            anim.SetBool(hash.superSpeedState, false);
            speedEnabled = false;
            speedTimer = 0;
            speedSys.Stop(); 
        }

    }

    private void Update()
    {
        runSpeed = baseSpeed * runMultiplier;
        walkSpeed = baseSpeed * walkMultiplier;
        superSpeed = baseSpeed * superSpeedMultiplier;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
      
        bool jump = Input.GetButtonDown("Jump");
        bool run = Input.GetButton("Run");

        Powers();
        temp = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, rotTime * Time.deltaTime);
        transform.rotation = Quaternion.Euler(temp.eulerAngles.x, transform.rotation.eulerAngles.y, temp.eulerAngles.z);

        float slidingSpeed = 1;

        if (!anim.GetBool(hash.slidingState))
        {
            mat.dynamicFriction = .6f;
            mat.staticFriction = .6f;
            mat.frictionCombine = PhysicMaterialCombine.Average;
        }
        else
        {
            slidingSpeed = slideDivider;
        }

        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            anim.SetBool(hash.movingState, true);
            Vector3 movement = new Vector3(xAxis, 0, yAxis);
            movement = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * movement; 
            yTemp = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), turnSpeed / slidingSpeed);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, yTemp.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            anim.SetBool(hash.movingState, false);
        }
     

        if (slope.magnitude > slopeLimit)
        {
            anim.SetBool(hash.slidingState, true);
        }
        else
        {
            anim.SetBool(hash.slidingState, false);
        }

        anim.SetBool(hash.runningState, run);

        if (GetComponent<Rigidbody>().velocity.y < -.1 && anim.GetBool(hash.inAirState))
        {
            anim.SetBool(hash.fallingState, true);
        }
        else
        {
            if (anim.GetBool(hash.fallingState))
            {
                anim.SetBool(hash.landingState, true);
            }
            else if (timer > 0.25f)
            {
                anim.SetBool(hash.landingState, false);
                timer = 0;
            }

            if (anim.GetBool(hash.landingState))
            {
                timer += Time.deltaTime;
            }

            anim.SetBool(hash.fallingState, false);
        }

        if (anim.GetBool(hash.runningState))
        {
            playerSpeed = runSpeed;
        }
        else
        {
            playerSpeed = walkSpeed;
        }

        if (anim.GetBool(hash.inAirState))
        {
            playerSpeed = playerSpeed / airDivider;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Mathf.SmoothStep(0, 1, 2 * Time.deltaTime * rotTime));
        }
        else if (anim.GetBool(hash.landingState))
        {
            playerSpeed = playerSpeed / landingDivider;
        }
        else
        {
            canDoubleJump = true; 
        }


        if (jump && !anim.GetBool(hash.inAirState))  
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (jump && anim.GetBool(hash.inAirState) && canDoubleJump)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
            canDoubleJump = false; 
        }

        if (GetComponent<Rigidbody>().velocity.y > 0.01f && anim.GetBool(hash.inAirState))
        {
            anim.SetBool(hash.jumpingState, true);
        }
        else
        {
            anim.SetBool(hash.jumpingState, false);
        }

        Vector3 direction = new Vector3(xAxis, 0, yAxis);

        if (anim.GetBool(hash.runningState) || anim.GetBool(hash.superSpeedState))
        {
            direction = direction.normalized;
            anim.speed = 1;
        }
        else
        {
            if (direction.magnitude > 1)
            {
                direction = direction.normalized;
                anim.speed = 1;
            }
            else if (anim.GetBool(hash.movingState))
            {
                anim.speed = direction.magnitude;
            }
            else
            {
                anim.speed = 1;
            }
        }

        if (speedEnabled)
        {
            playerSpeed = superSpeed; 
        }


        if (!anim.GetBool(hash.slidingState))
        {
            transform.position += (transform.forward * direction.magnitude * playerSpeed * Time.deltaTime);
        }
       
   }
}
