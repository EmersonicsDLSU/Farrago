using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineLevel : MonoBehaviour
{
    //list of timelines in the scene
    [SerializeField] private List<GameObject> timelineObjectsList = new List<GameObject>();
    public List<GameObject> triggerObjectList = new List<GameObject>();

    public Dictionary<CutSceneTypes, GameObject> timelineCollection = new Dictionary<CutSceneTypes, GameObject>();
    public Dictionary<CutSceneTypes, GameObject> timelineTriggerCollection = new Dictionary<CutSceneTypes, GameObject>();
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
    void Awake()
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
            this.timelineTriggerCollection.Add(obj.GetComponent<TimelineTrigger>().sceneType, obj);
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
        currentTrigger = triggerGO;

        //starts a coroutine for the currentTimeline
        StartCoroutine(PlayTimelineRoutine((float)this.currentTimeline.duration));
    }
    
    [HideInInspector]public bool timelinePlayIsFinished = false;

    //coroutine to check when the Timeline completed its scene
    private IEnumerator PlayTimelineRoutine(float timelineDuration)
    {
        yield return new WaitForSeconds(timelineDuration);
        //calls the timeline deactivator
        timelinePlayIsFinished = true;
        TimelineActiveChecker();
    }

    private void TimelineActiveChecker()
    {
        // timeline was finished; call its end events
        if (currentTrigger != null)
        {
            currentTrigger.GetComponent<TimelineTrigger>().CallEndTimelineEvents();
        }
        currentTrigger = null;

        lastPlayedSceneType = currentSceneType;
        currentSceneType = CutSceneTypes.None;
        //this.currentTimeline.Stop();
        currentTimeline = null;
        //Unfreezes the player; let the player move again
        isTimelinePlayed = false;

        if (hudControllerSc != null)
        {
            hudControllerSc.On_unPause();
        }
    }

    //can be called when you want to reactive again a specific scene
    public void ResetCutscene(CutSceneTypes sceneType)
    {
        timelineTriggerCollection[sceneType].GetComponent<TimelineTrigger>().OOnResetScene();
    }
    
    
}