using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR2_Instruction2 : ClueInteraction
{
    public override void OAwake()
    {
        Clue_Identification = E_ClueInteraction.R2_INSTRUCTION2;
        // add the fundamental image in the journal
        CallItemEvents(Clue_Identification);
        Debug.LogError($"Add Instruct 2: {Clue_Identification}");
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
