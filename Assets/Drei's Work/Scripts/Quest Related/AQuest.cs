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

    [Header("Objectives")]
    // boolean is used to check whether the objective is done
    public Dictionary<DescriptiveQuest, bool> descriptiveObjectives = 
        new Dictionary<DescriptiveQuest, bool> ();
    public List<string> UIObjectives = new List<string>();

    [Space]
    public List<GameObject> neededGameObjects = new List<GameObject>();
    [Space]
    public bool requiresObjectivesUI;

    // public constructor
    public AQuest(QuestDescriptions questID, bool requiresObjectivesUI,  
        Dictionary<DescriptiveQuest, bool> descriptiveObjectives, List<string> UIObjectives, List<GameObject> neededGameObjects)
    {
        this.questID = questID;

        //UI Objectives
        this.UIObjectives = UIObjectives;

        //descriptive Objectives -- FOR IN GAME CODE RECOGNITION
        this.descriptiveObjectives = descriptiveObjectives;
        this.requiresObjectivesUI = requiresObjectivesUI;

        //needed game objects
        this.neededGameObjects = neededGameObjects;
    }
}
