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
    PuzzleInventory playerPuzzleInv;
    private TimelineLevel TimelineLevel;
    private ObjectivePool objectivePool;

    // TODO: Find a better way to place the objectivesPanel activation
    public AQuest currentQuest
    {
        get
        {
            AQuest temp = null;
            if (RespawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL3)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
                FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(true);
            }
            //add else ifs here for other missions
            else if (RespawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL5)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.color_r5];
                FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(true);
            }
            else if (RespawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL6)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.color_r6];
                FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(true);
            }
            else
            {
                //DISABLE OBJECTIVES PANEL IF NOT IN MISSION
                FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(false);
            }
            // edit the 'return' statement if you want to debug a particular room/level
            // e.g. return QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
            return QuestCollection.Instance.questDict[QuestDescriptions.color_r6];

        }
        private set
        {}
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();

        //FOR TESTING, delete when there are multiple levels
        //check first if the player is in a tutorial level
        QuestCollection.Instance.InitializeQuests();

        if(FindObjectOfType<ObjectivePool>() != null)
            objectivePool = FindObjectOfType<ObjectivePool>();

    }
    
    public void UpdateObjectiveList()
    {
        // TODO: What if the objectiveTab is Open, the recently done objective will not be seen as completed through the fontStyle
        
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
        
        // Check if all objectives are completed
        if (currentQuest != null &&
            QuestCollection.Instance.questDict[currentQuest.questID].descriptiveObjectives.Values.All(e => e == true))
        {
            currentQuest.neededGameObjects.Clear();
        }
    }
}
