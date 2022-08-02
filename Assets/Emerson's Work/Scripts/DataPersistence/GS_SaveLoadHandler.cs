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
        // We saved career file again because we want to add the minutes played on the 
        // main menu portion
        DataPersistenceManager.instance.SaveCareerGame();
        DataPersistenceManager.instance.LoadCareerData();
        // Loads the saved game file
        DataPersistenceManager.instance.LoadGame(
            DataPersistenceManager.instance.currentSaveFile);
        if (++total_tries == 1)
        {
            Debug.LogError($"Save First Try");
            DataPersistenceManager.instance.SaveGame(
                DataPersistenceManager.instance.currentSaveFile);
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
