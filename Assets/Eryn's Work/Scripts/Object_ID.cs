using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objectCode
{
    redLines = 0,
    blueLines,
    yellowLines,
    orangeLines,
    violetLines,
    whiteLines,
    blackLines,
    blocker,
    JOURNAL,
    NOTE
};

public class Object_ID : MonoBehaviour
{
    [HideInInspector] public string objectName;

    public objectCode objectCode;

    [TextArea]
    public string[] Texts;

    void Awake()
    {
        objectName = this.gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TextControl.textInstance.Interact((TextControl.textType)System.Enum.Parse(typeof(TextControl.textType), objectCode.ToString()));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextControl.textInstance.clearQueue();
        }
    }

    private void OnValidate()
    {
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }
}
