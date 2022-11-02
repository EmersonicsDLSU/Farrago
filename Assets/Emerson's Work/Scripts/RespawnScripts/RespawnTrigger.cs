using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour, IDataPersistence
{
    //external scripts
    public MainPlayerSc player_mainSc = null;
    public RespawnPoints respawnPointEnum;
    private bool is_entered = false;
    
    private void Start()
    {
        if (player_mainSc == null)
        {
            if (FindObjectOfType<MainPlayerSc>() != null) player_mainSc = FindObjectOfType<MainPlayerSc>();
            else Debug.LogError($"Missing \"MainPlayerSc script\" in {this.gameObject.name}");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //assigns the location point of this trigger area to be set as a respawn point
        if(player_mainSc != null)
        {
            MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum = respawnPointEnum;
            is_entered = true;
            
            DataPersistenceManager.instance.SaveGame();
            
        }
        // add the current respawnPoint
        RespawnPointsHandler.CurrentRespawnPoint = respawnPointEnum;
        OnEntered(respawnPointEnum);
        // purpose: call the properties function only
        var temp = FindObjectOfType<QuestGiver>().currentQuest;
        //this.gameObject.SetActive(false);
    }

    private void OnEntered(RespawnPoints point)
    {
        switch (point)
        {
            case RespawnPoints.LEVEL5:
            {
                player_mainSc.playerLightSc.ConfigurePlayerLight(true);
            } 
                break;
            case RespawnPoints.LEVEL6:
            {
                player_mainSc.playerLightSc.ConfigurePlayerLight(false);
            } 
                break;
            case RespawnPoints.LEVEL7:
            {
                player_mainSc.playerLightSc.ConfigurePlayerLight(true);
            } 
                break;
            case RespawnPoints.LEVEL8:
            {
                player_mainSc.playerLightSc.ConfigurePlayerLight(false);
            } 
                break;
        }
    }
    
    public void LoadData(GameData data)
    {
        data.respawnTriggerPassed.TryGetValue((int)respawnPointEnum, out is_entered);
        GetComponent<BoxCollider>().enabled = !is_entered;
    }

    public void SaveData(GameData data)
    {
        if (data.respawnTriggerPassed.ContainsKey((int)respawnPointEnum))
        {
            data.respawnTriggerPassed.Remove((int)respawnPointEnum);
        }
        data.respawnTriggerPassed.Add((int)respawnPointEnum, is_entered);
    }
}
