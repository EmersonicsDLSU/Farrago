using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R9_Start : TimelineTrigger
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
        D_End += E_Event1;
    }

    public void OnDestroy()
    {
        D_Start -= Event1;
        D_End -= E_Event1;
    }

   

    private void Event1(C_Event e)
    {

    }
    private void E_Event1(C_Event e)
    {
        //FindObjectOfType<R9_CurePotion>().EnableInterActable();
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
        // show and disable objects
        foreach (var obj in End_ObjectsToHide)
        {
            obj.SetActive(false);
        }
        foreach (var obj in End_ObjectsToShow)
        {
            obj.SetActive(true);
        }
    }

    public override void OOnTriggerEnter(Collider other)
    {

    }

    private bool onceUsed = false;
    public override void OOnTriggerStay(Collider other)
    {
        // if objective is done
        if (!onceUsed && FindObjectOfType<T_R8_Ending>().isCompleted)
        {
            onceUsed = true;
            base.OOnTriggerEnter(other);
        }
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
