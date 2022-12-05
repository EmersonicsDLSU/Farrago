using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RT_R6 : RespawnTrigger
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
        player_mainSc.playerLightSc.ConfigurePlayerLight(false);
        player_mainSc.playerLightSc.ConfigurePlayerLightLessIntense(true);
        player_mainSc.playerLightSc.isInDarkLevel = false;
        // open rat spawner
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R6].SetActive(true);
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
    public override void OOnResetSavedPoint()
    {
        base.OOnResetSavedPoint();
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
