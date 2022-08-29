using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R5_Wires : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    private ParticleSystem.MainModule subEmitter;
    private ParticleSystem.TrailModule tr;
    [SerializeField] private GameObject wireLight;
    [SerializeField] private GameObject lampLight;

    // References
    private Inventory inventory;
    private ObjectivePool objectivePool;
    private QuestGiver questGiver;
    private TimelineLevel timelineLevel;
    
    // Conditions
    // for scripts with delegates that should be initialized once
    private bool isInitialized = false;

    public override void InheritorsAwake()
    {
        ma = ParticleSystem.main;
        tr = ParticleSystem.trails;
    }

    public override void InheritorsStart()
    {
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

    // Subscribe event should only be called once to avoid duplication
    public override void InitializeDelegates()
    {
        // if all related scripts are not yet initialize
        if (FindObjectsOfType<R5_Wires>().All(x => !x.isInitialized))
        {
            isInitialized = true;
            Debug.LogError($"WIRE5 INITIALIZED");
            Gameplay_DelegateHandler.D_R5_OnWire += (e) =>
            {
                var wireR5 = e.wireR5;
                // Check if color is correct
                if (wireR5.inventory.inventorySlots[0].colorMixer.color_code == ColorCode.YELLOW)
                {
                    // disables the interactable UI
                    wireR5.interactableParent.SetActive(false);
                    wireR5.isActive = false;

                    // open the light component from the wire
                    wireR5.wireLight.SetActive(true);
                
                    //CHANGE ELECTRICITY COLOR
                    wireR5.ma.startColor = wireR5.inventory.inventorySlots[0].colorMixer.color;
                    wireR5.tr.colorOverLifetime = wireR5.inventory.inventorySlots[0].colorMixer.color;
                    wireR5.ParticleSystem.GetComponent<Renderer>().materials[1].color = wireR5.inventory.inventorySlots[0].colorMixer.color;
                    wireR5.subEmitter = wireR5.ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
                    wireR5.subEmitter.startColor = wireR5.inventory.inventorySlots[0].colorMixer.color;

                    // TRIGGER CORRECT MONOLOGUE
                    Monologues.Instance.triggerPuzzleUITextCorrect();
                
                    // check if both wire lights are open or has been fixed
                    if (FindObjectsOfType<R5_Wires>().All(x => x.gameObject.activeSelf))
                    {
                        // enable the timeline trigger for plant growing cut-scene
                        wireR5.timelineLevel.timelineTriggerCollection[CutSceneTypes.Level5PlantGrow].
                        GetComponent<BoxCollider>().enabled = true;
                        // open the light component from the lampLight
                        wireR5.lampLight.GetComponent<Light>().enabled = true;

                        /* Start of Objective Completion / Setting strikethrough to the text's fontStyle*/
                        // set the two objectives as complete
                        QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
                            .descriptiveObjectives[DescriptiveQuest.R5_REPAIR_WIRE] = true;
                        QuestCollection.Instance.questDict[QuestDescriptions.color_r5]
                            .descriptiveObjectives[DescriptiveQuest.R5_ON_LIGHT] = true;

                        // Update the objectiveList as well; double update 
                        wireR5.objectivePool.itemPool.ReleaseAllPoolable();
                        wireR5.questGiver.UpdateObjectiveList();
                        wireR5.objectivePool.EnabledAnimation(true);

                        // Check if all objectives are completed
                        if (wireR5.questGiver.currentQuest != null && QuestCollection.Instance.questDict[wireR5.questGiver.currentQuest.questID].
                            descriptiveObjectives.Values.All(e => e == true))
                        {
                            wireR5.questGiver.currentQuest.neededGameObjects.Clear();
                        }
                        /* End of Objective Completion */
                    }
                }
                else
                {
                    // TRIGGER INCORRECT MONOLOGUE
                    Monologues.Instance.triggerPuzzleUITextIncorrect();
                }
            };
        }
    }
}
