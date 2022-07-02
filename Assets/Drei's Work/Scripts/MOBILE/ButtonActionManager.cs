using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActionManager : MonoBehaviour
{
    public static ButtonActionManager Instance;

    [HideInInspector] public Button interactButton;
    [HideInInspector] public Button jumpButton;
    [HideInInspector] public Button runButton;
    [HideInInspector] public Button sneakButton;
    [HideInInspector] public Button cleanseButton;
    [HideInInspector] public Button journalButton;
    [HideInInspector] public Button pauseButton;

    private GameObject lastPressedButton;
    [HideInInspector] public bool isInteractHeldDown = false;
    [HideInInspector] public bool isInteractTapped = false;
    [HideInInspector] public bool isRunHeldDown = false;
    [HideInInspector] public bool isSneakHeldDown = false;
    [HideInInspector] public bool isJumpPressed = false;
    [HideInInspector] public bool isCleanseHeldDown = false;
    [HideInInspector] public bool isJournalPressed = false;
    [HideInInspector] public bool isPausePressed = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        interactButton = GameObject.Find("AbsorbIntHelp").GetComponent<Button>();
        jumpButton = GameObject.Find("JumpHelp").GetComponent<Button>();
        runButton = GameObject.Find("RunHelp").GetComponent<Button>();
        sneakButton = GameObject.Find("CrouchHelp").GetComponent<Button>();
        cleanseButton = GameObject.Find("CleanseHelp").GetComponent<Button>();
        journalButton = GameObject.Find("JournalHelp").GetComponent<Button>();
        pauseButton = GameObject.Find("PauseButton (1)").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    

    
}
