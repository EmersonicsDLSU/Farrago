using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActionManager : MonoBehaviour
{
    public static ButtonActionManager Instance;

    public Button interactButton;
    public Button jumpButton;
    public Button runButton;
    public Button sneakButton;
    public Button cleanseButton;
    public Button journalButton;

    private GameObject lastPressedButton;
    [HideInInspector] public bool isInteractHeldDown = false;
    [HideInInspector] public bool isInteractTapped = false;
    [HideInInspector] public bool isRunHeldDown = false;
    [HideInInspector] public bool isSneakHeldDown = false;
    [HideInInspector] public bool isJumpPressed = false;
    [HideInInspector] public bool isCleanseHeldDown = false;
    [HideInInspector] public bool isJournalPressed = false;

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
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    

    
}
