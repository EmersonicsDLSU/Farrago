using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineTriggerIdentification : MonoBehaviour
{
    public CutSceneTypes sceneType = CutSceneTypes.None;

    public bool isRepeated = false;

    //external scripts
    public TimelineLevel timelineLevelSc = null;
    public MainPlayerSc player_mainSc = null;
    public RespawnManager respawnManagerSc = null;
    
    private void Start()
    {
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
        if (player_mainSc == null)
        {
            if (FindObjectOfType<MainPlayerSc>() != null) player_mainSc = FindObjectOfType<MainPlayerSc>();
            else Debug.LogError($"Missing \"MainPlayerSc script\" in {this.gameObject.name}");
        }
        if (respawnManagerSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) respawnManagerSc = FindObjectOfType<RespawnManager>();
            else Debug.LogError($"Missing \"RespawnManager script\" in {this.gameObject.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //assigns the latest cutscene
        if(player_mainSc != null)
        {
            MainCharacterStructs.Instance.playerSavedAttrib.recentTrigger = this.gameObject;
        }
        //assigns the location point of this trigger area to be set as a respawn point
        if(respawnManagerSc != null)
        {
            if(this.respawnManagerSc.respawnDictionary.ContainsKey(this.sceneType))
            {
                MainCharacterStructs.Instance.playerSavedAttrib.respawnPoint = 
                    this.respawnManagerSc.respawnDictionary[this.sceneType].transform.position;
                
                this.gameObject.SetActive(this.respawnManagerSc.respawnDictionary[this.sceneType]);
                //Debug.LogError($"Respawn Point Saved: {this.transform.position}");
            }
        }

    }
    void OnEnable()
    {
        this.onceUsed = false;
    }

    [HideInInspector]public bool onceUsed = false;
    private void OnTriggerStay(Collider other)
    {
        //activates the timeline for this trigger
        if(checkTriggerCondition(this.sceneType) && !this.onceUsed)
        {
            Debug.LogError($"Cutscene Start Playing");
            this.onceUsed = true;
            timelineLevelSc.ActivateTimeline(this.sceneType, this.gameObject);
        }
    }

    [HideInInspector]public bool onceChase = false;

    private bool checkTriggerCondition(CutSceneTypes scType)
    {
        switch (scType)
        {
            case CutSceneTypes.Level2JournalChecker:
            {
                if (!Journal.Instance.isJournalObtained)
                    return true;
                else return false;
            }
            case CutSceneTypes.Level4RatCage:
            {
                if (!this.onceChase)
                    {
                        this.onceChase = true;
                        StartCoroutine(BGM_Manager.Instance.SwapTrack(BGM_Manager.Instance.getClipByLabel("Chase")));
                        return true;
                    }
                else return false;
            }
        }

        return true;
    }
}
