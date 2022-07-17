using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string saveGameName;
    public Vector3 respawnPoint;
    public int deathCount;

    public SerializableDictionary<string, bool> respawnTriggerPassed;
    public SerializableDictionary<string, bool> cutsceneTriggerPassed;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        saveGameName = null;
        respawnPoint = Vector3.zero;
        this.deathCount = 0;
        respawnTriggerPassed = new SerializableDictionary<string, bool>();
        cutsceneTriggerPassed = new SerializableDictionary<string, bool>();
    }
}
