using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;
    [HideInInspector]
    public bool isFlashlightObtained = false;
    private bool isFlashlightOn = false;

    void Update()
    {
        if (isFlashlightObtained && Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            if (isFlashlightOn) flashlight.SetActive(true);
            else flashlight.SetActive(false);
        }
    }
}
