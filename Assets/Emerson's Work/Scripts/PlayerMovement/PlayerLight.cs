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
