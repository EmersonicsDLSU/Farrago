using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour, IDataPersistence
{
    // Delegate
    public class C_Item
    {
        public C_Item()
        {

        }
    }
    public Action<C_Item> D_Item = null;

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
        if (Item_Identification == PuzzleItem.R9_CURE_POTION)
        {
            //Debug.LogError($"IsActive: {isActive} : {canInteract}");
        }
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
                    mainPlayer.playerAngelaAnim.IH_ConsumeAnim(ref mainPlayer, false);
                    timePress = 0;
                    if (interactableFill != null)
                    {
                        interactableFill.fillAmount = 0.0f;
                    }
                }
                else if (OInput())
                {
                    mainPlayer.playerAngelaAnim.IH_ConsumeAnim(ref mainPlayer, true);
                    mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                    timePress += Time.deltaTime;
                    if (interactableFill != null)
                    {
                        interactableFill.fillAmount = timePress / 2.0f;
                    }

                    if (OFillCompletion())
                    {
                        mainPlayer.playerAngelaAnim.IH_ConsumeAnim(ref mainPlayer, false);
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
        // call the delegate of this clue
        if (D_Item != null)
        {
            D_Item(new C_Item());
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
                OLoadData(data);
            }
        }
    }
    
    // Save system
    public void SaveData(GameData data)
    {
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
        if (!isActive)
        {
            CallItemEvents(Item_Identification);
        }
    }
    
    // overridable function for save method
    public virtual void OSaveData(GameData data)
    {
        if (data.objectivesDone.ContainsKey((int)Item_Identification))
        {
            data.objectivesDone.Remove((int)Item_Identification);
        }
        data.objectivesDone.Add((int)Item_Identification, isActive);
    }
    
}
