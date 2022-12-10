using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class R9_CurePotion : PuzzleItemInteraction
{
    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R9_CURE_POTION;

    }
    
    public void EnableInterActable()
    {
        Debug.LogError($"Show Interactable!");
        interactableParent.SetActive(true);
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
        // Check if timeline is finished, then we can interact
        if (FindObjectOfType<T_R9_Start>().isCompleted)
        {
            Debug.LogError($"Ready To Interact!");
            // disables the interactable UI
            interactableParent.SetActive(false);
            isActive = false;

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

    }
}
