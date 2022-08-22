using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    //
    void Awake()
    {
        InitializeDelegates();
    }

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
            /* // old
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Count; i++)
            {
                // Old
                //objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
                // new
                objectivePool.RequestAndChangeText(currentQuest.UIObjectives[i]);
            }
            */
            isInQuest = true;
        }

        //add else ifs here for other missions

        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL5 && isInQuest == false)
        {
            currentQuest = questCollection.questDict[QuestDescriptions.color_r5];
            /*
            //SETTING UI OBJECTIVES
            for (int i = 0; i < currentQuest.UIObjectives.Count; i++)
            {
                // Old
                //objectiveTextsPrefabs[i].text = currentQuest.UIObjectives[i];
                //objectiveTextsPrefabs[i].fontStyle = FontStyles.Normal;
                // new
                var go = objectivePool.RequestAndChangeText(currentQuest.UIObjectives[i]);
                go.GetComponent<TMP_Text>().fontStyle = FontStyles.Normal;
            }
        */
            isInQuest = true;
        }
        
        else if (MainCharacterStructs.Instance.playerSavedAttrib.respawnPointEnum == RespawnPoints.LEVEL6 && isInQuest == false)
        {
            currentQuest = questCollection.questDict[QuestDescriptions.color_r6];
        }
        


        if (isInQuest == true)
        {
            checkIfObjectivesComplete();
        }
        
    }
    
    
    // Refactored; Only call this everytime a objective is completed
    public void checkIfObjectivesComplete()
    {
        // Check if all objectives are completed
        if (QuestCollection.Instance.questDict[currentQuest.questID].descriptiveObjectives.Values.All(e => e == true))
        {
            lastQuestDone = currentQuest;
            currentQuest.questComplete();
            isInQuest = false;
            currentQuest.neededGameObjects.Clear();
            currentQuest = null;
        }
    }

    public void clearCurrentGOObjectiveOnQuit()
    {
        //CALL IT ONLY ON UNSAVED QUIT FOR NOW
        currentQuest.neededGameObjects.Clear();
        isInQuest = false;
        currentQuest = null;
    }

    public void setQuestComplete()
    {
        lastQuestDone = currentQuest;
        currentQuest.questComplete();
        isInQuest = false;
        currentQuest.neededGameObjects.Clear();
        currentQuest = null;
    }
    
    public void UpdateObjectiveList()
    {
        // TODO: What if the objectiveTab is Open, the recently done objective will not be seen as completed through the fontStyle

        // WARNING: This line below is temporary !!!!
        Debug.LogError($"Warning: Delete the line below; temporary only!!!");
        currentQuest = QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
        // get the order list of the completed objectives
        var objectiveList = currentQuest.descriptiveObjectives.Values.ToList();
        Debug.LogError($"Objective Count: {objectiveList.Count}");
        // makes sure that the player is in a quest
        if (currentQuest != null)
        {
            for (int i = 0; i < currentQuest.UIObjectives.Count; i++)
            {
                // edit the UI Text content based from the set UIObjectives
                var go = objectivePool.RequestAndChangeText(currentQuest.UIObjectives[i]);
                // if objective is completed, then strikeThrough the text
                if (objectiveList[0])
                {
                    go.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                }
                // pop the element from the list
                objectiveList.RemoveAt(0);
            } 
        }
    }

    private void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R3_OnAcquiredKey += (c_onAcquireKey) =>
        {
            // set the key objective as completed
            QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3]
                .descriptiveObjectives[DescriptiveQuest.R3_OBTAINKEY] = true;
            // Update the objectiveList as well; double update 
            FindObjectOfType<ObjectivePool>().itemPool.ReleaseAllPoolable();
            UpdateObjectiveList();
            FindObjectOfType<ObjectivePool>().EnabledAnimation(true);
        };
    }

    public bool canTurnOnLight()
    {
        Debug.LogWarning($"Repaired: {currentQuest.wiresRepairedAmount}/{currentQuest.wiresToRepairAmount}");
        if (currentQuest.wiresRepairedAmount == currentQuest.wiresToRepairAmount)
        {
            // revert
            /*
            strikethroughTextByKey("repairWire");
            strikethroughTextByKey("onLight");
            */
            return true;
        }
        

        return false;
    }
}
