using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineTriggerIdentification : MonoBehaviour, IDataPersistence
{
    public CutSceneTypes sceneType = CutSceneTypes.None;

    public bool isRepeated = false;
    public bool isConditional = false;
    public bool isCompleted = false;

    //external scripts
    public TimelineLevel timelineLevelSc = null;
    public MainPlayerSc player_mainSc = null;
    

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
        DataPersistenceManager.instance.SearchForPersistenceObjInScene();
    }

    private void OnTriggerEnter(Collider other)
    {

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
    
    public void LoadData(GameData data)
    {
        data.cutsceneTriggerPassed.TryGetValue((int)sceneType, out isCompleted);
        if (isCompleted)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        // if the trigger has a condition before enabling the timelineTrigger
        if (isConditional && !isCompleted) 
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SaveData(GameData data)
    {
        if (data.cutsceneTriggerPassed.ContainsKey((int)sceneType))
        {
            data.cutsceneTriggerPassed.Remove((int)sceneType);
        }
        data.cutsceneTriggerPassed.Add((int)sceneType, isCompleted);
    }
}
