using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour
{
    public List<PuzzleItem> objectsRequired;

    //Interact
    [HideInInspector] public bool canInteract = false;
    [HideInInspector] public float timePress;
    [HideInInspector] public bool interactAgain = true;
    [HideInInspector] public bool interacted = false;
    
    public PuzzleItem Item_Identification;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector] public MainPlayerSc mainPlayer;
    
    void Awake()
    {
        InitializeDelegates();
    }

    void Start()
    {
        mainPlayer = FindObjectOfType<MainPlayerSc>();
    }
    
    public void Update()
    {
        InheritorsUpdate();
    }
    
    // Inherited class should override this method if they want to add events to the item interaction
    public virtual void InitializeDelegates()
    {

    }

    // Default Update content
    public virtual void InheritorsUpdate()
    {
        if (canInteract && !interacted)
        {
            interactableParent.SetActive(true);
            Debug.LogError($"Can interact with Door: {objectsRequired.All(e => PuzzleInventory.Instance.FindInInventory(e))} : {PuzzleInventory.puzzleItems.Count}");
            // Checks if all items are found in the inventory
            if (objectsRequired.All(e => PuzzleInventory.Instance.FindInInventory(e)))
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    timePress = 0;
                    interactableFill.fillAmount = 0.0f;
                    interactAgain = true;
                }
                else if (Input.GetKey(KeyCode.E) && interactAgain)
                {
                    mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                    timePress += Time.deltaTime;
                    interactableFill.fillAmount = timePress / 2.0f;

                    if (interactableFill.fillAmount == 1.0f && !this.GetComponent<AudioSource>().isPlaying)
                    {
                        // call the item's events
                        CallItemEvents(Item_Identification);

                        timePress = 0;
                        interactableFill.fillAmount = 0.0f;
                        interactAgain = false;
                        interacted = true;
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

    protected void CallItemEvents(PuzzleItem item)
    {
        switch (item)
        {
            case PuzzleItem.KEY:
                break;
            case PuzzleItem.DOOR:
                Gameplay_DelegateHandler.D_R3_OnDoorOpen(new Gameplay_DelegateHandler.C_R3_OnDoorOpen(this.gameObject));
                break;
        }
    }
}
