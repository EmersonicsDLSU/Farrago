using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public enum CutSceneTypes
{
    None = 0,
    Level1Intro,
    Level2Intro,
    Level2JournalChecker,
    Level3Intro,
    Level3End,
    Level4Intro,
    Level4RatCage,
    MeltIceScene,
    Level5PlantGrow,
    Level6Transition,
    Level6Dead
}


public class TimelineLevel : MonoBehaviour
{
    //list of timelines in the scene
    [SerializeField] private List<GameObject> timelineObjectsList = new List<GameObject>();
    public List<GameObject> triggerObjectList = new List<GameObject>();

    private Dictionary<CutSceneTypes, GameObject> timelineCollection = new Dictionary<CutSceneTypes, GameObject>();
    private Dictionary<CutSceneTypes, GameObject> timelineTriggerCollection = new Dictionary<CutSceneTypes, GameObject>();
    //current timeline being used / played
    public PlayableDirector currentTimeline = null;
    //current sceneType
    public CutSceneTypes currentSceneType = CutSceneTypes.None;
    //last played scene type
    public CutSceneTypes lastPlayedSceneType;
    //current trigger box
    private GameObject currentTrigger = null;
    //boolean checking of a timeline is currently playing
    [HideInInspector] public bool isTimelinePlayed = false;

    //reference to the game HUD
    private GameObject inGameHUD;

    //reference to HUD functionalities
    [SerializeField] private HUD_Controller hudControllerSc = null;
    [SerializeField] private TooltipHolder tooltipHolderSc = null;

    // mainPlayer reference
    private MainPlayerSc mainPlayer;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = FindObjectOfType<MainPlayerSc>();

        inGameHUD = GameObject.Find("InGameHUD");
        AssignTimelineCollection();

        if (this.hudControllerSc == null)
        {
            if (FindObjectOfType<HUD_Controller>() != null)
            {
                this.hudControllerSc = FindObjectOfType<HUD_Controller>();
            }
            else
            {
                Debug.LogError($"Missing HUD_Controller script in {this.gameObject.name}");
            }
        }
        if (this.tooltipHolderSc == null)
        {
            if (FindObjectOfType<TooltipHolder>() != null)
            {
                this.tooltipHolderSc = FindObjectOfType<TooltipHolder>();
            }
            else
            {
                Debug.LogError($"Missing TooltipHolder script in {this.gameObject.name}");
            }
        }
    }

    //can be called when you want to reactive again a specific scene
    public void resetCutscene(CutSceneTypes sceneType)
    {
        //write here your condition for the specific scene
        switch (sceneType)
        {
            case CutSceneTypes.Level2JournalChecker:
            {
                this.timelineTriggerCollection[sceneType].GetComponent<TimelineTriggerIdentification>().onceUsed = false;
                break;
            }
            case CutSceneTypes.Level4RatCage:
                {
                    //can be called again
                    this.timelineTriggerCollection[sceneType].GetComponent<TimelineTriggerIdentification>().onceChase = false;
                    this.timelineTriggerCollection[sceneType].GetComponent<TimelineTriggerIdentification>().onceUsed = false;
                    break;
                }

        }

        this.timelineCollection[sceneType].SetActive(true);
    }

    //adds each playableDirector's GO from the level to this list; also adds the list of timeline triggers
    private void AssignTimelineCollection()
    {
        //adds to the dictionary
        foreach (var obj in timelineObjectsList)
        {
            this.timelineCollection.Add(obj.GetComponent<TimelineIdentification>().sceneType, obj);
        }
        //adds to the dictionary
        foreach (var obj in triggerObjectList)
        {
            this.timelineTriggerCollection.Add(obj.GetComponent<TimelineTriggerIdentification>().sceneType, obj);
        }
    }

    //play the cutscene for the corresponding trigger
    public void ActivateTimeline(CutSceneTypes sceneType, GameObject triggerGO)
    {
        //disables the HUD for the cutscene DOESNT WORK
        //inGameHUD.SetActive(false);

        this.currentTimeline = this.timelineCollection[sceneType].GetComponent<PlayableDirector>();
        this.currentSceneType = sceneType;
        this.currentTimeline.Play();
        isTimelinePlayed = true;
        this.timelinePlayIsFinished = false;

        this.unfreezeOnce = false;

        //removes the all the HUD
        if (this.hudControllerSc != null)
        {
            this.hudControllerSc.disable_All();
        }
        /*
        if (this.tooltipHolderSc != null)
        {
            this.tooltipHolderSc.unTriggerObjectivesHelp();
            this.tooltipHolderSc.unTriggerSpaceHelp();
            this.tooltipHolderSc.unTriggerWASDHelp();
            this.tooltipHolderSc.untriggerJournalObtainHelp();
        }
        */
        this.currentTrigger = triggerGO;

        //starts a coroutine for the currentTimeline
        StartCoroutine(PlayTimelineRoutine((float)this.currentTimeline.duration));
    }

    private bool unfreezeOnce = false;
    [HideInInspector]public bool timelinePlayIsFinished = false;

    //coroutine to check when the Timeline completed its scene
    private IEnumerator PlayTimelineRoutine(float timelineDuration)
    {
        yield return new WaitForSeconds(timelineDuration);
        // timeline was finished
        this.currentTrigger.GetComponent<TimelineTriggerIdentification>().isCompleted = true;
        //calls the timeline deactivator
        this.timelinePlayIsFinished = true;
        TimelineActiveChecker();
        //resetting journalchecker cutscene
        if(this.lastPlayedSceneType == CutSceneTypes.Level2JournalChecker)
            this.resetCutscene(CutSceneTypes.Level2JournalChecker);
    }

    private void TimelineActiveChecker()
    {
        //checks if the timeline stops playing
        if (!this.currentTrigger.GetComponent<TimelineTriggerIdentification>().isRepeated)
        {
            //enables the HUD after the cutscene DOESNT WORK
            //inGameHUD.SetActive(true);

            this.currentTrigger.SetActive(false);
            this.currentTrigger = null;

            lastPlayedSceneType = currentSceneType;
            currentSceneType = CutSceneTypes.None;
            //this.currentTimeline.Stop();
            this.currentTimeline = null;
            //Unfreezes the player; let the player move again
            isTimelinePlayed = false;

            if (this.hudControllerSc != null)
            {
                this.hudControllerSc.On_unPause();
            }
        }

        //For the repeated timeline; unfreezes the player
        if (this.currentTimeline != null && this.currentTrigger.GetComponent<TimelineTriggerIdentification>().isRepeated &&
            !this.unfreezeOnce)
        {
            //enables the HUD after the cutscene DOESNT WORK
            //inGameHUD.SetActive(true);
            
            lastPlayedSceneType = currentSceneType;
            //Unfreezes the player; let the player move again
            isTimelinePlayed = false;
            if (this.hudControllerSc != null)
            {
                this.hudControllerSc.On_unPause();
            }

            this.unfreezeOnce = true;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        Debug.LogError($"QUEST: {GameObject.Find("QuestGiver").GetComponent<QuestGiver>().lastQuestDone.questID}");
        //reset a specific scene: chase rat scene
        if(lastPlayedSceneType == CutSceneTypes.Level6Dead)
        {
            resetCutscene(CutSceneTypes.Level6Dead);
            // re-position the player transform to its latest re-spawn point
            Debug.LogError($"Dead: {mainPlayer.timelineLevelSc.lastPlayedSceneType}");
            FindObjectOfType<MainPlayerSc>().gameObject.transform.position = 
                DataPersistenceManager.instance.currentLoadedData.respawnPoint;
        }
    }
    
}