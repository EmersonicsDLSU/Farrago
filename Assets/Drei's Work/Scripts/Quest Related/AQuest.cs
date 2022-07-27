using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AQuest
{
    private const int MAX_OBJECTIVES_SIZE = 4;
    private const int MAX_NEEDED_OBJ_SIZE = 4;

    [Header("Quest Identification")]
    public questDescriptions QuestID;
    public questType QuestType;
    public int currentQuestObjectiveSize;

    [Space]
    public bool isActive;
    [Space]
    
    [Header("Objectives")]
    public string[] descriptiveObjectives = new string[MAX_OBJECTIVES_SIZE];
    public string[] UIObjectives = new string[MAX_OBJECTIVES_SIZE];
    public int wiresToRepairAmount;
    public int wiresRepairedAmount;
    [Space]

    public List<GameObject> neededGameObjects = new List<GameObject>();

    [Space]
    public bool requiresObjectivesUI;

    public void questComplete()
    {
        neededGameObjects.Clear();
        isActive = false;
        Debug.LogError("OBJECTIVES COMPLETED");
    }

    public void clearNeededGameObjectsOnQuit()
    {
        //CALL IT ONLY ON UNSAVED QUIT FOR NOW
        neededGameObjects.Clear();
    }
}
