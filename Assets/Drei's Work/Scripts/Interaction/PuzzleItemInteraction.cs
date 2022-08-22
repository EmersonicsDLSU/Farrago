using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemInteraction : MonoBehaviour
{
    PuzzleInventory playerPuzzleInv;
    private QuestGiver questGiver;

    // Start is called before the first frame update
    void Start()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        playerPuzzleInv = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<PuzzleInventory>();
    }
    


    private void OnTriggerEnter(Collider other)
    {
        playerPuzzleInv.AddToInventory(this.gameObject);
        this.gameObject.SetActive(false);
        // call the delegate for the key captured interaction
        Gameplay_DelegateHandler.D_R3_OnAcquiredKey(new Gameplay_DelegateHandler.C_R3_OnAcquiredKey());

        //questGiver.checkItemObjectives();
        //questGiver.checkIfObjectivesComplete();
    }

    private void OnTriggerExit(Collider other)
    {

    }


}
