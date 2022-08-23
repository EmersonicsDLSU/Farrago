using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : PuzzleItemInteraction
{
    public override void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R3_OnDoorOpen += (c_onDoorOpen) =>
        {
            c_onDoorOpen.doorObj.GetComponent<AudioSource>().Play();
            if (c_onDoorOpen.doorObj.gameObject.activeSelf != false)
            {
                Animator animator = c_onDoorOpen.doorObj.GetComponent<Animator>();

                if (animator != null)
                {
                    animator.SetTrigger("Interact");
                }
            }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("EXIT");
        if (other.CompareTag("Player"))
            canInteract = false;
    }
}
