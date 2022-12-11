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
    [HideInInspector] public RespawnPointsHandler respawnPointsHandler = null;

    // TODO: Find a better way to place the objectivesPanel activation
    public AQuest currentQuest
    {
        get
        {
            AQuest temp = null;
            if (respawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL3)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
            }
            //add else ifs here for other missions
            else if (respawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL5)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.color_r5];
            }
            
            else if (respawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL6)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.color_r6];
                //FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(true);
            }

            else if (respawnPointsHandler.CurrentRespawnPoint == RespawnPoints.LEVEL8)
            {
                temp = QuestCollection.Instance.questDict[QuestDescriptions.color_r8];
                //FindObjectOfType<HUD_Controller>().objectivesPanel.SetActive(true);
            }

            // edit the 'return' statement if you want to debug a particular room/level
            // e.g. return QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3];
            return temp;

        }
        private set
        {}
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (respawnPointsHandler == null)
        {
            if (FindObjectOfType<RespawnPointsHandler>() != null) respawnPointsHandler = FindObjectOfType<RespawnPointsHandler>();
            //else Debug.LogError($"Missing \"RespawnPointsHandler script\" in {this.gameObject.name}");
        }

        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();

        //FOR TESTING, delete when there are multiple levels
        //check first if the player is in a tutorial level; if empty then we initialize
        
        QuestCollection.Instance.InitializeQuests();

        if(FindObjectOfType<ObjectivePool>() != null)
            objectivePool = FindObjectOfType<ObjectivePool>();

    }
    
    public void UpdateObjectiveList()
    {
        if (currentQuest == null)
        {
            //Debug.LogError($"Current Quest is currently empty!");
            return;
        }
        // get the order list of the completed objectives
        var objectiveList = currentQuest.descriptiveObjectives.Values.ToList();
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
                // pop the element from the list; move to the next objective
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
