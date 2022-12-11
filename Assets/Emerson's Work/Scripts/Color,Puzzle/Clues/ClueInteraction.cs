using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Time = UnityEngine.Time;

public abstract class ClueInteraction : MonoBehaviour, IDataPersistence
{
    // Delegate
    public class C_Clue
    {
        public C_Clue()
        {

        }
    }
    public Action<C_Clue> D_Clue = null;

    //Interact
    [HideInInspector] public bool canInteract;
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public float timePress;

    [HideInInspector] public E_ClueInteraction Clue_Identification;

    [Space] [Header("Interactables")] 
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector] public MainPlayerSc mainPlayer;
    
    public Sprite clueImage;

    public GameObject clueUIText;
    public GameObject interactText;
    public GameObject journalHelpButton;
    
    private Vector2 imageInitPos;
    private Object_ID object_ID;

    // PlayerSFXManager Instance reference - used for absorbed sfx
    private PlayerSFX_Manager playerSFX;

    private void Awake()
    {
        object_ID = GetComponent<Object_ID>();

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

        playerSFX = PlayerSFX_Manager.Instance;
    }
    
    private void Update()
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
                        CallItemEvents(Clue_Identification);
                        playerSFX.findSFXSourceByLabel("ClueObtain").PlayOneShot(playerSFX.findSFXSourceByLabel("ClueObtain").clip);

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
    public virtual void CallItemEvents(E_ClueInteraction item)
    {
        // adds the journal to the journalEntries(List)
        //Journal.Instance.journalEntries.Add(Clue_Identification, clueImage);
        //Journal.Instance.journalEntries[Clue_Identification].enabled = true;
        FindObjectOfType<R2_Journal>().journalImages.Add(new JournalImage(Clue_Identification, clueImage));

        // Deactivate this interactable clue
        isActive = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        // swap the recently taken image to the center
        FindObjectOfType<JournalBook>().displayClueImage.sprite = clueImage;
        // display the recently taken image
        if (Clue_Identification != E_ClueInteraction.R2_INSTRUCTION1 && Clue_Identification != E_ClueInteraction.R2_INSTRUCTION2)
        {
            FindObjectOfType<JournalBook>().displayClueImage.enabled = true;
        }

        // edit some text
        interactText.GetComponent<Text>().text = "Close";
        interactText.GetComponent<Text>().text = "Absorb/Interact";
        TextControl.Instance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
        TextControl.Instance.delayReset();
        clueUIText.GetComponent<Animator>().SetBool("isClueObtained", true);
        journalHelpButton.GetComponent<Animator>().SetBool("isClueObtained", true);

        // call the delegate of this clue
        if (D_Clue != null)
        {
            D_Clue(new C_Clue());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OOnTriggerEnter(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // disabled the image display
            FindObjectOfType<JournalBook>().displayClueImage.enabled = false;
        }
        OOnTriggerExit(other);
    }
    
    // Load system
    public void LoadData(GameData data)
    {
        if (data.journalImagesTaken.ContainsKey((int) Clue_Identification))
        {
            data.journalImagesTaken.TryGetValue((int)Clue_Identification, out isActive);
            if (!isActive)
            {
                OLoadData(data);
            }
        }
    }
    
    // Save system
    public void SaveData(GameData data)
    {
        if (data.journalImagesTaken.ContainsKey((int)Clue_Identification))
        {
            data.journalImagesTaken.Remove((int)Clue_Identification);
        }
        data.journalImagesTaken.Add((int)Clue_Identification, isActive);

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
        // adds the journal to the journalEntries(List)
        if (Clue_Identification != E_ClueInteraction.R2_INSTRUCTION1 && Clue_Identification != E_ClueInteraction.R2_INSTRUCTION2)
        {
            //Debug.LogError($"Load: {Clue_Identification}");
            FindObjectOfType<R2_Journal>().journalImages.Add(new JournalImage(Clue_Identification, clueImage));
        }

        // Deactivate this interactable clue
        isActive = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        // call the delegate of this clue
        if (D_Clue != null)
        {
            D_Clue(new C_Clue());
        }
    }
    
    // overridable function for save method
    public virtual void OSaveData(GameData data)
    {

    }
}
