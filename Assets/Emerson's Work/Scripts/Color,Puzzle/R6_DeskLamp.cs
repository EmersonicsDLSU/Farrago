using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R6_DeskLamp : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    private ParticleSystem.MainModule subEmitter;
    private ParticleSystem.TrailModule tr;
    [SerializeField] private GameObject lightToOpen;
    [SerializeField] private GameObject assignedVine;
    [SerializeField] private GameObject Level6DeadTrigger;

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
        
    }

    // Subscribe event should only be called once to avoid duplication
    public override void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R6_DeskLamp += (e) =>
        {
            // Check if color is correct
            if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.YELLOW)
            {
                // disables the interactable UI
                interactableParent.SetActive(false);
                isActive = false;

                // open the light component from the wire
                lightToOpen.SetActive(true);

                //CHANGE ELECTRICITY COLOR
                ma.startColor = inventory.inventorySlots[0].colorMixer.color;
                tr.colorOverLifetime = inventory.inventorySlots[0].colorMixer.color;
                ParticleSystem.GetComponent<Renderer>().materials[1].color = inventory.inventorySlots[0].colorMixer.color;
                subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
                subEmitter.startColor = inventory.inventorySlots[0].colorMixer.color;
                
                // play the vine animation
                assignedVine.GetComponent<Animator>().SetBool("willGrow", true);
                // enable the death timeline trigger
                //Level6DeadTrigger.GetComponent<BoxCollider>().enabled = true;

                //TRIGGER CORRECT MONOLOGUE
                Monologues.Instance.triggerPuzzleUITextCorrect();
            }
            else
            {
                //TRIGGER INCORRECT MONOLOGUE
                Monologues.Instance.triggerPuzzleUITextIncorrect();
            }
        };
    }
    
}
