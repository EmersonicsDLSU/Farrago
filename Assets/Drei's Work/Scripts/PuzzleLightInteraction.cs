using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLightInteraction : MonoBehaviour
{
    private QuestGiver questGiver;
    [SerializeField] private GameObject level5CutsceneTrigger;

    // Start is called before the first frame update
    void Start()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (questGiver.lastQuestDone.questID == questDescriptions.color_r5)
        {
            GetComponent<Light>().enabled = true;
            level5CutsceneTrigger.SetActive(true);
        }
    }

    
}
