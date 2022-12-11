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
    public GameObject gameOverPanel;
    public GameObject confirmationPanelLose;
    public GameObject journalPanel;
    private TooltipHolder tooltipHolder;
    
    private bool isEscPressed = false;
    [HideInInspector] public bool isJPressed = false;
    private bool canPress = true;
    
    PlayerSFX_Manager playerSFX;
    
    public QuestGiver questGiver; 

    private void Awake()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        playerSFX = PlayerSFX_Manager.Instance;
        //tooltipHolder = GameObject.Find("TooltipHolder").GetComponent<TooltipHolder>();
    }


    private void Update()
    {
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
        //JOURNAL MENU
        else if (Input.GetKeyDown(KeyCode.J) && canPress && FindObjectOfType<R2_Journal>().isJournalObtained == true)
        {
            canPress = false;
            if (isJPressed == false)
            {
                FindObjectOfType<JournalBook>().On_OpenJournal();
                isJPressed = true;
            }
            else
            {
                FindObjectOfType<JournalBook>().On_CloseJournal();
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
        FindObjectOfType<JournalBook>().displayClueImage.enabled = false;
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
    

    public void On_OpenJournal()
    {
        Invoke("time_Pause", 1.0f);
    }
    public void On_CloseJournal()
    {
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


}
