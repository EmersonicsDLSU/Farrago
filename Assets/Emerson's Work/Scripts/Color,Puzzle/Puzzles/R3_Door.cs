using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class R3_Door : PuzzleItemInteraction
{
    public List<PuzzleItem> objectsRequired;

    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R3_DOOR;
    }
    
    public override void OStart()
    {

    }

    // Subscribe event should only be called once to avoid duplication
    public override void ODelegates()
    {
        D_Item += Event1;
    }

    public void OnDestroy()
    {
        D_Item -= Event1;
    }
    
    private void Event1(C_Item e)
    {
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        GetComponent<AudioSource>().Play();
        if (gameObject.activeSelf != false)
        {
            Animator animator = GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Interact");
            }
        }
    }

    public override void OLoadData(GameData data)
    {
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        GetComponent<AudioSource>().Play();
        if (gameObject.activeSelf != false)
        {
            Animator animator = GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("Interact");
            }
        }
    }

    public override bool OBeforeInteraction()
    {
        //Debug.LogError($"IsKeyFound: {objectsRequired.All(e => PuzzleInventory.Instance.FindInInventory(e))}");
        // Checks if all items are found in the inventory
        if (objectsRequired.All(e => PuzzleInventory.Instance.FindInInventory(e)))
            return true;
        return false;
    }
    
    public override bool OFillCompletion()
    {
        if(interactableFill.fillAmount >= 1.0f && !this.GetComponent<AudioSource>().isPlaying)
            return true;
        return false;
    }
}
