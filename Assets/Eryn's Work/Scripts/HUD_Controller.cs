using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    [SerializeField]
    public GameObject HUD;
    public GameObject pausePanel;
    public GameObject confirmationPanel;
    public GameObject objectivesPanel;
    public GameObject gameOverPanel;
    public GameObject confirmationPanelLose;
    public GameObject journalPanel;
    private TooltipHolder tooltipHolder;

    public Button nextJournalButton;
    public Button prevJournalButton;
    

    private bool isEscPressed = false;
    private bool isJPressed = false;
    private bool canPress = true;
    private bool isNextPagePressed = false;
    private bool isPrevPagePressed = false;

    [HideInInspector] public bool isObjectivesOpen = false;
   

    private bool objectivesEnabled;

    [HideInInspector] public int curr_JournalIndex = 0;

    PlayerSFX_Manager playerSFX;

    private Journal playerJournal;
    public QuestGiver questGiver; 

    private void Awake()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        playerJournal = Journal.Instance;
        playerSFX = PlayerSFX_Manager.Instance;
        //tooltipHolder = GameObject.Find("TooltipHolder").GetComponent<TooltipHolder>();
    }


    private void Update()
    {
        //DISABLE OBJECTIVES PANEL IF NOT IN MISSION
        if (questGiver.isInQuest)
        {
            objectivesPanel.SetActive(true);
        }
        else
        {
            objectivesPanel.SetActive(false);
        }

        //PAUSE MENU
        if (Input.GetKeyDown(KeyCode.Escape) && canPress && isJPressed == false)
        {
            canPress = false;
            if (isEscPressed == false)
            {
                On_Pause();
                isEscPressed = true;
            }
            else
            {
                On_unPause();
                isEscPressed = false;
            }
        }
        //OBJECTIVES UI
        else if (Input.GetKeyDown(KeyCode.Tab) && canPress && questGiver.isInQuest)
        {
            canPress = false;
            On_ClickObjectives();
        }
        //JOURNAL MENU
        else if (ButtonActionManager.Instance.isJournalPressed && canPress && playerJournal.isJournalObtained == true)
        {
            canPress = false;
            if (isJPressed == false)
            {
                On_OpenJournal();
                isJPressed = true;
            }
            else
            {
                On_CloseJournal();
                isJPressed = false;
            }
        }
    }

    public void disable_All()
    {
        HUD.SetActive(false);
        pausePanel.SetActive(false); 
        confirmationPanel.SetActive(false);
        journalPanel.SetActive(false);
        //objectivesPanel.SetActive(false);
    }
    public void enable_HUD()
    {
        HUD.SetActive(true);
        //objectivesPanel.SetActive(true);
        canPress = true;
    }

    public void time_Pause()
    {
        Time.timeScale = 0;
        canPress = true;
    }

    public void On_Pause()
    {
        

        disable_All();
        pausePanel.SetActive(true);
        if (pausePanel.activeSelf != false)
        {
            Animator animator = pausePanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }

            

        }

        Invoke("time_Pause", 1.0f);
    }

    public void On_unPause()
    {
        Time.timeScale = 1;
        if (pausePanel.activeSelf != false)
        {
            Animator animator = pausePanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }

            
            
        }

        Invoke("disable_All", 1.0f);
        Invoke("enable_HUD", 1.01f);
    }

    public void On_Quit()
    {
        Time.timeScale = 1;
        confirmationPanel.SetActive(true);
    }

    public void On_No()
    {
        confirmationPanel.SetActive(false);
    }

    public void On_ClickObjectives()
    {
        //objectivesPanel.SetActive(true);
        isObjectivesOpen = !isObjectivesOpen;
        canPress = true;
        

        objectivesEnabled = objectivesPanel.GetComponent<Animator>().GetBool("isEnabled");
        Debug.LogWarning("working1");
        objectivesPanel.GetComponent<Animator>().SetBool("isEnabled", !objectivesEnabled);
    }

    public void On_OpenJournal()
    {
        //turning off the journal flash anim
        GameObject.Find("JournalHelp").GetComponent<Animator>().SetBool("isClueObtained", false);

        playerSFX.findSFXSourceByLabel("Journal").PlayOneShot(playerSFX.findSFXSourceByLabel("Journal").clip);

        disable_All();
        journalPanel.SetActive(true);
        if (journalPanel.activeSelf != false)
        {
            Animator animator = journalPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        //Invoke("time_Pause", 1.0f);

        Invoke("displayJournalPics", 1.0f);

    }

    public void displayJournalPics()
    {

        string key1 = "J" + curr_JournalIndex.ToString();
        string key2 = "J" + (curr_JournalIndex + 1).ToString();
        //displaying first items
        if (playerJournal.journalEntries.ContainsKey(key1))
        {
            playerJournal.journalEntries[key1].enabled = true;
        }

        if (playerJournal.journalEntries.ContainsKey(key2))
        {
            playerJournal.journalEntries[key2].enabled = true;
        }
           
        /*
        if (playerJournal.journalEntries.Count > 0)
        {
           
        }
        */
    }

    public void On_CloseJournal()
    {
        isJPressed = false;
        ButtonActionManager.Instance.isJournalPressed = false;

        //Time.timeScale = 1;
        if (journalPanel.activeSelf != false)
        {
            Animator animator = journalPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }

            foreach (var image in playerJournal.journalEntries)
            {
                image.Value.enabled = false;
            }

        }

        playerSFX.findSFXSourceByLabel("Journal").PlayOneShot(playerSFX.findSFXSourceByLabel("Journal").clip);

        Invoke("disable_All", 1.0f);
        Invoke("enable_HUD", 1.01f);
    }

    public void On_Lose()
    {
        Time.timeScale = 0;
        disable_All();
        gameOverPanel.SetActive(true);
    }

    public void On_QuitLose()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        confirmationPanelLose.SetActive(true);
    }

    public void On_TriggerNextPageJournal()
    {
        string key1 = "J" + curr_JournalIndex.ToString();
        string key2 = "J" + (curr_JournalIndex + 1).ToString();

        Debug.LogWarning("Initial:" + curr_JournalIndex);

        playerSFX.findSFXSourceByLabel("Journal").PlayOneShot(playerSFX.findSFXSourceByLabel("Journal").clip);

        //disable current pics
        if (playerJournal.journalEntries.ContainsKey(key1))
            playerJournal.journalEntries[key1].enabled = false;
        //check if next pic exists
        if (playerJournal.journalEntries.ContainsKey(key2))
        {
            //if next pic exists, disable
            playerJournal.journalEntries[key2].enabled = false;
        }

        //checks if the next page returns back to the first page
        if ((this.curr_JournalIndex+=2) >= playerJournal.journalEntries.Count)
        {
            Debug.LogWarning("entered");
            this.curr_JournalIndex = 0;
        }


        string nextkey1 = "J" + (curr_JournalIndex).ToString();
        string nextkey2 = "J" + (curr_JournalIndex + 1).ToString();

        Debug.LogWarning("After condition:" + curr_JournalIndex);

        //enable next pic
        if (playerJournal.journalEntries.ContainsKey(nextkey1))
            playerJournal.journalEntries[nextkey1].enabled = true;
        //if next pic exists
        if (playerJournal.journalEntries.ContainsKey(nextkey2))
        {
            playerJournal.journalEntries[nextkey2].enabled = true;
        }
        
       
    }

    public void On_TriggerPrevPageJournal()
    {
        string key1 = "J" + curr_JournalIndex.ToString();
        string key2 = "J" + (curr_JournalIndex + 1).ToString();

        playerSFX.findSFXSourceByLabel("Journal").PlayOneShot(playerSFX.findSFXSourceByLabel("Journal").clip);

        //disable current pics
        if (playerJournal.journalEntries.ContainsKey(key1))
            playerJournal.journalEntries[key1].enabled = false;
        if (playerJournal.journalEntries.ContainsKey(key2))
        {
            playerJournal.journalEntries[key2].enabled = false;
        }
        

        //NEED TO FIX
        if ((this.curr_JournalIndex -= 2) < 0)
        {
            this.curr_JournalIndex = playerJournal.journalEntries.Count - 2; //1, 2
        }

        string nextkey1 = "J" + (curr_JournalIndex).ToString(); //1 default
        string nextkey2 = "J" + (curr_JournalIndex + 1).ToString(); //1+1 = 2 bunsen

        //enable next pic
        if (playerJournal.journalEntries.ContainsKey(nextkey1))
        {
            playerJournal.journalEntries[nextkey1].enabled = true;
        }
        if (playerJournal.journalEntries.ContainsKey(nextkey2))
        {
            playerJournal.journalEntries[nextkey2].enabled = true;
        }
        

    }
}
