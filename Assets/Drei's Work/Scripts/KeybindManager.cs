using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public enum KeyBind
{
    FORWARD = 0,
    BACK,
    LEFT,
    RIGHT,
    RUN,
    JUMP,
    SNEAK,
    INTERACT,
    CLEANSE
};

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    private Event keyEvent;

    public Dictionary<KeyBind, KeyCode> keybinds { get; set; }
    private string bindName;

    public bool waitingForInput = false;
    private KeyCode newKey;
    public Text buttonText;


    private void Awake()
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
        keybinds = new Dictionary<KeyBind, KeyCode>();

        //ASSIGNING INITIAL KEYBINDS
        BindKey(KeyBind.FORWARD, KeyCode.W);
        BindKey(KeyBind.BACK, KeyCode.S);
        BindKey(KeyBind.LEFT, KeyCode.A);
        BindKey(KeyBind.RIGHT, KeyCode.D);
        BindKey(KeyBind.JUMP, KeyCode.Space);
        BindKey(KeyBind.SNEAK, KeyCode.LeftControl);
        BindKey(KeyBind.RUN, KeyCode.LeftShift);
        BindKey(KeyBind.INTERACT, KeyCode.E);
        BindKey(KeyBind.CLEANSE, KeyCode.R);
    }

    public void BindKey(KeyBind key, KeyCode keyBind)
    {
        Dictionary<KeyBind, KeyCode> currentDict = keybinds;

        if (!currentDict.ContainsValue(keyBind))
        {
            if (!currentDict.ContainsKey(key))
            {
                currentDict.Add(key, keyBind);
            }
        }
        else if (currentDict.ContainsValue(keyBind))
        {
            Debug.Log("HERE");

            KeyBind myKey = currentDict.First(x => x.Value == keyBind).Key;

            currentDict[myKey] = KeyCode.None;

        }

        currentDict[key] = keyBind;
        bindName = string.Empty;
        
    }

    public KeyCode getKeyByAction(KeyBind action)
    {
        return keybinds[action];
    }

    void OnGUI()
    {
        //checks and stores the current event in the scene
        keyEvent = Event.current;

        //check if the event is a key event and if a keybind button(inWaiting) is active
        if (keyEvent.isKey && waitingForInput)
        {
            //assigns the last keyCode type, excluding "None" values
            if(keyEvent.keyCode != KeyCode.None)
            {
                lastKeyCode = keyEvent.keyCode;
            }

            Debug.LogError($"lastKeyCode!! {lastKeyCode}");

            //checks if the last key code already doesn't exist in the bindManagerDictionary
            if (!keybinds.ContainsValue(lastKeyCode))
            {
                invalid = false;
            }
            //if it has a duplicate, waits for the user to enter a unique key again
            else
            {
                invalid = true;
                return;
            }
            //assigns the new valid key
            newKey = keyEvent.keyCode;

            Debug.LogError($"CLICKED!! {lastKeyCode.ToString()}");

            waitingForInput = false;
        }
    }

    public void startAssigningKey()
    {
        Debug.LogError($"Button Clicked {UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.name}");

        //gets the last button clicked throught getting its last char int number
        string sample = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.name;

        //converting int to KeyBind enum
        KeyBind key = (KeyBind)Convert.ToInt32(sample.Substring(sample.Length - 1));

        //deletes the recent keybind
        keybinds[key] = KeyCode.None;

        if (waitingForInput == false)
        {
            StartCoroutine(AssignKey(key));
        }
    }

    public void assignText(Text text)
    {
        buttonText = text;
    }

    IEnumerator waitForKey()
    {
        buttonText.text = "Waiting...";
        //check if still no key pressed recently and if there's duplicate in the keybinds
        //button keeps on waiting for the new and valid keybind
        while (invalid || !keyEvent.isKey)
        {
            assignText(buttonText);
            yield return null;
        }
    }

    public IEnumerator AssignKey(KeyBind keyName)
    {
        waitingForInput = true;

        yield return waitForKey();
        
        BindKey(keyName, newKey);
        buttonText.text = keybinds[keyName].ToString();
  
        
        yield return null;
    }

    private bool invalid = true;
    private KeyCode lastKeyCode;
    
    void Update()
    {

    }
}
