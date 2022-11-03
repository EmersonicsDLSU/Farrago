using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour, IDataPersistence
{
    // Delegate
    public class C_Event
    {
        public C_Event()
        {

        }
    }
    public Action<C_Event> D_Start = null;
    public Action<C_Event> D_End = null;
    
    //external scripts
    [HideInInspector] public MainPlayerSc player_mainSc = null;
    [HideInInspector] public RatSpawnerCollection ratSpawnerCollection = null;
    [HideInInspector] public RespawnPointsHandler respawnPointsHandler = null;
    [HideInInspector] public TimelineLevel timelineLevelSc = null;
    public RespawnPoints respawnPointEnum;
    private bool isCompleted = false;

    void Awake()
    {
        ODelegates();
        if (player_mainSc == null)
        {
            if (FindObjectOfType<MainPlayerSc>() != null) player_mainSc = FindObjectOfType<MainPlayerSc>();
            else Debug.LogError($"Missing \"MainPlayerSc script\" in {this.gameObject.name}");
        }
        if (ratSpawnerCollection == null)
        {
            if (FindObjectOfType<RatSpawnerCollection>() != null) ratSpawnerCollection = FindObjectOfType<RatSpawnerCollection>();
            else Debug.LogError($"Missing \"RatSpawnerCollection script\" in {this.gameObject.name}");
        }
        if (respawnPointsHandler == null)
        {
            if (FindObjectOfType<RespawnPointsHandler>() != null) respawnPointsHandler = FindObjectOfType<RespawnPointsHandler>();
            else Debug.LogError($"Missing \"RespawnPointsHandler script\" in {this.gameObject.name}");
        }
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
        OAwake();
    }

    void Start()
    {
        OStart();
    }

    public void Update()
    {
        InheritorsUpdate();
    }
    
    // Default Update content
    public virtual void InheritorsUpdate()
    {

    }
    
    public virtual void CallStartTimelineEvents()
    {
        // call the delegate of this clue
        if (D_Start != null)
        {
            D_Start(new C_Event());
        }
    }

    public virtual void CallEndTimelineEvents()
    {
        // add the current respawnPoint
        respawnPointsHandler.CurrentRespawnPoint = respawnPointEnum;
        respawnPointsHandler.CurrentRespawnPosition = transform.position;
        MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum = respawnPointEnum;

        GetComponent<BoxCollider>().enabled = false;
        isCompleted = true;
        // call the delegate of this clue
        if (D_End != null)
        {
            D_End(new C_Event());
        }
        // save the last position
        DataPersistenceManager.instance.SaveGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        OOnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OOnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OOnTriggerExit(other);
    }

    // Load system
    public void LoadData(GameData data)
    {
        OLoadData(data);
    }
    
    // Save system
    public void SaveData(GameData data)
    {
        OSaveData(data);
    }

    /* VIRTUAL METHODS */
    
    // overridable function for Awake method; Default
    public virtual void OAwake()
    {

    }

    // overridable function for Start method; Default
    public virtual void OStart()
    {

    }
    
    // Inherited class should override this method if they want to add events to the item interaction; Default
    public virtual void ODelegates()
    {

    }

    // this is the default condition for OnTriggerEnter; Default
    public virtual void OOnTriggerEnter(Collider other)
    {
        CallStartTimelineEvents();
        // purpose: call the properties function only
        var temp = FindObjectOfType<QuestGiver>().currentQuest;
        CallEndTimelineEvents();
    }
    public virtual void OOnTriggerStay(Collider other)
    {

    }

    // this is the default condition for OnTriggerExit; Default
    public virtual void OOnTriggerExit(Collider other)
    {

    }
    
    public virtual void OOnResetSavedPoint()
    {
        isCompleted = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    // overridable function for load method
    public virtual void OLoadData(GameData data)
    {
        data.respawnTriggerPassed.TryGetValue((int)respawnPointEnum, out isCompleted);
        GetComponent<BoxCollider>().enabled = !isCompleted;
        if (data.currentRespawnPoint == (int) RespawnPoints.LEVEL7 || data.currentRespawnPoint == (int) RespawnPoints.LEVEL8)
        {
            player_mainSc.playerLightSc.ConfigurePlayerLight(true);
        }
    }
    
    // overridable function for save method
    public virtual void OSaveData(GameData data)
    {
        if (data.respawnTriggerPassed.ContainsKey((int)respawnPointEnum))
        {
            data.respawnTriggerPassed.Remove((int)respawnPointEnum);
        }
        data.respawnTriggerPassed.Add((int)respawnPointEnum, isCompleted);
    }
}
