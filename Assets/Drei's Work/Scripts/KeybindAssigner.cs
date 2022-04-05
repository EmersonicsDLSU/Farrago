using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindAssigner : MonoBehaviour
{
    private Event keyEvent;

    private bool waitingForInput = false;
    private KeyCode newKey;
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void startAssigningKey(KeyBind keyName)
    {
        if (waitingForInput == false)
        {
            StartCoroutine(AssignKey(keyName));
        }
    }

    public void assignText(Text text)
    {
        buttonText = text;
    }

    IEnumerator waitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(KeyBind keyName)
    {
        waitingForInput = true;

        yield return waitForKey();

        KeybindManager.Instance.BindKey(keyName, newKey);
        buttonText.text = KeybindManager.Instance.keybinds[keyName].ToString();

        yield return null;
    }
}
