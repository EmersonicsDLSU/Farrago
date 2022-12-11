using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public string dateCreated;
    public string timeCreated;
    public Vector3 respawnPoint;
    public int total_tries;

    public int currentRespawnPoint;

    // List of respawn and cutscene elements in the game
    public SerializableDictionary<int, bool> respawnTriggerPassed;
    public SerializableDictionary<int, bool> cutsceneTriggerPassed;
    public SerializableDictionary<int, bool> objectivesDone;
    public SerializableDictionary<int, bool> journalImagesTaken;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        respawnPoint = new Vector3(-249.49f, 14.79f, -2.98f);
        this.total_tries = 0;
        respawnTriggerPassed = new SerializableDictionary<int, bool>();
        cutsceneTriggerPassed = new SerializableDictionary<int, bool>();
        objectivesDone = new SerializableDictionary<int, bool>();
        journalImagesTaken = new SerializableDictionary<int, bool>();
    }
    public int GetPercentageComplete() 
    {
        // figure out how many coins we've collected
        int totalCollected = 0;
        foreach (bool notCompleted in objectivesDone.Values) 
        {
            if (!notCompleted) 
            {
                Debug.LogError($"Collected: {((PuzzleItem)totalCollected).ToString()}!");
                totalCollected++;
            }
            else
            {
                Debug.LogError($"Not Collected: {((PuzzleItem)totalCollected).ToString()}!");
            }
        }

        // ensure we don't divide by 0 when calculating the percentage
        int percentageCompleted = -1;
        if (objectivesDone.Count != 0) 
        {
            Debug.LogError($"Total Collected: {totalCollected} : {objectivesDone.Count}");
            percentageCompleted = (totalCollected * 100 / objectivesDone.Count);
        }
        return percentageCompleted;
    }
}
