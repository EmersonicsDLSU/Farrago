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
    

}
