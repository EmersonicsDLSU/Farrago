using System;
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

    // References
    private Inventory inventory;
    private ObjectivePool objectivePool;
    private QuestGiver questGiver;
    private TimelineLevel timelineLevel;
    
    // Conditions
    // for scripts with delegates that should be initialized once
    private bool isInitialized = false;
    
    public override void OAwake()
    {
        ma = ParticleSystem.main;
        tr = ParticleSystem.trails;

        // set the item identification
        Item_Identification = PuzzleItem.R6_DESK_LAMP;

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

    [SerializeField] private RatSpawner ratSpawner;
    [SerializeField] private GameObject destinationSet;
    [SerializeField] private GameObject[] deActivateDestinations;
    [SerializeField] private GameObject[] ActivateDestinations;

    private void Event1(C_Item e)
    {
        // Check if color is correct
        if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.YELLOW)
        {
            // change the rat's sensor distance
            foreach (var rats in FindObjectsOfType<AISensor>())
            {
                rats.distance = 1;
            }

            // remove some rats destination points
            foreach (var pos in deActivateDestinations)
            {
                pos.SetActive(false);
            }
            foreach (var pos in ActivateDestinations)
            {
                pos.SetActive(true);
            }
            foreach (var rat in ratSpawner.enemyPool.usedObjects)
            {
                rat.GetComponentInChildren<EnemyPatrolling>().assignDestinations(destinationSet);
            }

            // disables the interactable UI
            interactableParent.SetActive(false);
            isActive = false;
            canInteract = false;

            // open the light component from the wire
            lightToOpen.SetActive(true);

            //CHANGE ELECTRICITY COLOR
            ma.startColor = inventory.inventorySlots[0].colorMixer.color;
            tr.colorOverLifetime = inventory.inventorySlots[0].colorMixer.color;
            ParticleSystem.GetComponent<Renderer>().materials[1].color = inventory.inventorySlots[0].colorMixer.color;
            subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
            subEmitter.startColor = inventory.inventorySlots[0].colorMixer.color;
            
            // disable the death timeline trigger
            timelineLevel.timelineTriggerCollection[CutSceneTypes.Level6Dead2].
                GetComponent<BoxCollider>().enabled = false;
            // play the vine animation
            assignedVine.GetComponent<Animator>().SetBool("willGrow", true);
            // enable the death timeline trigger
            timelineLevel.timelineTriggerCollection[CutSceneTypes.Level6Transition].
                GetComponent<BoxCollider>().enabled = true;
            // disable the death timeline trigger
            //Level6DeadTrigger.GetComponent<BoxCollider>().enabled = false;

            //TRIGGER CORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextCorrect();
        }
        else
        {
            //TRIGGER INCORRECT MONOLOGUE
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
        timelineLevel.timelineTriggerCollection[CutSceneTypes.Level6Transition].
            GetComponent<BoxCollider>().enabled = true;
        // disable the death timeline trigger
        //Level6DeadTrigger.GetComponent<BoxCollider>().enabled = false;
    }
}
