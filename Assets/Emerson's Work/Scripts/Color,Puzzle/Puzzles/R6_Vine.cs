using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R6_Vine : PuzzleItemInteraction
{
    private Inventory inventory;
    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R6_VINE;

        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"Missing Script: Inventory.cs");
        }
    }

    public override void OStart()
    {

    }
    
    public override void ODelegates()
    {
        D_Item += (e) =>
        {
            if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.GREEN)
            {
                // disables the interactable UI
                interactableParent.SetActive(false);
                isActive = false;
                canInteract = false;

                // play the stem growing animation
                this.gameObject.GetComponent<Animator>().SetBool("willGrow", true);
                
                //CHANGE VINE COLOR
                this.gameObject.GetComponent<Renderer>().material.color = 
                    inventory.inventorySlots[0].colorMixer.color;

                //TRIGGER CORRECT MONOLOGUE
                Monologues.Instance.triggerPuzzleUITextCorrect();
            }
            else
            {
                //TRIGGER INCORRECT MONOLOGUE
                Monologues.Instance.triggerPuzzleUITextIncorrect();
            }
        };
    }

    public override void OLoadData(GameData data)
    {
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        // play the stem growing animation
        this.gameObject.GetComponent<Animator>().SetBool("willGrow", true);
                
        //CHANGE VINE COLOR
        this.gameObject.GetComponent<Renderer>().material.color = 
            inventory.inventorySlots[0].colorMixer.color;
    }

}
