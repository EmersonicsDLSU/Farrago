using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R8_ScareRat : TimelineTrigger
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

    [SerializeField] private RatSpawner ratSpawner;
    [SerializeField] private GameObject destinationSet;
    [SerializeField] private GameObject[] deActivateDestinations;
    [SerializeField] private GameObject[] ActivateDestinations;

    private void Event1(C_Event e)
    {
        foreach (var pos in deActivateDestinations)
        {
            pos.SetActive(false);
        }
        foreach (var pos in ActivateDestinations)
        {
            pos.SetActive(true);
        }
        foreach (var rat in ratSpawner.enemyPool.usedObjects)
        {
            rat.GetComponentInChildren<EnemyPatrolling>().enemy_property.walkSpeed *= 3;
            rat.GetComponentInChildren<EnemyPatrolling>().assignDestinations(destinationSet);
        }

    }
    private void E_Event1(C_Event e)
    {
        // reduced the rat count to half
        int ratSize = ratSpawner.enemyPool.usedObjects.Count / 2;
        for (int j = 0; j < ratSize; j++)
        {
            ratSpawner.enemyPool.ReleasePoolable(0);
        }
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        // if objective is done
        if (QuestCollection.Instance.questDict[QuestDescriptions.color_r8]
                .descriptiveObjectives[DescriptiveQuest.R8_REPAIR_WIRE1] == true)
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