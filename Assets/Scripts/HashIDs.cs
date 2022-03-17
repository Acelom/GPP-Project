using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour
{

    public int movingState;
    public int injuredState;
    public int jumpingState;
    public int actionState;
    public int fallingState;
    public int landingState;
    public int inAirState;
    public int runningState;
    public int slidingState;
    public int superSpeedState;
    public int pressingState; 

    private void Awake()
    {
        movingState = Animator.StringToHash("Moving");
        injuredState = Animator.StringToHash("Injured");
        jumpingState = Animator.StringToHash("Jumping");
        actionState = Animator.StringToHash("Action");
        fallingState = Animator.StringToHash("Falling");
        landingState = Animator.StringToHash("Landing");
        inAirState = Animator.StringToHash("InAir");
        runningState = Animator.StringToHash("Running");
        slidingState = Animator.StringToHash("Sliding");
        superSpeedState = Animator.StringToHash("SuperSpeed");
        pressingState = Animator.StringToHash("Pressing"); 
    }
}
