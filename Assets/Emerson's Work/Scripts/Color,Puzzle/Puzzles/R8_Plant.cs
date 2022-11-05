using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R8_Plant : PuzzleItemInteraction
{
    private Inventory inventory;
    private Component[] materialComponents;
    private bool isPlantActivated = false;

    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R8_PLANT;

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
        D_Item += Event1;
    }

    public void OnDestroy()
    {
        D_Item -= Event1;
    }

    private void Event1(C_Item e)
    {
        if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.GREEN)
        {
            //CHANGE PLANT COLOR
            materialComponents = this.gameObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer materialComponent in materialComponents)
            {
                materialComponent.material.color = inventory.inventorySlots[0].colorMixer.color;
            }
            isPlantActivated = true;
        }
        else if(inventory.inventorySlots[0].colorMixer.color_code == ColorCode.BLUE && isPlantActivated)
        {
            // disables the interactable UI
            interactableParent.SetActive(false);
            isActive = false;
            canInteract = false;

            // play the stem growing animation
            GetComponent<Animator>().SetBool("willGrow", true);
            Invoke("StopGrowAnim", 2.0f);

            //TRIGGER CORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextCorrect();
        }
        else
        {
            //TRIGGER INCORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextIncorrect();
        }
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

        isPlantActivated = true;
    }

    private void StopGrowAnim()
    {
        GetComponent<Animator>().SetBool("willGrow", false);
    }

}