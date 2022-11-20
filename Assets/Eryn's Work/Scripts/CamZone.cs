using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CamZone : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera = null;

    [SerializeField] private Movement_Angle movementAngle = Movement_Angle.DEG_270;
    private MainPlayerSc mainPlayerSc;

    private void Start()
    {
        virtualCamera.enabled = false;
        if (FindObjectOfType<MainPlayerSc>() != null)
        {
            mainPlayerSc = FindObjectOfType<MainPlayerSc>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
            //Debug.LogWarning(virtualCamera.enabled);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // assign the movement order based on the camera angle
            if (virtualCamera.isActiveAndEnabled)
            {
                mainPlayerSc.playerMovementSc.movement_angle = movementAngle;

            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false;
            //Debug.LogWarning(virtualCamera.enabled);
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }

}
