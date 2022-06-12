using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Time = UnityEngine.Time;

public class ClueInteraction : MonoBehaviour
{
    [SerializeField] private Image clueImage;

    [Space] [Header("Interactables")] public GameObject interactableParent;
    public Image interactableFill;

    private Journal playerJournal;

    [SerializeField] private bool isEPressed = false;
    [SerializeField] private bool isClueObtained = false;
    [SerializeField] private bool isPlayerInClueArea = false;

    public GameObject clueUIText;
    public GameObject interactText;
    public GameObject journalHelpButton;

    private string key;
    private Vector2 imageInitPos;
    private bool isFirstInteract = true;
    private Object_ID object_ID;

    private void Awake()
    {
        playerJournal = Journal.Instance;
        imageInitPos = clueImage.rectTransform.anchoredPosition;
        object_ID = GetComponent<Object_ID>();
    }

private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.LogError("COLLIDED");

            if (this.isClueObtained == false)
            {
                this.interactableParent.SetActive(true);
                this.isPlayerInClueArea = true;
            }
                

        }
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isPlayerInClueArea = true;

            if (this.isEPressed == true && this.isClueObtained == false)
            {
                clueImage.rectTransform.anchoredPosition = new Vector2(0,0);
                
                Debug.LogWarning("GOOD");


                key = "J" + playerJournal.journalEntries.Count.ToString();
                playerJournal.journalEntries.Add(key, clueImage);

                playerJournal.journalEntries[key].enabled = true;


                this.isEPressed = false;
                this.isClueObtained = true;
                interactText.GetComponent<Text>().text = "Close";

            }

            else if (this.isEPressed == true && this.isClueObtained == true && playerJournal.journalEntries[key].isActiveAndEnabled == true)
            {

                clueImage.rectTransform.anchoredPosition = imageInitPos;

                interactText.GetComponent<Text>().text = "Absorb/Interact";

                playerJournal.journalEntries[key].enabled = false;
                this.interactableParent.SetActive(false);

                this.isEPressed = false;

                TextControl.textInstance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
                TextControl.textInstance.delayReset();

                clueUIText.GetComponent<Animator>().SetBool("isClueObtained", true);
                journalHelpButton.GetComponent<Animator>().SetBool("isClueObtained", true);
                Invoke("closeUIText", 2.0f);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
         

            isPlayerInClueArea = false;
            clueImage.rectTransform.anchoredPosition = imageInitPos;

            //interactText.GetComponent<Text>().text = "Absorb/Interact";

            Debug.LogError("EXITED");
            this.interactableParent.SetActive(false);
            if(playerJournal.journalEntries.ContainsKey(key))
                playerJournal.journalEntries[key].enabled = false;
            this.isEPressed = false;

            if (isClueObtained == true && isFirstInteract == true)
            {
                clueUIText.GetComponent<Animator>().SetBool("isClueObtained", true);
                journalHelpButton.GetComponent<Animator>().SetBool("isClueObtained", true);
                Invoke("closeUIText", 2.0f);
                isFirstInteract = false;

                TextControl.textInstance.setText(object_ID.Texts[Random.Range(0, object_ID.Texts.Length - 1)]);
                TextControl.textInstance.delayReset();
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isEPressed == false && isPlayerInClueArea == true)
        {
            this.isEPressed = true;
        }
       
    }

    private void closeUIText()
    {
        clueUIText.GetComponent<Animator>().SetBool("isClueObtained", false);
        journalHelpButton.GetComponent<Animator>().SetBool("isClueObtained", false);
    }
}
