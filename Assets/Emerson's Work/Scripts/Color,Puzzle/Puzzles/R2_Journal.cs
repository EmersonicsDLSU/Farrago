using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class R2_Journal : PuzzleItemInteraction
{
    [SerializeField] private GameObject journalHUDText;
    
    public Image journalFirstPage;
    public Image journalSecondPage;
    private Object_ID object_ID;

    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R2_JOURNAL;

        if (Journal.Instance.isJournalObtained == false)
        {
            journalHUDText.SetActive(false);
        }
        else
        {
            journalHUDText.SetActive(true);
        }
        
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
        Debug.LogError($"Journal Is Obtained");

        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        // journal is obtained
        Journal.Instance.isJournalObtained = true;

        // texts are now added after acquiring the journal
        TextControl.Instance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
        TextControl.Instance.delayReset();
            
        // display UI of journal
        journalHUDText.SetActive(true);
            
        this.gameObject.SetActive(false);

        // CALL THE END EVENTS FOR JournalChecker cutscene 
        FindObjectOfType<T_R2_JournalCheck>().CallEndTimelineEvents();
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
