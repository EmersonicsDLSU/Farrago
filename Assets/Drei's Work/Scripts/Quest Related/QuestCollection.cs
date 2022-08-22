using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
//ENUM
/* // transferred this to 'EnumHandler.cs'
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
*/
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

    public Dictionary<QuestDescriptions, AQuest> questDict = new Dictionary<QuestDescriptions, AQuest>();
    public bool isInTutorialLevel;

    // initialize quest
    public void InitializeQuests()
    {
        AQuest quest1 = new AQuest(QuestDescriptions.tutorial_color_r3, true, true,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R3_COMPLETED_FIRE, false}, {DescriptiveQuest.R3_OBTAINKEY, false}},
            new List<string>() {$"- Turn on bunsen burner", "- Obtain key"},
            new List<GameObject>() {GameObject.Find("KEY")});
        AQuest quest2 = new AQuest(QuestDescriptions.color_r5, true, true,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R5_REPAIR_WIRE, false}, {DescriptiveQuest.R5_ON_LIGHT, false}},
            new List<string>() {$"- Repair wires", "- Turn on light"},
            new List<GameObject>());
        quest2.wiresRepairedAmount = 0;
        quest2.wiresToRepairAmount = 2;
        AQuest quest3 = new AQuest(QuestDescriptions.color_r6, true, false,
            new Dictionary<DescriptiveQuest, bool>() 
                {{DescriptiveQuest.R6_ON_LEFT_LIGHT, false}, {DescriptiveQuest.R6_ON_DESKLIGHT, false}},
            new List<string>(),
            new List<GameObject>());

        //add to dict
        questDict.Add(quest1.questID, quest1);
        questDict.Add(quest2.questID, quest2);
        questDict.Add(quest3.questID, quest3);
    }
}
