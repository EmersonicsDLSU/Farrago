using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R4_RatCage : TimelineTrigger
{
    private RatChaseSpawner ratChaseSpawnerSc;
    public override void OAwake()
    {
        if (ratChaseSpawnerSc == null)
        {
            if (FindObjectOfType<RatChaseSpawner>() != null) ratChaseSpawnerSc = FindObjectOfType<RatChaseSpawner>();
            else Debug.LogError($"Missing \"RatChaseSpawner script\" in {this.gameObject.name}");
        }
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
        ratChaseSpawnerSc.enemyPool.ReleaseAllPoolable();
    }
    private void E_Event1(C_Event e)
    {
        while (ratChaseSpawnerSc.enemyPool.HasObjectAvailable(1))
        {
            ratChaseSpawnerSc.enemyPool.RequestPoolable();
        }
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
        base.OLoadData(data);
    }
    
    public override void OSaveData(GameData data)
    {
        base.OSaveData(data);
    }

}
