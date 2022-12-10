using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class R5_Flashlight : PuzzleItemInteraction
{
    private Object_ID object_ID;
    [SerializeField] private ParticleSystem itemParticles;
    public GameObject itemBlocker;

    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R5_FLASHLIGHT;

        object_ID = GetComponent<Object_ID>();
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
        //Debug.LogError($"Flashlight Is Obtained");

        itemBlocker.SetActive(false);
        
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        // flashlight is obtained
        FindObjectOfType<PlayerLight>().isFlashlightObtained = true;
        FindObjectOfType<PlayerLight>().flashlight.SetActive(true);
        
        // Play sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("ItemObtain").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("ItemObtain").clip);

        // texts are now added after acquiring the journal
        //TextControl.Instance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
        //TextControl.Instance.delayReset();
        itemParticles.Stop();
        this.gameObject.SetActive(false);

        // CALL THE END EVENTS FOR JournalChecker cutscene 
        //FindObjectOfType<T_R2_JournalCheck>().CallEndTimelineEvents();
    }

    // once interacted, the journal will be acquired instantly
    public override bool OFillCompletion()
    {
        return true;
    }

    // input pressed condition
    public override bool OInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    
}
