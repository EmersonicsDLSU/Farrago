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
    private SaveFile selectedSaveFile;
    #endregion
    
    [Header("Load Panel Buttons")]
    // Load Panel Buttons
    #region LP_Buttons_Ref
    [SerializeField] private Button[] btnsLoadFile; 
    [SerializeField] private Button btnLoadSave; 
    [SerializeField] private Button btnLoad_Back; 
    private SaveFile selectedLoadFile;
    #endregion
    
    private void Start()
    {
        Time.timeScale = 1;

        // Main Menu Buttons
        #region MM_Buttons
        btnNewGame.onClick.AddListener(disableAll);
        btnNewGame.onClick.AddListener(delegate()
            {
                savePanel.SetActive(true);
                if (savePanel.activeSelf != false)
                {
                    Animator animator = savePanel.GetComponent<Animator>();

                    if (animator != null)
                    {
                        bool isOpen = animator.GetBool("Open");

                        animator.SetBool("Open", !isOpen);
                    }
                }
            }
        );
        btnNewGame.onClick.AddListener(CheckSaveFiles);
        
        btnLoadGame.onClick.AddListener(disableAll);
        btnLoadGame.onClick.AddListener(on_Load);
        btnLoadGame.onClick.AddListener(CheckLoadFiles);
        
        btnSettings.onClick.AddListener(disableAll);
        btnSettings.onClick.AddListener(on_Settings);

        btnQuit.onClick.AddListener(() => Application.Quit());
        #endregion
        
        // Save Panel Buttons
        #region LP_Buttons
        btnsSaveFile[0].onClick.AddListener(() => selectedSaveFile = SaveFile.FILE_0);
        btnsSaveFile[0].onClick.AddListener(() => btnSave.interactable = true);
        btnsSaveFile[1].onClick.AddListener(() => selectedSaveFile = SaveFile.FILE_1);
        btnsSaveFile[1].onClick.AddListener(() => btnSave.interactable = true);
        btnsSaveFile[2].onClick.AddListener(() => selectedSaveFile = SaveFile.FILE_2);
        btnsSaveFile[2].onClick.AddListener(() => btnSave.interactable = true);
        
        // TODO: Add a confirmation window before overwriting the saved file
        btnSave.onClick.AddListener(delegate()
            {
                if (selectedSaveFile != SaveFile.NONE)
                {
                    DataPersistenceManager.instance.DeleteFile(selectedSaveFile);
                    DataPersistenceManager.instance.NewGame(selectedSaveFile);
                }
            }
        );
        btnSave.onClick.AddListener(() => Loader.loadinstance.LoadLevel(1));

        btnSave_Back.onClick.AddListener(on_Return);
        btnSave_Back.onClick.AddListener(ResetSettings);
        #endregion

        // Load Panel Buttons
        #region LP_Buttons
        btnsLoadFile[0].onClick.AddListener(() => selectedLoadFile = SaveFile.FILE_0);
        btnsLoadFile[0].onClick.AddListener(() => btnLoadSave.interactable = true);
        btnsLoadFile[1].onClick.AddListener(() => selectedLoadFile = SaveFile.FILE_1);
        btnsLoadFile[1].onClick.AddListener(() => btnLoadSave.interactable = true);
        btnsLoadFile[2].onClick.AddListener(() => selectedLoadFile = SaveFile.FILE_2);
        btnsLoadFile[2].onClick.AddListener(() => btnLoadSave.interactable = true);
        
        btnLoadSave.onClick.AddListener(() => DataPersistenceManager.instance.currentSaveFile = selectedLoadFile);
        btnLoadSave.onClick.AddListener(() => Loader.loadinstance.LoadLevel(1));
        
        btnLoad_Back.onClick.AddListener(on_Return);
        btnLoad_Back.onClick.AddListener(ResetSettings);
        #endregion
    }

    void ResetSettings()
    {
        selectedLoadFile = SaveFile.NONE;
        selectedSaveFile = SaveFile.NONE;
    }
    
    void CheckSaveFiles()
    {
        var isAFileSelected = (selectedSaveFile != SaveFile.NONE) ? true : false;
        btnSave.interactable = false;
    }

    void CheckLoadFiles()
    {
        var isAFileSelected = (selectedLoadFile != SaveFile.NONE) ? true : false;
            btnLoadSave.interactable = false;
        // Set the interactable status of the Load SaveFiles Buttons
        btnsLoadFile[0].interactable = DataPersistenceManager.instance.
            CheckIfSaveFileExist(SaveFile.FILE_0);
        btnsLoadFile[1].interactable = DataPersistenceManager.instance.
            CheckIfSaveFileExist(SaveFile.FILE_1);
        btnsLoadFile[2].interactable = DataPersistenceManager.instance.
            CheckIfSaveFileExist(SaveFile.FILE_2);
    }

    void disableAll()
    {
        mainMenu.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void on_Load()
    {
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
            Animator animator = settingsPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
        
        if (savePanel.activeSelf != false)
        {
            Animator animator = savePanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        if (loadPanel.activeSelf != false)
        {
            Animator animator = loadPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        Invoke("disableAll", 1);

        Invoke("activateMenu", 1.01f);
    }

    public void to_MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void activateMenu()
    {
        mainMenu.SetActive(true);
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
