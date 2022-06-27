using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]


//ENUM
public enum questDescriptions
{
    tutorial_color_r3 = 0
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

    //BOOLS
    public bool isInTutorialLevel;


    //CALL THIS IN QUEST GIVER START
    public void initializeTutorialQuests()
    {
        quest_color_tut_room3.questID = questDescriptions.tutorial_color_r3;
        quest_color_tut_room3.isActive = true;

        //UI Objectives
        quest_color_tut_room3.UIObjectives[0] = "- Collect journal clues";
        quest_color_tut_room3.UIObjectives[1] = "- Turn on bunsen burner";
        quest_color_tut_room3.UIObjectives[2] = "- Obtain key";

        //descriptive Objectives -- FOR IN GAME CODE RECOGNITION
        quest_color_tut_room3.descriptiveObjectives[0] = "collectClues";
        quest_color_tut_room3.descriptiveObjectives[1] = "completeFire";
        quest_color_tut_room3.descriptiveObjectives[2] = "obtainKey";
        quest_color_tut_room3.currentQuestObjectiveSize = 3;
        quest_color_tut_room3.requiresObjectivesUI = true;

        //NUMBER OF CLUES TO OBTAIN
        quest_color_tut_room3.cluesToObtainAmount = 2;

        //needed game objects
        quest_color_tut_room3.neededGameObjects.Add(GameObject.Find("KEY"));

        //add to dict
        questDict.Add(quest_color_tut_room3.questID, quest_color_tut_room3);
    }
}
