using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class R8_Wires_2 : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    private ParticleSystem.MainModule subEmitter;
    private ParticleSystem.TrailModule tr;
    [SerializeField] private GameObject lampLight;

    // References
    private Inventory inventory;
    private ObjectivePool objectivePool;
    private QuestGiver questGiver;
    private TimelineLevel timelineLevel;

    public override void OAwake()
    {
        ma = ParticleSystem.main;
        tr = ParticleSystem.trails;

        // set the item identification
        Item_Identification = PuzzleItem.R8_WIRES_2;

        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"Missing Script: Inventory.cs");
        }

        objectivePool = FindObjectOfType<ObjectivePool>();
        if (objectivePool == null)
        {
            Debug.LogError($"Missing Script: ObjectivePool.cs");
        }

        questGiver = FindObjectOfType<QuestGiver>();
        if (questGiver == null)
        {
            Debug.LogError($"Missing Script: QuestGiver.cs");
        }

        timelineLevel = FindObjectOfType<TimelineLevel>();
        if (timelineLevel == null)
        {
            Debug.LogError($"Missing Script: TimelineLevel.cs");
        }

        ma = ParticleSystem.main;
    }
    public override void OStart()
    {

    }
    // Subscribe event should only be called once to avoid duplication
    public override void ODelegates()
    {
        D_Item += Event1;
    }

    public void OnDestroy()
    {
        D_Item -= Event1;
    }

    private void Event1(C_Item e)
    {
        // Check if color is correct
        if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.YELLOW)
        {
            // Play sound
            PlayerSFX_Manager.Instance.findSFXSourceByLabel("FixWire").
                PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("FixWire").clip); 

            // disables the interactable UI
            interactableParent.SetActive(false);
            isActive = false;
            canInteract = false;

            // open the light component from the wire
            lampLight.SetActive(true);
            lampLight.GetComponent<Light>().enabled = true;

            //CHANGE ELECTRICITY COLOR
            ma.startColor = inventory.inventorySlots[0].colorMixer.color;
            tr.colorOverLifetime = inventory.inventorySlots[0].colorMixer.color;
            ParticleSystem.GetComponent<Renderer>().materials[1].color = inventory.inventorySlots[0].colorMixer.color;
            subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
            subEmitter.startColor = inventory.inventorySlots[0].colorMixer.color;

            // TRIGGER CORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextCorrect();


            /* Start of Objective Completion / Setting strikethrough to the text's fontStyle*/
            // set the two objectives as complete
            /*
            QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
                .descriptiveObjectives[DescriptiveQuest.R5_REPAIR_WIRE] = true;
            QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
                .descriptiveObjectives[DescriptiveQuest.R5_ON_LIGHT] = true;

            // Update the objectiveList as well; double update 
            objectivePool.itemPool.ReleaseAllPoolable();
            questGiver.UpdateObjectiveList();

            // Check if all objectives are completed
            if (questGiver.currentQuest != null && QuestCollection.Instance.questDict[questGiver.currentQuest.questID].
                    descriptiveObjectives.Values.All(e => e == true))
            {
                questGiver.currentQuest.neededGameObjects.Clear();
            }
            /* End of Objective Completion */

            //PUT CODE HERE TO ENABLE BOX COLLIDER FOR PLANT GROW ANIMATION (NOT CUTSCENE)
        }
        else
        {
            // TRIGGER INCORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextIncorrect();
        }
    }

    public override void OLoadData(GameData data)
    {
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;
        canInteract = false;

        // open the light component from the wire
        lampLight.SetActive(true);

        //CHANGE ELECTRICITY COLOR
        ma.startColor = inventory.inventorySlots[0].colorMixer.color;
        tr.colorOverLifetime = inventory.inventorySlots[0].colorMixer.color;
        ParticleSystem.GetComponent<Renderer>().materials[1].color = inventory.inventorySlots[0].colorMixer.color;
        subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
        subEmitter.startColor = inventory.inventorySlots[0].colorMixer.color;

        // enable the timeline trigger for plant growing cut-scene
        timelineLevel.timelineTriggerCollection[CutSceneTypes.Level5PlantGrow].
            GetComponent<BoxCollider>().enabled = true;
        // open the light component from the lampLight
       lampLight.GetComponent<Light>().enabled = true;

        /* Start of Objective Completion / Setting strikethrough to the text's fontStyle*/
        // set the two objectives as complete
        QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
            .descriptiveObjectives[DescriptiveQuest.R5_REPAIR_WIRE] = true;
        QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
            .descriptiveObjectives[DescriptiveQuest.R5_ON_LIGHT] = true;

        // Update the objectiveList as well; double update 
        objectivePool.itemPool.ReleaseAllPoolable();
        questGiver.UpdateObjectiveList();

        // Check if all objectives are completed
        if (questGiver.currentQuest != null && QuestCollection.Instance.questDict[questGiver.currentQuest.questID].
                descriptiveObjectives.Values.All(e => e == true))
        {
            questGiver.currentQuest.neededGameObjects.Clear();
        }
        /* End of Objective Completion */
    }
}
