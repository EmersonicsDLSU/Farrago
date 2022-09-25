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
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject MainMenuGameText;
    #endregion

    [Header("Main Menu Buttons")]
    // Main Menu Buttons
    #region MM_Buttons_Ref
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnLoadGame;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnQuit;
    #endregion

    [Header("Save Panel Buttons")]
    // Save Panel Buttons
    #region SP_Buttons_Ref
    [SerializeField] private Button[] btnsSaveFile;
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnSave_Back;
    [SerializeField] private GameObject goSaveConfirmation;
    [SerializeField] private Button btnSaveConfirmYes;
    [SerializeField] private Button btnSaveConfirmNo;

    private SaveSlot selectedSaveFile;
    #endregion

    [Header("Load Panel Buttons")]
    // Load Panel Buttons
    #region LP_Buttons_Ref
    [SerializeField] private Button[] btnsLoadFile;
    [SerializeField] private Button btnLoadSave;
    [SerializeField] private Button btnLoad_Back;
    [SerializeField] private GameObject goLoadConfirmation;
    [SerializeField] private Button btnLoadConfirmYes;
    [SerializeField] private Button btnLoadConfirmNo;

    private SaveSlot selectedLoadFile;
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
        
        btnLoadGame.onClick.AddListener(disableAll);
        btnLoadGame.onClick.AddListener(on_Load);
        
        btnSettings.onClick.AddListener(disableAll);
        btnSettings.onClick.AddListener(on_Settings);

        btnQuit.onClick.AddListener(() => Application.Quit());
        #endregion
        
        // Save Panel Buttons
        #region LP_Buttons
        
        btnsSaveFile[0].onClick.AddListener(() => selectedSaveFile = btnsSaveFile[0].transform.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[0].onClick.AddListener(() => btnSave.interactable = true);
        btnsSaveFile[1].onClick.AddListener(() => selectedSaveFile = btnsSaveFile[1].transform.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[1].onClick.AddListener(() => btnSave.interactable = true);
        btnsSaveFile[2].onClick.AddListener(() => selectedSaveFile = btnsSaveFile[2].transform.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[2].onClick.AddListener(() => btnSave.interactable = true);


        btnSave.onClick.AddListener(() => goSaveConfirmation.SetActive(true));
        btnSave.onClick.AddListener(() => goSaveConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmEntry"));

        btnSaveConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnClearClicked(selectedSaveFile));
        btnSaveConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnSaveSlotClicked(selectedSaveFile));
        btnSaveConfirmNo.onClick.AddListener(() => goSaveConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));

        btnSave_Back.onClick.AddListener(on_Return);
        btnSave_Back.onClick.AddListener(ResetSettings);
        #endregion

        // Load Panel Buttons
        #region LP_Buttons

        btnsLoadFile[0].onClick.AddListener(() => selectedSaveFile = btnsLoadFile[0].transform.gameObject.GetComponent<SaveSlot>());
        btnsLoadFile[0].onClick.AddListener(() => btnLoadSave.interactable = true);
        btnsLoadFile[1].onClick.AddListener(() => selectedSaveFile = btnsLoadFile[1].transform.gameObject.GetComponent<SaveSlot>());
        btnsLoadFile[1].onClick.AddListener(() => btnLoadSave.interactable = true);
        btnsLoadFile[2].onClick.AddListener(() => selectedSaveFile = btnsLoadFile[2].transform.gameObject.GetComponent<SaveSlot>());
        btnsLoadFile[2].onClick.AddListener(() => btnLoadSave.interactable = true);
        
        btnLoadSave.onClick.AddListener(() => goLoadConfirmation.SetActive(true));
        btnLoadSave.onClick.AddListener(() => goLoadConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmEntry"));

        btnLoadConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnSaveSlotClicked(selectedSaveFile));
        btnLoadConfirmNo.onClick.AddListener(() => goLoadConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));

        btnLoad_Back.onClick.AddListener(on_Return);
        btnLoad_Back.onClick.AddListener(ResetSettings);
        #endregion
        // Setting Panel Buttons
        #region SP_Buttons
        btnSetting_Back.onClick.AddListener(on_Return);
        #endregion

   
        
    }
    
    void ResetSettings()
    {
        selectedLoadFile = null;
        selectedSaveFile = null;
    }

    void disableAll()
    {
        mainMenu.SetActive(false);
        MainMenuGameText.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void on_Load()
    {
        bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
        bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
        mmCameraAnimator.SetBool("LeftMenuTriggered", !isLeftMenuTriggered);
        mmCameraAnimator.SetBool("RightMenuTriggered", isRightMenuTriggered);

        for(int i = 0; i < btnsLoadFile.Length; i++)
        {
            btnsLoadFile[i].interactable = true;
        }

        loadPanel.SetActive(true);
        if (loadPanel.activeSelf != false)
        {
            Animator animator = loadPanel.GetComponent<Animator>();

            if (animator != null)
            {
             
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
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

        if (loadPanel.activeSelf != false)
        {
            bool isLeftMenuTriggered = mmCameraAnimator.GetBool("LeftMenuTriggered");
            bool isRightMenuTriggered = mmCameraAnimator.GetBool("RightMenuTriggered");
            mmCameraAnimator.SetBool("LeftMenuTriggered", !isLeftMenuTriggered);
            mmCameraAnimator.SetBool("RightMenuTriggered", isRightMenuTriggered);

            Animator animator = loadPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        Invoke("disableAll", 1);

        Invoke("activateMenu", 3.5f);
    }

    public void to_MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    void activateMenu()
    {
        mainMenu.SetActive(true);
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
