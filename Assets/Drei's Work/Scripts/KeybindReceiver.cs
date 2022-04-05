using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindReceiver : MonoBehaviour
{

    private Dictionary<KeyBind, KeyCode> keybinds { get; set; }

    private string bindName;

    public KeyCode fwd = KeyCode.W,
        back = KeyCode.S,
        right = KeyCode.D,
        left = KeyCode.A,
        jump = KeyCode.Space,
        run = KeyCode.LeftShift,
        sneak = KeyCode.LeftControl,
        interact = KeyCode.E,
        cleanse = KeyCode.R;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<KeybindManager>() != null)
        {
            keybinds = KeybindManager.Instance.keybinds;
            fwd = KeybindManager.Instance.getKeyByAction(KeyBind.FORWARD);
            right = KeybindManager.Instance.getKeyByAction(KeyBind.RIGHT);
            left = KeybindManager.Instance.getKeyByAction(KeyBind.LEFT);
            run = KeybindManager.Instance.getKeyByAction(KeyBind.RUN);
            back = KeybindManager.Instance.getKeyByAction(KeyBind.BACK);
            jump = KeybindManager.Instance.getKeyByAction(KeyBind.JUMP);
            sneak = KeybindManager.Instance.getKeyByAction(KeyBind.SNEAK);
            interact = KeybindManager.Instance.getKeyByAction(KeyBind.INTERACT);
            cleanse = KeybindManager.Instance.getKeyByAction(KeyBind.CLEANSE);
        }
    }


    
}
