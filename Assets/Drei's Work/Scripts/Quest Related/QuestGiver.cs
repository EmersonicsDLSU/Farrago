using System;
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
    private ObjectivePool objectivePool;


    // Start is called before the first frame update
    void Start()
    {
        questCollection = QuestCollection.Instance;
        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();

        //FOR TESTING, delete when there are multiple levels
        //check first if the player is in a tutorial level
        questCollection.InitializeQuests();
        currentQuest = null;
        lastQuestDone = null;

        if(FindObjectOfType<ObjectivePool>() != null)
            objectivePool = FindObjectOfType<ObjectivePool>();

    }

    // Update is called once per frame
    void Update()
    {
        //IF PLAYER IS ON ROOM 3 TUTORIAL
        
        if (TimelineLevel.currentSceneType == CutSceneTypes.Level3Intro && isInQuest == false)
        {
            currentQuest = QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Count; i++)
            {
                //objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
                objectivePool.RequestAndChangeText(currentQuest.UIObjectives[i]);
            }
            isInQuest = true;
        }

        //add else ifs here for other missions

        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL5 && isInQuest == false)
        {
            currentQuest = questCollection.questDict[QuestDescriptions.color_r5];
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Count; i++)
            {
                
                objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
                objectiveTextsPrefabs[i].fontStyle = FontStyles.Normal;
                
                var go = objectivePool.RequestAndChangeText(currentQuest.UIObjectives[i]);
                go.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
            }
            isInQuest = true;
        }
        
        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL6 && isInQuest == false)
        {
            currentQuest = questCollection.questDict[QuestDescriptions.color_r6];
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
        if (completedObjectives.Count < currentQuest.descriptiveObjectives.Count)
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
        if (completedObjectives.Count == currentQuest.descriptiveObjectives.Count)
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
        currentQuest.neededGameObjects.Clear();
        isInQuest = false;
        completedObjectives.Clear();
        currentQuest = null;
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

    // Doesn't check, but instantly add the description to the completed list
    public void checkPuzzleInteractionObjectives(string descriptiveObjective)
    {
        completedObjectives.Add(descriptiveObjective);
        strikethroughTextByKey(descriptiveObjective);
    }

    public bool canTurnOnLight()
    {
        if (currentQuest.wiresRepairedAmount == currentQuest.wiresToRepairAmount)
        {
            completedObjectives.Add("repairWire");
            strikethroughTextByKey("repairWire");
            completedObjectives.Add("onLight");
            strikethroughTextByKey("onLight");
            return true;
        }

        return false;
    }
}
