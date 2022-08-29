using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class R6_RightWire : PuzzleItemInteraction
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
            // Check if color is correct and if the left wire is turned on
            if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.YELLOW && FindObjectOfType<R6_LeftWire>().isActive)
            {
                // disables the interactable UI
                interactableParent.SetActive(false);
                isActive = false;

                // open the light component from the wire
                lightToOpen.SetActive(true);
                
                //enable anim of correct vine length
                assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
                assignedVine.GetComponent<Animator>().SetBool("isRightOn", true);
                Invoke("StopVineAnim", 3.0f);

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
    
    private void StopVineAnim()
    {
        assignedVine.GetComponent<Animator>().enabled = false;
    }
}
