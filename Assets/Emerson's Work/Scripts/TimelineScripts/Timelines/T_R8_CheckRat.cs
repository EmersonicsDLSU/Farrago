using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R8_CheckRat : TimelineTrigger
{
    public override void OAwake()
    {

    }

    public override void OStart()
    {

    }
    
    public override void ODelegates()
    {
        D_Start += Event1;
    }

    public void OnDestroy()
    {
        D_Start -= Event1;
    }

    private void Event1(C_Event e)
    {

    }

    public override void CallEndTimelineEvents()
    {
        // call the delegate of this clue
        if (D_End != null)
        {
            D_End(new C_Event());
        }
        timelineLevelSc.ResetCutscene(CutSceneTypes.Level8CheckRat);
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        if (FindObjectOfType<R8_Wires_1>().isActive)
        {
            base.OOnTriggerEnter(other);
        }
    }
    public override void OOnTriggerStay(Collider other)
    {
        base.OOnTriggerStay(other);
    }
    
    public override void OOnTriggerExit(Collider other)
    {
        base.OOnTriggerExit(other);
    }
    public override void OOnResetScene()
    {
        base.OOnResetScene();
    }
    public override void OLoadData(GameData data)
    {
        base.OLoadData(data);
    }
    
    public override void OSaveData(GameData data)
    {
        base.OSaveData(data);
    }

}
