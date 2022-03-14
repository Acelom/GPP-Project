using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private AnimationCurve yourAnimationCurve;

    public float spin;
    public float movementTime;
    public float top;
    public float bottom;
    public float shrinkTime;
    public bool SpeedCoin;
    public bool JumpCoin;
    


    private Vector3 up;
    private Vector3 down;
    private Vector3 origin;
    private bool goingUp;
    private bool goingDown;
    private bool die;
    private float timer;
    private PlayerControls script; 

    private void Awake()
    {
        origin = transform.position;
        goingUp = true;
        script = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (JumpCoin)
        {
            script.jumpEnabled = true; 
        }
        if (SpeedCoin)
        {
            script.speedEnabled = true; 
        }
        die = true;
    }

    private void Update()
    {
        if (die)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * shrinkTime);
        }

        if (transform.localScale.x < 1)
        {
            Destroy(gameObject);
        }

        up = new Vector3(origin.x, origin.y + top, origin.z);
        down = new Vector3(origin.x, origin.y - bottom, origin.z);

        transform.Rotate(0, spin * Time.deltaTime, 0);

        if (Vector3.Distance(transform.position, up) < 0.1 && goingUp)
        {
            goingUp = false;
            goingDown = true;
            timer = 0;
        }
        if (Vector3.Distance(transform.position, down) < 0.1 && goingDown)
        {
            goingUp = true;
            goingDown = false;
            timer = 0;
        }

        float factor = timer / movementTime;
        factor = yourAnimationCurve.Evaluate(factor);

        if (goingUp)
        {
            transform.position = Vector3.Slerp(transform.position, up, factor);
        }

        if (goingDown)
        {
            transform.position = Vector3.Slerp(transform.position, down, factor);
        }

        timer += Time.deltaTime;

    }

}

