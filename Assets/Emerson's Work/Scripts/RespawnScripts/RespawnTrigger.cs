using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    //external scripts
    public RespawnManager respawnManagerSc = null;
    public MainPlayerSc player_mainSc = null;
    public RespawnPoints respawnPointEnum;
    
    private void Start()
    {
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
    
    private void OnTriggerEnter(Collider other)
    {
        //assigns the location point of this trigger area to be set as a respawn point
        if(player_mainSc != null)
        {
            MainCharacterStructs.Instance.playerSavedAttrib.respawnPoint = this.transform.position;
            MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum = respawnPointEnum;
            Debug.LogError($"Respawn Point Save In: {this.transform.name}");
        }
        
        this.gameObject.SetActive(false);
    }
}
