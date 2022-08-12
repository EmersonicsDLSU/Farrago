using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedAttributes
{
    public bool IsDead;
    public RespawnPoints respawnPointEnum;

    public SavedAttributes(bool IsDead, RespawnPoints respawnPoint = RespawnPoints.NONE)
    {
        this.IsDead = IsDead;
        this.respawnPointEnum = respawnPoint;
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
    
    public SavedAttributes playerSavedAttrib = new SavedAttributes(false);
}
