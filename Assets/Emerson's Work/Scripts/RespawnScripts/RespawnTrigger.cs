using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() 
    {
        id = System.Guid.NewGuid().ToString();
    }

    //external scripts
    public RespawnManager respawnManagerSc = null;
    public MainPlayerSc player_mainSc = null;
    public RespawnPoints respawnPointEnum;
    private bool is_entered = false;
    
    private void Start()
    {
        GenerateGuid();
        if (respawnManagerSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) respawnManagerSc = FindObjectOfType<RespawnManager>();
            else Debug.LogError($"Missing \"RespawnManager script\" in {this.gameObject.name}");
        }
        if (player_mainSc == null)
        {
            if (FindObjectOfType<MainPlayerSc>() != null) player_mainSc = FindObjectOfType<MainPlayerSc>();
            else Debug.LogError($"Missing \"MainPlayerSc script\" in {this.gameObject.name}");
        }
    }
    
    public void LoadData(GameData data) 
    {
        data.respawnTriggerPassed.TryGetValue(id, out is_entered);
        if (is_entered) 
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data) 
    {
        if (data.respawnTriggerPassed.ContainsKey(id))
        {
            data.respawnTriggerPassed.Remove(id);
        }
        data.respawnTriggerPassed.Add(id, is_entered);
    }

    private void OnTriggerEnter(Collider other)
    {
        //assigns the location point of this trigger area to be set as a respawn point
        if(player_mainSc != null)
        {
            //MainCharacterStructs.Instance.playerSavedAttrib.respawnPoint = this.transform.position;
            MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum = respawnPointEnum;
            // newGame function shouldn't be here
            DataPersistenceManager.instance.NewGame();
            is_entered = true;
            DataPersistenceManager.instance.SaveGame();
            Debug.LogError($"Respawn Point Save In: {this.transform.name}");
        }
        
        this.gameObject.SetActive(false);
    }
}
