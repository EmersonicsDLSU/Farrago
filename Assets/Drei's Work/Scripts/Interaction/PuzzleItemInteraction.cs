using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemInteraction : MonoBehaviour
{
    [HideInInspector] public bool canInteract = false;
    PuzzleInventory playerPuzzleInv;
    private QuestGiver questGiver;

    // Start is called before the first frame update
    void Start()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract == true)
        {
            playerPuzzleInv.AddToInventory(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        canInteract = true;
        //questGiver.checkItemObjectives();
        //questGiver.checkIfObjectivesComplete();
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
    }


}
