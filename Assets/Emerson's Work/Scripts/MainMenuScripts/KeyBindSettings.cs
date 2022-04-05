using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindSettings : MonoBehaviour
{
    public List<Button> KeyBindButtons= new List<Button>();

    private Button sample;
    /*
    /*This is the order of the text in the List
public enum KeyBind
{
    FORWARD = 0,
    BACK,
    LEFT,
    RIGHT,
    JUMP,
    SNEAK,
    INTERACT,
    CLEANSE
};
    */

    private KeybindManager KBM;

    // Start is called before the first frame updates
    void Start()
    {
        KBM = FindObjectOfType<KeybindManager>();
    }

    private List<string> listOfKB = new List<string>();

    //everytime the loader goes back to this scene, this will automatically updates
    //executes only if the gameobject is active
    int index = 0;
    void OnEnable()
    {
        index = 0;
        foreach (KeyBind key in Enum.GetValues(typeof(KeyBind)))
        {
            KeyBindButtons[index++].GetComponentInChildren<Text>().text = KeybindManager.Instance.keybinds[key].ToString();
        }
        
        //assign the new keybinds by order
        foreach (KeyBind key in Enum.GetValues(typeof(KeyBind)))
        {
            listOfKB.Add(key.ToString());
        }

        //resets the listeners in the button and reassigns the default functions
        index = 0;
        foreach (var button in KeyBindButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { KBM.assignText(button.GetComponentInChildren<Text>()); });
            button.onClick.AddListener(() => { KBM.startAssigningKey(); });
            index++;
        }
        
    }
}
