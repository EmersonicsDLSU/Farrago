using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R3_Key : PuzzleItemInteraction
{
    private QuestGiver questGiver;
    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R3_KEY;

        questGiver = FindObjectOfType<QuestGiver>();
    }
    // removes the default update
    public override void InheritorsUpdate()
    {
        // empty
    }

    // Subscribe event should only be called once to avoid duplication
    public override void ODelegates()
    {
        Gameplay_DelegateHandler.D_R3_OnAcquiredKey += (e) =>
        {
            // set the key objective as completed
            QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3]
                .descriptiveObjectives[DescriptiveQuest.R3_OBTAINKEY] = true;
            isActive = false;
            // Update the objectiveList as well; double update 
            FindObjectOfType<ObjectivePool>().itemPool.ReleaseAllPoolable();
            questGiver.UpdateObjectiveList();
            FindObjectOfType<ObjectivePool>().EnabledAnimation(true);
        };
    }
    
    public override void OOnTriggerEnter(Collider other)
    {
        Debug.LogError($"Key Acquired!!!");
        PuzzleInventory.Instance.AddToInventory(Item_Identification, this.gameObject);
        this.gameObject.SetActive(false);
        // call the delegate for the key captured interaction
        Gameplay_DelegateHandler.D_R3_OnAcquiredKey(new Gameplay_DelegateHandler.C_R3_OnAcquiredKey());
    }

    public override void OLoadData(GameData data)
    {
        // call the delegate for the key captured interaction
        Gameplay_DelegateHandler.D_R3_OnAcquiredKey(new Gameplay_DelegateHandler.C_R3_OnAcquiredKey());

        Debug.LogError($"Key Acquired!!!");
        PuzzleInventory.Instance.AddToInventory(Item_Identification, this.gameObject);
        this.gameObject.SetActive(false);
    }
}
