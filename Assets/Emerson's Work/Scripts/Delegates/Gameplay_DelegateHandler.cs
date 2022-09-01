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
    
}
