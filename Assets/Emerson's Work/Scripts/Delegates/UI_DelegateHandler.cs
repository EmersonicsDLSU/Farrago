using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_DelegateHandler : MonoBehaviour
{
    public static UI_DelegateHandler instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            //Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    
}
