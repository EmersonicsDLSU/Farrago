using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour
{
    //Interact
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public float timePress;
    
    public PuzzleItem Item_Identification;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector] public MainPlayerSc mainPlayer;
    
    void Awake()
    {
        // if delegate should only be subscribe once
        if(isDelegateInitializedOnce)
        {
            // if delegate has not yet subscribed to
            if (!hasInitializedOnce)
            {
                hasInitializedOnce = true;
                InitializeDelegates();
            }
        }
        else
        {
            InitializeDelegates();
        }
        mainPlayer = FindObjectOfType<MainPlayerSc>();
        InheritorsAwake();
    }
    // overridable function for Awake method
    public virtual void InheritorsAwake()
    {

    }

    void Start()
    {
        InheritorsStart();
    }
    
    // overridable function for Start method
    public virtual void InheritorsStart()
    {

    }
    
    /* Important Field */
    [HideInInspector]public bool hasInitializedOnce = false;
    public bool isDelegateInitializedOnce = false;
    // Inherited class should override this method if they want to add events to the item interaction
    public virtual void InitializeDelegates()
    {

    }

    public void Update()
    {
        InheritorsUpdate();
    }
    
    // Default Update content
    public virtual void InheritorsUpdate()
    {
        if (canInteract)
        {
            interactableParent.SetActive(true);
            if(ConditionBeforeInteraction())
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    timePress = 0;
                    interactableFill.fillAmount = 0.0f;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                    timePress += Time.deltaTime;
                    interactableFill.fillAmount = timePress / 2.0f;

                    if (ConditionFillCompletion())
                    {
                        // call the item's events
                        CallItemEvents(Item_Identification);

                        timePress = 0;
                        interactableFill.fillAmount = 0.0f;
                    }
                }
            }
        }
        else
        {
            timePress = 0;
            interactableFill.fillAmount = 0.0f;
            interactableParent.SetActive(false);
        }
    }
    
    // this is the default condition for interaction
    public virtual bool ConditionBeforeInteraction()
    {
        return true;
    }

    // this is the default condition for radial-fill completion
    public virtual bool ConditionFillCompletion()
    {
        if(interactableFill.fillAmount >= 1.0f)
            return true;
        return false;
    }


    // Add here the delegate to be called for a specific puzzle
    protected void CallItemEvents(PuzzleItem item)
    {
        switch (item)
        {
            case PuzzleItem.KEY:
                break;
            case PuzzleItem.DOOR:
                Gameplay_DelegateHandler.D_R3_OnDoorOpen(new Gameplay_DelegateHandler.C_R3_OnDoorOpen(this.gameObject));
                break;
            case PuzzleItem.BUNSENBURNER:
                Gameplay_DelegateHandler.D_R3_OnCompletedFire(new Gameplay_DelegateHandler.C_R3_OnCompletedFire());
                break;
            case PuzzleItem.WIRE_R5:
                Gameplay_DelegateHandler.D_R5_OnWire(new Gameplay_DelegateHandler.C_R5_OnWire());
                break;
            case PuzzleItem.WIRE_R6:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.canInteract = false;
        }
    }
}
