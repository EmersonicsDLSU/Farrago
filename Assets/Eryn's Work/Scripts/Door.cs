using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Door : PuzzleItemInteraction
{
    public List<PuzzleItem> objectsRequired;

    public override void InheritorsAwake()
    {
        // empty
    }
    
    public override void InheritorsStart()
    {
        // empty
    }

    // Subscribe event should only be called once to avoid duplication
    public override void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R3_OnDoorOpen += (e) =>
        {
            e.doorObj.GetComponent<AudioSource>().Play();
            if (e.doorObj.gameObject.activeSelf != false)
            {
                Animator animator = e.doorObj.GetComponent<Animator>();

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
    
    public override bool ConditionFillCompletion()
    {
        if(interactableFill.fillAmount >= 1.0f && !this.GetComponent<AudioSource>().isPlaying)
            return true;
        return false;
    }
}
