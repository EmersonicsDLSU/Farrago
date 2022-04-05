using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SavedAttributes
{
    public Vector3 respawnPoint;
    public bool IsJournalObtained;
    public bool IsDead;
    public GameObject recentTrigger;

    public SavedAttributes(Vector3 respawnPoint, bool IsJournalObtained, bool IsDead, GameObject recentTrigger)
    {
        this.respawnPoint = respawnPoint;
        this.IsJournalObtained = IsJournalObtained;
        this.IsDead = IsDead;
        this.recentTrigger = recentTrigger;
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
