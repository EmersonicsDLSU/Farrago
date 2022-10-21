using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private GameObject PointLight;

    public void ConfigurePlayerLight(bool isOn)
    {
        PointLight.GetComponent<Light>().enabled = isOn;
    }
    public void ConfigurePlayerLight(Color color)
    {
        PointLight.GetComponent<Light>().color = color;
    }
}
