using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonologueHolder : MonoBehaviour
{
    private Vector2 initTextBoxPos;

    // Start is called before the first frame update
    void Start()
    {
        initTextBoxPos = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void triggerMonologueRoom2Check()
    {
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(150, initTextBoxPos.y);
        this.gameObject.GetComponent<Text>().text = "Sample monologue text here";
        
        this.gameObject.GetComponent<Animator>().SetBool("toTriggerMonologue", true);
    }
}
