using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JournalBookInteraction : MonoBehaviour
{
    [Space] [Header("Interactables")] public GameObject interactableParent;

    [SerializeField] private GameObject journalHUDText;
    private GameObject journalObtainHelp;

    private Journal playerJournal;
    private bool isEPressed = false;
    private bool isPlayerInJournalArea = false;
    public Image journalFirstPage;
    public Image journalSecondPage;
    private string key;
    private Object_ID object_ID;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogError("COLLIDED");

            if (playerJournal.isJournalObtained == false)
            {
                interactableParent.SetActive(true);
                this.isPlayerInJournalArea = true;
            }
            

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.isPlayerInJournalArea = true;

            if (this.isEPressed == true && playerJournal.isJournalObtained == false)
            {
                playerJournal.isJournalObtained = true;

                TextControl.textInstance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
                TextControl.textInstance.delayReset();

                //display UI of journal
                journalHUDText.SetActive(true);

                //add animation here for angela's monologue


                //add here the first 2 pages of the journal
                key = "J" + playerJournal.journalEntries.Count.ToString();
                playerJournal.journalEntries.Add(key, journalFirstPage);
                key = "J" + playerJournal.journalEntries.Count.ToString();
                playerJournal.journalEntries.Add(key, journalSecondPage);

                this.isEPressed = false;
                this.gameObject.SetActive(false);

                //journalObtainHelp.GetComponent<Animator>().SetBool("toTriggerHelp", false);
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.interactableParent.SetActive(false);
            this.isPlayerInJournalArea = false;
            this.isEPressed = false;
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        if (Journal.Instance.isJournalObtained == false)
        {
            journalHUDText.SetActive(false);
        }
        else
        {
            journalHUDText.SetActive(true);
        }
        
        playerJournal = Journal.Instance;
        journalObtainHelp = GameObject.Find("TooltipText");
        object_ID = GetComponent<Object_ID>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEPressed == false && isPlayerInJournalArea == true)
        {
            this.isEPressed = true;
        }

        /*
        if (playerJournal.isJournalObtained == true)
        {
            journalHUDText.SetActive(true);
        }
        */
    }

    public void resetJournalStatusOnQuit()
    {
        Journal.Instance.isJournalObtained = false;
        Journal.Instance.journalEntries.Clear();
    }
}
