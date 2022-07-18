using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_SaveLoadHandler : MonoBehaviour, IDataPersistence
{
    // opening the gameScene
    private int total_tries = 0;

    // Start is called before the first frame update
    void Awake()
    {
        // TODO: if player first time playing, we saved the first
        // We saved career file again because we want to add the minutes played on the 
        // main menu portion
        DataPersistenceManager.instance.SaveCareerGame();
        DataPersistenceManager.instance.LoadCareerData();
        // Loads the saved game file
        DataPersistenceManager.instance.LoadGame();
        total_tries++;
        if (total_tries == 1)
        {
            DataPersistenceManager.instance.SaveGame();
        }
    }
    
    public void LoadData(GameData data)
    {
        total_tries = data.total_tries;
    }
    
    public void SaveData(GameData data)
    {
        data.total_tries = total_tries;
    }
}
