using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R6_Dead : TimelineTrigger
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
        // transform back to the respawn point
        player_mainSc.gameObject.transform.position = respawnPointsHandler.CurrentRespawnPosition;
        // Reset the right wire in room 6
        FindObjectOfType<R6_RightWire>().ResetWire();
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
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

    }
    
    public override void OSaveData(GameData data)
    {

    }

}
