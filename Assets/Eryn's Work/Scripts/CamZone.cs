using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = null;

    private void Start()
    {
        virtualCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
            Debug.LogWarning(virtualCamera.enabled);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false;
            Debug.LogWarning(virtualCamera.enabled);
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

}
