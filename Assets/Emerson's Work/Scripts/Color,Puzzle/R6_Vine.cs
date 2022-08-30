using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R6_Vine : PuzzleItemInteraction
{
    private Inventory inventory;
    public override void InheritorsAwake()
    {

    }

    public override void InheritorsStart()
    {
        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"Missing Script: Inventory.cs");
        }
    }
    
    public override void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R6_OnVineGrow += (e) =>
        {
            if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.GREEN)
            {
                // disables the interactable UI
                interactableParent.SetActive(false);
                isActive = false;

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
    
}
