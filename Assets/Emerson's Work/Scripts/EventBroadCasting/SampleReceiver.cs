using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleReceiver : MonoBehaviour
{
    public GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        ExampleManager.Instance.sampleClass += OnTapHere;
    }

    private void OnDisable()
    {
        ExampleManager.Instance.sampleClass -= OnTapHere;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTapHere(object sender, SampleEventArgs args)
    {
        //you can now add functions here together with the class obj passed with its set values
    }

}
