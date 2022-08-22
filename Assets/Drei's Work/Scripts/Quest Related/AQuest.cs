using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AQuest
{
    //private const int MAX_OBJECTIVES_SIZE = 4;

    [Header("Quest Identification")]
    public QuestDescriptions questID;
    public QuestType questType;
    [Space]
    public bool isActive;
    [Space]

    [Header("Objectives")]
    public Dictionary<DescriptiveQuest, bool> descriptiveObjectives = 
        new Dictionary<DescriptiveQuest, bool> ();
    public List<string> UIObjectives = new List<string>();

    public int wiresToRepairAmount;
    public int wiresRepairedAmount;

    [Space]
    public List<GameObject> neededGameObjects = new List<GameObject>();
    [Space]
    public bool requiresObjectivesUI;

    // public constructor
    public AQuest(QuestDescriptions questID, bool isActive, bool requiresObjectivesUI,  
        Dictionary<DescriptiveQuest, bool> descriptiveObjectives, List<string> UIObjectives, List<GameObject> neededGameObjects)
    {
        this.questID = questID;
        this.isActive = isActive;

        //UI Objectives
        this.UIObjectives = UIObjectives;

        //descriptive Objectives -- FOR IN GAME CODE RECOGNITION
        this.descriptiveObjectives = descriptiveObjectives;
        this.requiresObjectivesUI = requiresObjectivesUI;

        //needed game objects
        this.neededGameObjects = neededGameObjects;
    }

    public void questComplete()
    {
        neededGameObjects.Clear();
        isActive = false;
        Debug.LogError("OBJECTIVES COMPLETED");
    }
    
}
