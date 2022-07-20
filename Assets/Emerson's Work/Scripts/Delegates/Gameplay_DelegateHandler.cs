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
    
}
