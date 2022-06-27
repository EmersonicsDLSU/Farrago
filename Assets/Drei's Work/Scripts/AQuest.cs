using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class AQuest
{
    private const int MAX_OBJECTIVES_SIZE = 4;
    private const int MAX_NEEDED_OBJ_SIZE = 4;
    public int currentQuestObjectiveSize;

    public bool isActive;
    public string[] descriptiveObjectives = new string[MAX_OBJECTIVES_SIZE];
    public string[] UIObjectives = new string[MAX_OBJECTIVES_SIZE];
    public questDescriptions questID;
    public List<GameObject> neededGameObjects = new List<GameObject>();

    [HideInInspector] public int cluesToObtainAmount = 0;
    [HideInInspector] public int wiresToRepairAmount = 0;
    [HideInInspector] public int wiresRepairedAmount = 0;

    public bool requiresObjectivesUI;

    public void questComplete()
    {
        neededGameObjects.Clear();
        cluesToObtainAmount = 0;
        isActive = false;
        Debug.LogError("OBJECTIVES COMPLETED");
    }

    public void clearNeededGameObjectsOnQuit()
    {
        //CALL IT ONLY ON UNSAVED QUIT FOR NOW
        neededGameObjects.Clear();
    }
}
