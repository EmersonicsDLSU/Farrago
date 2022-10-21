using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProperty
{
    [Tooltip("Player current/minimum speed")]
    public float speed = 6.0f;
    [Tooltip("Player maximum speed")]
    public float maxSpeed = 6.0f;
    [Tooltip("Player Verticallity")]
    public float jumpHeight = 1.0f;
    [Tooltip("Weight in lbs")]
    public float playerWeight = 5.0f;
    [Tooltip("Current color")]
    public Color currentColor;
    [Tooltip("Timer for early jump")]
    public float earlyJumpTime = 0.25f;
    [Tooltip("Timer for delay jump")]
    public float onGroundTimer = 0.25f;
    public float jumpTimer = 0.0f;
    public float rotationToMove = 30.0f;

    //player's condition
    [HideInInspector] public bool isRun = false;
    [HideInInspector] public bool isSneak = false;
    [HideInInspector] public bool canWalk = false;
    [HideInInspector] public bool isJump = false;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool isGround;
    [HideInInspector] public bool isInteract = false;
    [HideInInspector] public bool isDead = false;
    
    //input axes
    [HideInInspector]public readonly float SPEED_MULTIPLIER = 5.0f;
    
    // 'Jump' properties
    [HideInInspector] public float jumpTicks= 0.0f;
    //Early jump properties
    [HideInInspector] public float earlyJumpTicks = 0.0f;
    //cliff jump properties; smoother jump when jumping on an edge of a platform
    [HideInInspector] public float onGroundTicks = 0.0f;

    // Fall Properties
    [HideInInspector] public bool was_Grounded; // boolean if the last frame was onGround
    [HideInInspector] public bool was_Falling; // boolean if the last frame was falling
    [HideInInspector] public bool isFalling; // boolean if the player is now falling in the current frame
    [HideInInspector] public float startOfFallPoint; // highest point reached coming from a jump/fall
    [HideInInspector] public float air_Time1 = 0.0f; // current air_time frame
    [HideInInspector] public float air_Time2 = 0.0f; // last air_time frame
    // vertical measurement from the 'startOfFallPoint' up to the 'landingPoint'
    [HideInInspector] public float fall_Distance = 0.0f;
    [HideInInspector] public float distanceForDeath = 11.50f; // threshold for the fall damage
}
