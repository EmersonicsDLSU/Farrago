using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R5_Plant : TimelineTrigger
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
        // add the current respawnPoint
        respawnPointsHandler.CurrentRespawnPosition = transform.position;

        GetComponent<BoxCollider>().enabled = false;
        isCompleted = true;
        // call the delegate of this clue
        if (D_End != null)
        {
            D_End(new C_Event());
        }
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        // if objective is done
        if (QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
            .descriptiveObjectives[DescriptiveQuest.R5_ON_LIGHT] == true)
            base.OOnTriggerEnter(other);
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
