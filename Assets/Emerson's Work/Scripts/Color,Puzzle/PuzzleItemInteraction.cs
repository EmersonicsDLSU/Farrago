using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour, IDataPersistence
{
    //Interact
    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public float timePress;
    
    [HideInInspector] public PuzzleItem Item_Identification;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector] public MainPlayerSc mainPlayer;
    
    void Awake()
    {
        ODelegates();
        mainPlayer = FindObjectOfType<MainPlayerSc>();
        OAwake();
    }

    void Start()
    {
        OStart();

        // assign value to fields
        canInteract = false;
        isActive = true;
    }
    
    public void Update()
    {
        InheritorsUpdate();
    }
    
    // Default Update content
    public virtual void InheritorsUpdate()
    {
        if (canInteract && isActive)
        {
            interactableParent.SetActive(true);
            if(OBeforeInteraction())
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    timePress = 0;
                    if (interactableFill != null)
                    {
                        interactableFill.fillAmount = 0.0f;
                    }
                }
                else if (OInput())
                {
                    mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                    timePress += Time.deltaTime;
                    if (interactableFill != null)
                    {
                        interactableFill.fillAmount = timePress / 2.0f;
                    }

                    if (OFillCompletion())
                    {
                        // call the item's events
                        CallItemEvents(Item_Identification);

                        timePress = 0;
                        if (interactableFill != null)
                        {
                            interactableFill.fillAmount = 0.0f;
                        }
                    }
                }
            }
        }
        else
        {
            timePress = 0;
            if (interactableFill != null)
            {
                interactableFill.fillAmount = 0.0f;
            }
            interactableParent.SetActive(false);
        }
    }
    
    // Add here the delegate to be called for a specific puzzle
    protected void CallItemEvents(PuzzleItem item)
    {
        switch (item)
        {
            case PuzzleItem.R2_JOURNAL:
                Gameplay_DelegateHandler.D_R2_OnAcquiredJournal(new Gameplay_DelegateHandler.C_R2_OnAcquiredJournal());
                break;
            case PuzzleItem.R3_KEY:
                break;
            case PuzzleItem.R3_DOOR:
                Gameplay_DelegateHandler.D_R3_OnDoorOpen(new Gameplay_DelegateHandler.C_R3_OnDoorOpen());
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
        OOnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OOnTriggerExit(other);
    }

    // Load system
    public void LoadData(GameData data)
    {
        if (data.objectivesDone.ContainsKey((int) Item_Identification))
        {
            data.objectivesDone.TryGetValue((int)Item_Identification, out isActive);
            if (!isActive)
            {
                Debug.LogError($"Load: {Item_Identification}");
                OLoadData(data);
            }
        }
    }
    
    // Save system
    public void SaveData(GameData data)
    {
        if (data.objectivesDone.ContainsKey((int)Item_Identification))
        {
            data.objectivesDone.Remove((int)Item_Identification);
        }
        data.objectivesDone.Add((int)Item_Identification, isActive);

        OSaveData(data);
    }

    /* VIRTUAL METHODS */
    
    // overridable function for Awake method; Default
    public virtual void OAwake()
    {

    }

    // overridable function for Start method; Default
    public virtual void OStart()
    {

    }
    
    // Inherited class should override this method if they want to add events to the item interaction; Default
    public virtual void ODelegates()
    {

    }

    // input pressed condition; Default
    public virtual bool OInput()
    {
        return Input.GetKey(KeyCode.E);
    }

    // this is the default condition for interaction; Default
    public virtual bool OBeforeInteraction()
    {
        return true;
    }

    // this is the default condition for radial-fill completion; Default
    public virtual bool OFillCompletion()
    {
        if(interactableFill != null && interactableFill.fillAmount >= 1.0f)
            return true;
        return false;
    }
    
    // this is the default condition for OnTriggerEnter; Default
    public virtual void OOnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            this.canInteract = true;
        }
    }

    // this is the default condition for OnTriggerExit; Default
    public virtual void OOnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.canInteract = false;
        }
    }

    // overridable function for load method
    public virtual void OLoadData(GameData data)
    {
        CallItemEvents(Item_Identification);
    }
    
    // overridable function for save method
    public virtual void OSaveData(GameData data)
    {

    }
    
    
}
