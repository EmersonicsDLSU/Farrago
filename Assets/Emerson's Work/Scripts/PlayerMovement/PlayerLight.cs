using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private GameObject[] PointLights;

    public void ConfigurePlayerLight(bool isOn)
    {
        foreach(var light in PointLights)
        {
            light.GetComponent<Light>().enabled = isOn;

            //change intensity and range to high to accommodate dark levels
            light.GetComponent<Light>().range = 10;
            light.GetComponent<Light>().intensity = 10;


        }
    }
    public void ConfigurePlayerLight(Color color)
    {
        foreach(var light in PointLights)
        {
            light.GetComponent<Light>().color = color;
        }
        
    }
}
