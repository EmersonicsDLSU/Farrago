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
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DataPersistenceManager.instance.LoadCareer();
    }
    public void LoadGameFromIntroDelay()
    {
        // Loads the saved game file
        DataPersistenceManager.instance.LoadGame();
    }

    public void LoadData(GameData data)
    {
        //Debug.LogError($"Total tries: {data.total_tries}");
        total_tries = ++data.total_tries;
    }
    
    public void SaveData(GameData data)
    {
        data.total_tries = total_tries;
    }
    

}
