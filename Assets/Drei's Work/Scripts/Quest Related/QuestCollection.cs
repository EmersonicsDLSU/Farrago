using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class QuestCollection
{
    private QuestCollection()
    {

    }

    private static QuestCollection _instance = null;

    public static QuestCollection Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new QuestCollection();
            }

            return _instance;
        }
    }

    public Dictionary<QuestDescriptions, AQuest> questDict = new Dictionary<QuestDescriptions, AQuest>();
    public bool isInTutorialLevel;

    // initialize quest
    public void InitializeQuests()
    {
        // reset dictionary
        questDict.Clear();

        AQuest quest1 = new AQuest(QuestDescriptions.tutorial_color_r3, true,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R3_COMPLETED_FIRE, false}, {DescriptiveQuest.R3_OBTAINKEY, false}},
            new List<string>() {$"- Turn on bunsen burner", "- Obtain key"},
            new List<GameObject>() {GameObject.Find("KEY")});
        AQuest quest2 = new AQuest(QuestDescriptions.color_r5, true,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R5_REPAIR_WIRE, false}, {DescriptiveQuest.R5_ON_LIGHT, false}},
            new List<string>() {$"- Repair wires", "- Turn on light"},
            new List<GameObject>());
        AQuest quest3 = new AQuest(QuestDescriptions.color_r6, false,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R6_ON_LEFT_LIGHT, false}, {DescriptiveQuest.R6_ON_DESKLIGHT, false}},
            new List<string>() {$"- Repair the correct wire/s", "- Repair desk lamp"},
            new List<GameObject>());
        AQuest quest4 = new AQuest(QuestDescriptions.color_r8, true,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R8_REPAIR_WIRE1, false}, {DescriptiveQuest.R8_REPAIR_WIRE2, false}, 
                    {DescriptiveQuest.R8_COMPLETED_FIRE, false}, {DescriptiveQuest.R8_COLOR_PLANT, false}},
            new List<string>() {$"- Repair wires", "- Turn on bunsen burner", "- Find a way up."},
            new List<GameObject>());

        //add to dict
        questDict.Add(quest1.questID, quest1);
        questDict.Add(quest2.questID, quest2);
        questDict.Add(quest3.questID, quest3);
        questDict.Add(quest4.questID, quest4);
    }
}
