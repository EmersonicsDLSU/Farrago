using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_DelegateHandler : MonoBehaviour
{
    public static Gameplay_DelegateHandler _instance { get; private set; }
    
    public static Gameplay_DelegateHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Gameplay_DelegateHandler>();

                if (_instance == null)
                {
                    _instance = new Gameplay_DelegateHandler();
                }
            }

            return _instance;
        }
    }
    
    void Awake()
    {
        if(_instance != null) Destroy(this);
        DontDestroyOnLoad(this);
    }

    // Action Delegates w/ its corresponding class parameter
    //
    public class C_OnDeath
    {
        public bool isPlayerCaptured;

        public C_OnDeath(bool isPlayerCaptured = false)
        {
            this.isPlayerCaptured = isPlayerCaptured;
        }
    }
    public static Action<C_OnDeath> D_OnDeath = null;
    
    //
    public class C_R3_OnCompletedFire
    {
        public C_R3_OnCompletedFire()
        {

        }
    }
    public static Action<C_R3_OnCompletedFire> D_R3_OnCompletedFire = null;
    
    //
    public class C_R3_OnAcquiredKey
    {
        public C_R3_OnAcquiredKey()
        {

        }
    }
    public static Action<C_R3_OnAcquiredKey> D_R3_OnAcquiredKey = null;

    //
    public class C_R3_OnDoorOpen
    {
        public GameObject doorObj;
        public C_R3_OnDoorOpen(GameObject doorObj)
        {
            this.doorObj = doorObj;
        }
    }
    public static Action<C_R3_OnDoorOpen> D_R3_OnDoorOpen = null;

    //
    public class C_R5_OnWire
    {
        public R5_Wires wireR5;
        public C_R5_OnWire(R5_Wires wireR5)
        {
            this.wireR5 = wireR5;
        }
    }
    public static Action<C_R5_OnWire> D_R5_OnWire = null;

    //
    public class C_R6_OnVineGrow
    {
        public C_R6_OnVineGrow()
        {

        }
    }
    public static Action<C_R6_OnVineGrow> D_R6_OnVineGrow = null;
    
    //
    public class C_R6_RightWire
    {
        public C_R6_RightWire()
        {

        }
    }
    public static Action<C_R6_RightWire> D_R6_RightWire = null;

    //
    public class C_R6_LeftWire
    {
        public C_R6_LeftWire()
        {

        }
    }
    public static Action<C_R6_LeftWire> D_R6_LeftWire = null;

    //
    public class C_R6_DeskLamp
    {
        public C_R6_DeskLamp()
        {

        }
    }
    public static Action<C_R6_DeskLamp> D_R6_DeskLamp = null;
}
