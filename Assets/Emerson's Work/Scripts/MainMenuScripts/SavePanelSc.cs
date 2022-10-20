using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePanelSc : MonoBehaviour
{
    [Header("Save Panel Buttons")]
    // Save Panel Buttons
    #region SP_Buttons_Ref
    [SerializeField] private Button[] btnsSaveFile;
    public Button[] btnsDeleteFile;
    [SerializeField] private Button btnSave_Back;
    [SerializeField] private GameObject goSaveConfirmation;
    [SerializeField] private GameObject goLoadConfirmation;
    [SerializeField] private GameObject goDeleteConfirmation;
    [SerializeField] private Button btnSaveConfirmYes;
    [SerializeField] private Button btnSaveConfirmNo;
    [SerializeField] private Button btnLoadConfirmYes;
    [SerializeField] private Button btnLoadConfirmNo;
    [SerializeField] private Button btnDeleteConfirmYes;
    [SerializeField] private Button btnDeleteConfirmNo;
    private SaveSlot selectedFile;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        // Save Panel Buttons
        #region LP_Buttons
        
        // when saveSlots are clicked, then show the confirmation panel
        btnsSaveFile[0].onClick.AddListener(() => selectedFile = btnsSaveFile[0].transform.parent.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[0].onClick.AddListener(() => ShowConfirmation(selectedFile));
        btnsSaveFile[1].onClick.AddListener(() => selectedFile = btnsSaveFile[1].transform.parent.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[1].onClick.AddListener(() => ShowConfirmation(selectedFile));
        btnsSaveFile[2].onClick.AddListener(() => selectedFile = btnsSaveFile[2].transform.parent.gameObject.GetComponent<SaveSlot>());
        btnsSaveFile[2].onClick.AddListener(() => ShowConfirmation(selectedFile));

        // button for deleting the file
        btnsDeleteFile[0].onClick.AddListener(() => ShowConfirmation(btnsSaveFile[0].transform.parent.gameObject.GetComponent<SaveSlot>(), true));
        btnsDeleteFile[1].onClick.AddListener(() => ShowConfirmation(btnsSaveFile[1].transform.parent.gameObject.GetComponent<SaveSlot>(), true));
        btnsDeleteFile[2].onClick.AddListener(() => ShowConfirmation(btnsSaveFile[2].transform.parent.gameObject.GetComponent<SaveSlot>(), true));

        // overwrite save confirmation options
        btnSaveConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnSaveSlot(selectedFile));
        btnSaveConfirmNo.onClick.AddListener(() => goSaveConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));
        
        // load file confirmation options
        btnLoadConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnLoadSlot(selectedFile));
        btnLoadConfirmNo.onClick.AddListener(() => goLoadConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));
        
        // delete file confirmation options
        btnDeleteConfirmYes.onClick.AddListener(() => FindObjectOfType<SaveSlotsMenu>().OnDeleteFileSlot(selectedFile));
        btnDeleteConfirmYes.onClick.AddListener(() => goDeleteConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));
        btnDeleteConfirmNo.onClick.AddListener(() => goDeleteConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmExit"));

        // return option
        btnSave_Back.onClick.AddListener(FindObjectOfType<MainMenu_Controller>().on_Return);
        btnSave_Back.onClick.AddListener(ResetSettings);
        #endregion
    }

    public void ShowConfirmation(SaveSlot saveSlot, bool isDelete = false)
    {
        if (isDelete)
        {
            goDeleteConfirmation.SetActive(true);
            goDeleteConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmEntry");
            selectedFile = saveSlot;
            return;
        }
        // we ask the player if he/she will load the game
        if (FindObjectOfType<SaveSlotsMenu>().HasAFile(selectedFile))
        {
            goLoadConfirmation.SetActive(true);
            goLoadConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmEntry");
        }
        else
        {
            goSaveConfirmation.SetActive(true);
            goSaveConfirmation.GetComponent<Animator>().SetTrigger("SaveConfirmEntry");
        }
    }
    
    void ResetSettings()
    {
        selectedFile = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
