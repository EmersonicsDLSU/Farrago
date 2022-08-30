using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour
{
    //Interact
    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isActive;
    [HideInInspector] public float timePress;
    
    public PuzzleItem Item_Identification;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector] public MainPlayerSc mainPlayer;
    
    void Awake()
    {
        InitializeDelegates();
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

        // assign value to fields
        canInteract = false;
        isActive = true;
    }
    
    // overridable function for Start method
    public virtual void InheritorsStart()
    {

    }
    
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
            case PuzzleItem.R3_KEY:
                break;
            case PuzzleItem.R3_DOOR:
                Gameplay_DelegateHandler.D_R3_OnDoorOpen(new Gameplay_DelegateHandler.C_R3_OnDoorOpen(this.gameObject));
                break;
            case PuzzleItem.R3_BUNSEN_BURNER:
                Gameplay_DelegateHandler.D_R3_OnCompletedFire(new Gameplay_DelegateHandler.C_R3_OnCompletedFire());
                break;
            case PuzzleItem.R5_WIRES:
                Gameplay_DelegateHandler.D_R5_OnWire(new Gameplay_DelegateHandler.C_R5_OnWire(GetComponent<R5_Wires>()));
                break;
            case PuzzleItem.R6_VINE:
                Gameplay_DelegateHandler.D_R6_OnVineGrow(new Gameplay_DelegateHandler.C_R6_OnVineGrow());
                break;
            case PuzzleItem.R6_LEFT_WIRE:
                Gameplay_DelegateHandler.D_R6_LeftWire(new Gameplay_DelegateHandler.C_R6_LeftWire());
                break;
            case PuzzleItem.R6_RIGHT_WIRE:
                Gameplay_DelegateHandler.D_R6_RightWire(new Gameplay_DelegateHandler.C_R6_RightWire());
                break;
            case PuzzleItem.R6_DESK_LAMP:
                Gameplay_DelegateHandler.D_R6_DeskLamp(new Gameplay_DelegateHandler.C_R6_DeskLamp());
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
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
