using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R6_LeftWire : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
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

    public override void InheritorsAwake()
    {

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
        Gameplay_DelegateHandler.D_R3_OnCompletedFire += (e) =>
        {
            // Check if color is correct
            if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.ORANGE)
            {
                // disables the interactable UI
                interactableParent.SetActive(false);
                isActive = false;

                // open the light component from the wire
                lightToOpen.SetActive(true);

                // if left wire is the only one activated
                if (FindObjectOfType<R6_RightWire>().isActive)
                {
                    assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
                    assignedVine.GetComponent<Animator>().SetBool("isRightOn", false);
                }
                // if the right wire has already been activated
                else
                {
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
        };
    }
    
}
