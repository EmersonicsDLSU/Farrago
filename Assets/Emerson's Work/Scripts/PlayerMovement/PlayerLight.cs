using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public GameObject flashlight;
    [SerializeField] private GameObject light;
    [HideInInspector]
    public bool isFlashlightObtained = false;
    private bool isFlashlightOn = false;

    void Update()
    {
        if (isFlashlightObtained && Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            if (isFlashlightOn)
            {
                light.SetActive(true);
            } 
            else light.SetActive(false);
            
            // Play sound
            PlayerSFX_Manager.Instance.findSFXSourceByLabel("FlashLight").
                PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("FlashLight").clip);
        }
    }

    public void ConfigureFlashlightColor(Color color)
    {
        foreach (var lightComp in light.GetComponents<Light>())
        {
            lightComp.color = color;
        }
        foreach (var lightComp in light.GetComponentsInChildren<Light>())
        {
            lightComp.color = color;
        }
    }
}
