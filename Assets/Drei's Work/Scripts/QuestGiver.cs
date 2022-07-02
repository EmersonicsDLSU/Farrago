using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{

    public TMP_Text[]objectiveTextsPrefabs;
    public List<string> completedObjectives = new List<string>();
    public QuestCollection questCollection;
    PuzzleInventory playerPuzzleInv;
    public AQuest currentQuest;
    private TimelineLevel TimelineLevel;
    public bool isInQuest;
    public AQuest lastQuestDone;

    public int cluesObtained = 0;


    // Start is called before the first frame update
    void Start()
    {
        questCollection = QuestCollection.Instance;
        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();

        //FOR TESTING, delete when there are multiple levels
        //check first if the player is in a tutorial level
        //insert code here; di na cacall
        //Debug.LogError($"Test Cheez1: {questDescriptions.tutorial_color_r3}");
        questCollection.initializeTutorialQuests();
        questCollection.initializeRoom5Quest();
        questCollection.initializeRoom6Quest();
        //Debug.LogError($"Test Cheez3: {questDescriptions.tutorial_color_r3}");

    }

    // Update is called once per frame
    void Update()
    {
        //IF PLAYER IS ON ROOM 3 TUTORIAL
        
        if (TimelineLevel.currentSceneType == CutSceneTypes.Level3Intro)
        {
            currentQuest = QuestCollection.Instance.questDict[questDescriptions.tutorial_color_r3];
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Length; i++)
            {
                objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
            }
            isInQuest = true;
        }
        //add else ifs here for other missions
        

        
        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL5)
        {
            currentQuest = questCollection.questDict[questDescriptions.color_r5];
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Length; i++)
            {
                objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
                objectiveTextsPrefabs[i].fontStyle = FontStyles.Normal;
            }
            isInQuest = true;
        }
        
        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL6)
        {
            currentQuest = questCollection.questDict[questDescriptions.color_r6];
        }
        


        if (isInQuest == true)
        {
            checkItemObjectives();
            checkIfObjectivesComplete();
        }
        
    }

    public void strikethroughTextByKey(string key)
    {
        int idx = 0;
        foreach (var compObjText in completedObjectives)
        {
            if (compObjText == key)
            {
                objectiveTextsPrefabs[idx].fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                idx++;
            }
        }
    }

    public void checkItemObjectives()
    {
        if (completedObjectives.Count < currentQuest.currentQuestObjectiveSize)
        {
            //checks every object needed to complete objective
            foreach (var gameObject in currentQuest.neededGameObjects)
            {
                //Debug.LogWarning(gameObject.name);
                switch (gameObject.name)
                {
                    //IF A KEY IS REQUIRED
                    case "KEY":
                        //IF A KEY IS OBTAINED
                        if (playerPuzzleInv.FindInInventory("KEY"))
                        {
                            //Debug.LogWarning("AAAAAAAAAAAAAAAAA");
                            completedObjectives.Add("obtainKey");
                            strikethroughTextByKey("obtainKey");
                        }
                        break;
                }
            }
        }
    }

    public void checkIfObjectivesComplete()
    {
        //IF QUEST IS COMPLETE
        if (completedObjectives.Count == currentQuest.currentQuestObjectiveSize)
        {
            lastQuestDone = currentQuest;
            currentQuest.questComplete();
            isInQuest = false;
            currentQuest.neededGameObjects.Clear();
            completedObjectives.Clear();
            currentQuest = null;
        }
    }

    public void clearCurrentGOObjectiveOnQuit()
    {
        //CALL IT ONLY ON UNSAVED QUIT FOR NOW
        currentQuest.clearNeededGameObjectsOnQuit();
    }

    public void setQuestComplete()
    {
        lastQuestDone = currentQuest;
        currentQuest.questComplete();
        isInQuest = false;
        currentQuest.neededGameObjects.Clear();
        completedObjectives.Clear();
        currentQuest = null;
    }
}
