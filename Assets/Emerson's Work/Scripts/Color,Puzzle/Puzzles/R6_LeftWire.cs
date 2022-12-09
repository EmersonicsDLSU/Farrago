using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R6_LeftWire : PuzzleItemInteraction
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
        Item_Identification = PuzzleItem.R6_LEFT_WIRE;

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
            lightToOpen.SetActive(true); 
            
            //CHANGE ELECTRICITY COLOR
            ma.startColor = inventory.inventorySlots[0].colorMixer.color;
            tr.colorOverLifetime = inventory.inventorySlots[0].colorMixer.color;
            ParticleSystem.GetComponent<Renderer>().materials[1].color = inventory.inventorySlots[0].colorMixer.color;
            subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
            subEmitter.startColor = inventory.inventorySlots[0].colorMixer.color;

            // if left wire is the only one activated
            if (FindObjectOfType<R6_RightWire>().isActive)
            { 
                assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
                assignedVine.GetComponent<Animator>().SetBool("isRightOn", false);
            }
            // if the right wire has already been activated
            else
            {
                // enable the death timeline trigger
                timelineLevel.timelineTriggerCollection[CutSceneTypes.Level6Dead].
                    GetComponent<BoxCollider>().enabled = true;
                assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
                assignedVine.GetComponent<Animator>().SetBool("isRightOn", true);
            }

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

        // if left wire is the only one activated
        if (FindObjectOfType<R6_RightWire>().isActive)
        {
            assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
            assignedVine.GetComponent<Animator>().SetBool("isRightOn", false);
        }
        // if the right wire has already been activated
        else
        {
            // enable the death timeline trigger
            timelineLevel.timelineTriggerCollection[CutSceneTypes.Level6Dead].
                GetComponent<BoxCollider>().enabled = true;
            assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
            assignedVine.GetComponent<Animator>().SetBool("isRightOn", true);
        }
    }
}
