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

    // this is the default condition interaction
    public virtual bool ConditionBeforeInteraction()
    {
        return true;
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

                    if (interactableFill.fillAmount == 1.0f && !this.GetComponent<AudioSource>().isPlaying)
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
