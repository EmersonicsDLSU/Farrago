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
    
    [SerializeField] private Image clueImage;
    public GameObject clueUIText;
    public GameObject interactText;
    public GameObject journalHelpButton;

    private string key;
    private Vector2 imageInitPos;
    private Object_ID object_ID;

    private void Awake()
    {
        imageInitPos = clueImage.rectTransform.anchoredPosition;
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
    protected void CallItemEvents(E_ClueInteraction item)
    {
        // adds the journal to the journalEntries(List)
        key = "J" + Journal.Instance.journalEntries.Count.ToString();
        Journal.Instance.journalEntries.Add(key, clueImage);
        Journal.Instance.journalEntries[key].enabled = true;

        // Deactivate this interactable clue
        isActive = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        // position the Image UI at the center
        clueImage.rectTransform.anchoredPosition = new Vector2(0,0);

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
        clueImage.rectTransform.anchoredPosition = imageInitPos;
        if(Journal.Instance.journalEntries.ContainsKey(key))
            Journal.Instance.journalEntries[key].enabled = false;

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
                Debug.LogError($"Load: {Clue_Identification}");
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
        key = "J" + Journal.Instance.journalEntries.Count.ToString();
        Journal.Instance.journalEntries.Add(key, clueImage);

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
