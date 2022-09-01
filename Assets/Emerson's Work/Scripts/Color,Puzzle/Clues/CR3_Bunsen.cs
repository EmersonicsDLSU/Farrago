using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR3_Bunsen : ClueInteraction
{
    public override void OAwake()
    {
        Clue_Identification = E_ClueInteraction.R2_BUNSEN;
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
