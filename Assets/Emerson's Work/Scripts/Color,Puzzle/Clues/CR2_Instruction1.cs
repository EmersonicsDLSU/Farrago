using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CR2_Instruction1 : ClueInteraction
{
    public override void OAwake()
    {
        Clue_Identification = E_ClueInteraction.R2_INSTRUCTION1;
        // add the fundamental image in the journal
        CallItemEvents(Clue_Identification);
        //Debug.LogError($"Add Instruct1: {Clue_Identification}");
    }

    public override void CallItemEvents(E_ClueInteraction item)
    {
        // adds the journal to the journalEntries(List)
        //Journal.Instance.journalEntries.Add(Clue_Identification, clueImage);
        //Journal.Instance.journalEntries[Clue_Identification].enabled = true;
        FindObjectOfType<R2_Journal>().journalImages.Add(new JournalImage(Clue_Identification, clueImage));

        // Deactivate this interactable clue
        isActive = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        // swap the recently taken image to the center
        FindObjectOfType<JournalBook>().displayClueImage.sprite = clueImage;
        // display the recently taken image
        if (Clue_Identification != E_ClueInteraction.R2_INSTRUCTION1 && Clue_Identification != E_ClueInteraction.R2_INSTRUCTION2)
        {
            FindObjectOfType<JournalBook>().displayClueImage.enabled = true;
        }

        // edit some text
        interactText.GetComponent<Text>().text = "Close";
        interactText.GetComponent<Text>().text = "Absorb/Interact";
        clueUIText.GetComponent<Animator>().SetBool("isClueObtained", true);
        journalHelpButton.GetComponent<Animator>().SetBool("isClueObtained", true);

        // call the delegate of this clue
        if (D_Clue != null)
        {
            D_Clue(new C_Clue());
        }
    }

    // once interacted, the clue will be acquired instantly
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
