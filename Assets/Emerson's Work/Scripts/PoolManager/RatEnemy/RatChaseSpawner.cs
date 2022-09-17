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
        if(timelineLevelSc.currentTimeline == null)
            return;
        if(enemyPool == null)
        {
                Debug.LogError($"Script Empty");
        }
        // checks if the pool storage is not empty and the required existing size still complies
        if (enemyPool.HasObjectAvailable(1) && enemyPool.usedObjects.Count < existingSpawnSize &&
            timelineLevelSc.lastPlayedSceneType == CutSceneTypes.Level4RatCage && 
            timelineLevelSc.timelinePlayIsFinished)
        {
            // request for a new rat
            enemyPool.RequestPoolable();
        }
        // release the rat first if one is being used
        else if(enemyPool.usedObjects.Count > 0 && timelineLevelSc.currentSceneType == CutSceneTypes.Level4RatCage && 
                !timelineLevelSc.timelinePlayIsFinished)
            enemyPool.ReleasePoolable(enemyPool.usedObjects[enemyPool.usedObjects.Count - 1]);
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
