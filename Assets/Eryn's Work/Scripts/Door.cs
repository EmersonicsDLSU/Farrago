using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Door : PuzzleItemInteraction
{
    public List<PuzzleItem> objectsRequired;

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

    public override bool ConditionBeforeInteraction()
    {
        // Checks if all items are found in the inventory
        if (objectsRequired.All(e => PuzzleInventory.Instance.FindInInventory(e)))
            return true;
        return false;
    }
}
