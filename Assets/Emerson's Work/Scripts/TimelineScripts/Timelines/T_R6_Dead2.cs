using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R6_Dead2 : TimelineTrigger
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
        // close rat spawner
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R6].SetActive(false);
    }

    public override void CallEndTimelineEvents()
    {
        GetComponent<BoxCollider>().enabled = false;
        isCompleted = true;
        // call the delegate of this clue
        if (D_End != null)
        {
            D_End(new C_Event());
        }
        // open rat spawner
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R6].SetActive(true);
        // transform back to the respawn point
        player_mainSc.gameObject.transform.position = respawnPointsHandler.CurrentRespawnPosition;
        // resets the 'isPlayerCaptured' boolean
        Gameplay_DelegateHandler.D_OnDeath(new Gameplay_DelegateHandler.C_OnDeath(isPlayerCaptured:false));
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        if (FindObjectOfType<R6_DeskLamp>().isActive)
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
        timelineLevelSc.ResetCutscene(CutSceneTypes.Level6Dead2);
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

    }

}
