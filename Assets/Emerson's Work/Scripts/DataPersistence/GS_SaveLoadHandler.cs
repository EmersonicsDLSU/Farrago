using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GS_SaveLoadHandler : MonoBehaviour, IDataPersistence
{
    // opening the gameScene
    [HideInInspector] public int total_tries = 0;
    
    void Awake()
    {
        DataPersistenceManager.instance.LoadCareerData();
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void LoadGameFromIntroDelay()
    {
        // Loads the saved game file
        DataPersistenceManager.instance.LoadGame(DataPersistenceManager.instance.currentSaveFile);
        Debug.LogError($"Load from GameScene {FindObjectOfType<GS_SaveLoadHandler>().total_tries}");
        if (++FindObjectOfType<GS_SaveLoadHandler>().total_tries == 1)
        {
            Debug.LogError($"Save First Try");
            DataPersistenceManager.instance.SaveGame(DataPersistenceManager.instance.currentSaveFile);
        }
    }

    public void LoadData(GameData data)
    {
        Debug.LogError($"Total tries: {data.total_tries}");
        total_tries = data.total_tries;
    }
    
    public void SaveData(GameData data)
    {
        data.total_tries = total_tries;
    }
    

}
