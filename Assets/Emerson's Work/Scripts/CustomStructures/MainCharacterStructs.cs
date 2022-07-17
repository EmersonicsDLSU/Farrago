using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RespawnPoints
{
    NONE = -1,
    LEVEL1,
    LEVEL2,
    LEVEL3,
    LEVEL4,
    LEVEL4_CHASE,
    LEVEL5,
    LEVEL6,
};

public class SavedAttributes
{
    public Vector3 respawnPoint;
    public bool IsDead;
    public GameObject recentTrigger;
    public RespawnPoints respawnPointEnum;

    public SavedAttributes(Vector3 respawnPoint, bool IsJournalObtained, bool IsDead, GameObject recentTrigger)
    {
        this.respawnPoint = respawnPoint;
        this.IsDead = IsDead;
        this.recentTrigger = recentTrigger;
        this.respawnPointEnum = RespawnPoints.NONE;
    }
}

public sealed class MainCharacterStructs
{
    private static MainCharacterStructs instance;

    public static MainCharacterStructs Instance
    {
        get
        {
            if(instance == null) 
                instance = new MainCharacterStructs();
            return instance;
        }
    }
    
    public SavedAttributes playerSavedAttrib = new SavedAttributes(Vector3.zero, false, false, null);
}
