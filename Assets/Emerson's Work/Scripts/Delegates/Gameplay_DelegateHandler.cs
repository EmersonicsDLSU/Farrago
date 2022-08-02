using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_DelegateHandler : MonoBehaviour
{
    public static Gameplay_DelegateHandler instance { get; private set; }
    
    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
        DontDestroyOnLoad(this);
        
    }

    // Action Delegates w/ its corresponding class parameter
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
