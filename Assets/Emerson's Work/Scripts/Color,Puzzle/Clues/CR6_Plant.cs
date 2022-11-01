using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR6_Plant : ClueInteraction
{
    [SerializeField] ParticleSystem clueParticles;

    public override void OAwake()
    {
        Clue_Identification = E_ClueInteraction.R6_PLANT;
    }

    // once interacted, the clue will be acquired instantly
    public override bool OFillCompletion()
    {
        clueParticles.Stop();
        return true;
    }

    // input pressed condition
    public override bool OInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
