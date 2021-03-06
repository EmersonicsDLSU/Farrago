using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TooltipHolder : MonoBehaviour
{
    private TimelineLevel TimelineLevel;
    private GameObject tooltipHelp;
    private bool isMoveTutDone;
    private bool isJumpTutDone;
    [SerializeField] private bool isObjectiveTutDone;
    private Journal playerJournal;
    public bool isObjectivesAvailable;
    public bool isJournalTutDone = false;
    private bool isCleanseTutDone;
    private bool isRunTutDone;
    private Color playerCurrentColor;
    float cleanseTicks = 0.0f;
    private float runTicks = 0.0f;

    //REFERENCE TO THE LEVEL AREA
    private Area_Identifier level2_AreaIdentifier;

    // Start is called before the first frame update
    void Start()
    {
        playerJournal = Journal.Instance;

        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();
        tooltipHelp = GameObject.Find("TooltipText");

        level2_AreaIdentifier = GameObject.Find("Level2").GetComponent<Area_Identifier>();

        playerCurrentColor = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //DISABLE TOOLTIPS WHEN CUTSCENE IS PLAYING
        if (TimelineLevel.isTimelinePlayed)
        {
            tooltipHelp.SetActive(false);
        }
        else
        {
            tooltipHelp.SetActive(true);
        }


        //ROOM 1 MOVEMENT TOOLTIPS
        if (TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level1Intro &&
            !TimelineLevel.isTimelinePlayed && isMoveTutDone == false)
        {
            isJumpTutDone = false;
            

            tooltipHelp.GetComponent<Text>().text = "press W, A, S, D to move";
            triggerTooltipHelp();
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.S)) && isMoveTutDone == false && !TimelineLevel.isTimelinePlayed && TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level1Intro)
        {
            isMoveTutDone = true;
            unTriggerTooltipHelp();

            tooltipHelp.GetComponent<Text>().text = "press SPACE to jump";
            triggerTooltipHelp();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isMoveTutDone == true && isJumpTutDone == false && !TimelineLevel.isTimelinePlayed && TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level1Intro)
        {
            isJumpTutDone = true;

            unTriggerTooltipHelp();
        }

        //ROOM 2 JOURNAL OBTAIN TOOLTIP
        if (playerJournal.isJournalObtained == true && isJournalTutDone == false)
        {
            tooltipHelp.GetComponent<Text>().text = "press J to open Journal";
            triggerTooltipHelp();
        }

        //ROOM 2 JOURNAL CHECK TOOLTIP
        if (TimelineLevel.currentSceneType == CutSceneTypes.Level2JournalChecker  && 
            !TimelineLevel.isTimelinePlayed && playerJournal.isJournalObtained == false)
        {
            tooltipHelp.GetComponent<Text>().text = "find Angela's JOURNAL";
            triggerTooltipHelp();
        }

        if (Input.GetKeyDown(KeyCode.J) && isJournalTutDone == false && !TimelineLevel.isTimelinePlayed && (TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level2Intro ||
               TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level2JournalChecker || level2_AreaIdentifier.level == 2))
        {
            isJournalTutDone = true;
            unTriggerTooltipHelp();
        }

        //ROOM 3 BEGINNING ENABLE JOURNAL
        if (TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level3Intro &&
            !TimelineLevel.isTimelinePlayed && isObjectiveTutDone == false)
        {
            isObjectivesAvailable = true;
            isObjectiveTutDone = false;
            tooltipHelp.GetComponent<Text>().text = "press TAB to open Objectives";
            triggerTooltipHelp();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && isObjectiveTutDone == false && !TimelineLevel.isTimelinePlayed && TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level3Intro)
        {
            isObjectiveTutDone = true;
            unTriggerTooltipHelp();
        }

        //ROOM 3 CLEANSE TUTORIAL
        if (TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level3Intro &&
            !TimelineLevel.isTimelinePlayed && isObjectiveTutDone == true && isCleanseTutDone == false)
        {
            tooltipHelp.GetComponent<Text>().text = "hold R to Cleanse colors";
            triggerTooltipHelp();
        }

        if (Input.GetKey(KeyCode.R) && isCleanseTutDone == false && TimelineLevel.lastPlayedSceneType == CutSceneTypes.Level3Intro &&
            !TimelineLevel.isTimelinePlayed)
        {
            cleanseTicks += Time.deltaTime;


            if (cleanseTicks >= 1.0f)
            {
                playerCurrentColor = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>().material.color;
                if (playerCurrentColor == Color.white)
                {
                    cleanseTicks = 0.0f;
                    isCleanseTutDone = true;
                    unTriggerTooltipHelp();
                }
            }
        }

        //ROOM 4 CHASE RUN TUTORIAL
        if (TimelineLevel.currentSceneType == CutSceneTypes.Level4RatCage &&
            !TimelineLevel.isTimelinePlayed && isRunTutDone == false)
        {
            tooltipHelp.GetComponent<Text>().text = "hold LSHIFT + W/A/S/D to RUN";
            triggerTooltipHelp();
        }

        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) && 
            isRunTutDone == false && TimelineLevel.currentSceneType == CutSceneTypes.Level4RatCage && !TimelineLevel.isTimelinePlayed)
        {
            runTicks += Time.deltaTime;

            Debug.LogWarning(runTicks);

            if (runTicks >= 1.0f)
            {
                runTicks = 0.0f;
                isRunTutDone = true;
                unTriggerTooltipHelp();
            }
        }
    }


    public void triggerTooltipHelp()
    {
        tooltipHelp.GetComponent<Animator>().SetBool("toTriggerHelp", true);
    }

    public void unTriggerTooltipHelp()
    {
        tooltipHelp.GetComponent<Animator>().SetBool("toTriggerHelp", false);
    }

   
}
