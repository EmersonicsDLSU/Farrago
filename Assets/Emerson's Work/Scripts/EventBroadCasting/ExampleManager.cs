using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ExampleManager : MonoBehaviour
{
    public EventHandler<SampleEventArgs> sampleClass;

    [HideInInspector] public static ExampleManager Instance;

    //Gets the properties for the system
    public ExampleProperty _exampleProperty;

    // Start is called before the first frame update
    public void Awake()
    {
        //assigns the one instance
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            //destroys the duplicate gameObject 
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    void Update()
    {

    }

    private void callEvent()
    {
        SampleEventArgs args = new SampleEventArgs(0, true);
        if(sampleClass != null)
        {
            sampleClass(this, args);
        }
    }
}
