using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R7_DoorClose : TimelineTrigger
{
    public GameObject door;
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
        // close rat spawner
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R7_Swarm].SetActive(false);
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R8].SetActive(true);
        base.CallEndTimelineEvents();
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        ratSpawnerCollection.spawnerCollection[RatSpawnerArea.R7_Swarm].SetActive(false);
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
        data.cutsceneTriggerPassed.TryGetValue((int)sceneType, out isCompleted);
        if (isCompleted)
        {
            GetComponent<BoxCollider>().enabled = false;
            door.transform.Rotate(0, 90, 0);
            //Debug.LogError($"Close Door!");
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }

    }
    
    public override void OSaveData(GameData data)
    {
        base.OSaveData(data);
    }

}
