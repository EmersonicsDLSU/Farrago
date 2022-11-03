using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is an exmaple of our spawn type
public class RatChaseSpawner : RatSpawner
{
    private TimelineLevel timelineLevelSc;
    
    public override void OStart()
    {
        enemyPool.Initialize(ref originalObjs, poolableLocation, this);
        if(FindObjectOfType<TimelineLevel>() != null)
        {
            timelineLevelSc = FindObjectOfType<TimelineLevel>();
        }
        else
        {
            Debug.LogError($"Script not found: TimelineLevel in {gameObject.transform.name}");
        }
    }

    // Update is called once per frame
    public override void OUpdate()
    {

    }


    
    public override void OOnRequestGo(List<Transform> spawnLocations)
    {
        // Debug.LogError($"spawnLocations count: {spawnLocations.Count}");
        int randIndex = Random.Range(0, spawnLocations.Count);
        // Debug.LogError($"random index: {randIndex}");
        // random spawn location
        this.enemyPool.usedObjects[this.enemyPool.usedObjects.Count - 1].transform.position = spawnLocations[randIndex].position;
        this.enemyPool.usedObjects[this.enemyPool.usedObjects.Count - 1].transform.SetParent(spawnLocations[randIndex]);
    }
    public override void OOnReleaseGo()
    {

    }
}
