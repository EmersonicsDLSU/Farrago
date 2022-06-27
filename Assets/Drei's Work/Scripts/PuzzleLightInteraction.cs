using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLightInteraction : MonoBehaviour
{
    private QuestGiver questGiver;
    [SerializeField] private GameObject lightObject;

    // Start is called before the first frame update
    void Start()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questGiver.currentQuest.questID == questDescriptions.color_r5 && questGiver.currentQuest.wiresRepairedAmount == questGiver.currentQuest.wiresToRepairAmount)
        {
            lightObject.SetActive(true);

            //trigger plant grow cutscene here

            //light lamp objectives complete
            questGiver.completedObjectives.Add("onLight");
            questGiver.strikethroughTextByKey("onLight");
        }
    }
}
