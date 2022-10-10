using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Controller : MonoBehaviour
{
    [Header("Main Menu Panels")]
    // Main Menu Panels
    #region MM_Panels
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject MainMenuGameText;
    #endregion

    [Header("Main Menu Buttons")]
    // Main Menu Buttons
    #region MM_Buttons_Ref
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnQuit;
    #endregion

    [Header("Settings Panel Buttons")]
    // Settings Panel Buttons
    #region LP_Buttons_Ref
    [SerializeField] private Button btnSetting_Back;
    #endregion

    [Header("Camera Animator")]
    [SerializeField] private Animator mmCameraAnimator;

    private DataPersistenceManager DPM;
    
    private void Start()
    {
        DPM = DataPersistenceManager.instance;

        Time.timeScale = 1;
        

        // Main Menu Buttons
        #region MM_Buttons
        btnNewGame.onClick.AddListener(disableAll);
        btnNewGame.onClick.AddListener(delegate()
            {
                savePanel.SetActive(true);
                if (savePanel.activeSelf != false)
                {
                    //set trigger for camera animator
                    if (mmCameraAnimator != null)
                    {
                        bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
                        bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
                        mmCameraAnimator.SetBool("LeftMenuTriggered", !isLeftMenuTriggered);
                        mmCameraAnimator.SetBool("RightMenuTriggered", isRightMenuTriggered);
                    }

                    Animator animator = savePanel.GetComponent<Animator>();

                    if (animator != null)
                    {
                        bool isOpen = animator.GetBool("Open");

                        animator.SetBool("Open", !isOpen);
                    }
                }
            }
        );

        btnSettings.onClick.AddListener(disableAll);
        btnSettings.onClick.AddListener(on_Settings);

        btnQuit.onClick.AddListener(() => Application.Quit());
        #endregion

        // Setting Panel Buttons
        #region SP_Buttons
        btnSetting_Back.onClick.AddListener(on_Return);
        #endregion

   
        
    }

    void disableAll()
    {
        mainMenu.SetActive(false);
        MainMenuGameText.SetActive(false);
        savePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void on_Settings()
    {
        bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
        bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
        mmCameraAnimator.SetBool("LeftMenuTriggered", isLeftMenuTriggered);
        mmCameraAnimator.SetBool("RightMenuTriggered", !isRightMenuTriggered);

        settingsPanel.SetActive(true);

        if (settingsPanel.activeSelf != false)
        {
            Animator animator = settingsPanel.GetComponent<Animator>();

            if(animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);

            }
        }
    }
    

    public void on_Return()
    {
        if (KeybindManager.Instance.waitingForInput)
        {
            return;
        }

        if (settingsPanel.activeSelf != false)
        {
            bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
            bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
            mmCameraAnimator.SetBool("LeftMenuTriggered", isLeftMenuTriggered);
            mmCameraAnimator.SetBool("RightMenuTriggered", !isRightMenuTriggered);

            Animator animator = settingsPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
        
        if (savePanel.activeSelf != false)
        {
            bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
            bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
            mmCameraAnimator.SetBool("LeftMenuTriggered", !isLeftMenuTriggered);
            mmCameraAnimator.SetBool("RightMenuTriggered", isRightMenuTriggered);

            Animator animator = savePanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        Invoke("disableAll", 1);

        Invoke("activateMenu", 1.5f);
    }

    public void to_MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    void activateMenu()
    {
        mainMenu.SetActive(true);

        btnNewGame.GetComponent<Animator>().SetTrigger("Normal");
        btnSettings.GetComponent<Animator>().SetTrigger("Normal");
        btnQuit.GetComponent<Animator>().SetTrigger("Normal");

        MainMenuGameText.SetActive(true);
    }
    

    private void Update()
    {
        /*
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                Debug.Log(vKey.ToString());
            }
        }
        */
    }

}
