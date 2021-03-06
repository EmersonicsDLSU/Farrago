using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]


//ENUM
public enum questDescriptions
{
    tutorial_color_r3,
    color_r5,
    color_r6
};

public enum questType
{
    ObjectActivation,
    VineInteraction,
    WireRepair,
};


public sealed class QuestCollection
{
    private QuestCollection()
    {

    }

    private static QuestCollection instance = null;

    public static QuestCollection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestCollection();
            }

            return instance;
        }
    }


    public Dictionary<questDescriptions, AQuest> questDict = new Dictionary<questDescriptions, AQuest>();

    //QUESTS
    public AQuest quest_color_tut_room3 = new AQuest();
    public AQuest quest_color_room5 = new AQuest();
    public AQuest quest_color_room6 = new AQuest();

    //BOOLS
    public bool isInTutorialLevel;


    //CALL THIS IN QUEST GIVER START
    public void initializeTutorialQuests()
    {
        quest_color_tut_room3.QuestID = questDescriptions.tutorial_color_r3;
        quest_color_tut_room3.isActive = true;

        //UI Objectives
        quest_color_tut_room3.UIObjectives[0] = "- Turn on bunsen burner";
        quest_color_tut_room3.UIObjectives[1] = "- Obtain key";

        //descriptive Objectives -- FOR IN GAME CODE RECOGNITION
        quest_color_tut_room3.descriptiveObjectives[0] = "completeFire";
        quest_color_tut_room3.descriptiveObjectives[1] = "obtainKey";
        quest_color_tut_room3.currentQuestObjectiveSize = 2;
        quest_color_tut_room3.requiresObjectivesUI = true;

        //needed game objects
        quest_color_tut_room3.neededGameObjects.Add(GameObject.Find("KEY"));

        //add to dict
        questDict.Add(quest_color_tut_room3.QuestID, quest_color_tut_room3);
    }

    public void initializeRoom5Quest()
    {
        quest_color_room5.QuestID = questDescriptions.color_r5;
        quest_color_room5.isActive = true;

        //UI Objectives
        quest_color_room5.UIObjectives[0] = "Repair wires";
        quest_color_room5.UIObjectives[1] = "Turn on light";

        //descriptive objectives
        quest_color_room5.descriptiveObjectives[0] = "repairWire";
        quest_color_room5.descriptiveObjectives[1] = "onLight";
        quest_color_room5.currentQuestObjectiveSize = 2;
        quest_color_room5.wiresRepairedAmount = 0;
        quest_color_room5.wiresToRepairAmount = 2;
        quest_color_room5.requiresObjectivesUI = true;

        //add to dict
        questDict.Add(quest_color_room5.QuestID, quest_color_room5);

    }

    public void initializeRoom6Quest()
    {
        quest_color_room6.QuestID = questDescriptions.color_r6;
        quest_color_room6.isActive = true;

        //descriptive objectives
        quest_color_room5.descriptiveObjectives[0] = "onLeftLight";
        quest_color_room5.descriptiveObjectives[1] = "onDeskLight";
        quest_color_room5.currentQuestObjectiveSize = 2;
        quest_color_room6.requiresObjectivesUI = false;

        questDict.Add(quest_color_room6.QuestID, quest_color_room6);
    }
}
