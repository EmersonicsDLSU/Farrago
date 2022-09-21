using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalBook : MonoBehaviour
{
    private int curr_JournalIndex = 0;
    private HUD_Controller hud;
    
    public Image leftImage;
    public Image rightImage;
    public Image displayClueImage;
    // Start is called before the first frame update
    void Awake()
    {
        hud = FindObjectOfType<HUD_Controller>();
    }

    public void displayJournalPics()
    {
        // display the current journal images in the journalBook
        leftImage.sprite = Journal.Instance.journalImages[curr_JournalIndex].imageSource;
        leftImage.enabled = true;
        rightImage.enabled = true;
        if (curr_JournalIndex + 1 < Journal.Instance.journalImages.Count)
        {
            rightImage.sprite = Journal.Instance.journalImages[curr_JournalIndex + 1].imageSource;
            var tempColor = rightImage.color;
            tempColor.a = 1.0f;
            rightImage.color = tempColor;
        }
    }
    
    public void On_OpenJournal()
    {
        //turning off the journal flash anim
        GameObject.Find("JournalHelp").GetComponent<Animator>().SetBool("isClueObtained", false);
        
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);

        hud.disable_All();
        hud.journalPanel.SetActive(true);
        if (hud.journalPanel.activeSelf != false)
        {
            Animator animator = hud.journalPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        hud.On_OpenJournal();
        Invoke("displayJournalPics", 1.0f);
    }

    public void On_CloseJournal()
    {
        hud.isJPressed = false;

        Time.timeScale = 1;
        if (hud.journalPanel.activeSelf != false)
        {
            Animator animator = hud.journalPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", false);
            }
            
            leftImage.enabled = false;
            rightImage.enabled = false;
        }

        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);
        
        hud.On_CloseJournal();
    }
    public void On_TriggerNextPageJournal()
    {
        // checks if the next page returns back to the first page / journalImage
        if (Journal.Instance.journalImages.Count > 2 && (curr_JournalIndex+=2) >= Journal.Instance.journalImages.Count)
        {
            Debug.LogError($"Back to first page: {curr_JournalIndex}:{Journal.Instance.journalImages.Count}");
            curr_JournalIndex = 0;
        }
        else if (Journal.Instance.journalImages.Count <= 2)
        {
            // if there are no more pages infront, skip the process
            return;
        }
        
        // play page flip sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);
        Debug.LogError($"Pages: {curr_JournalIndex}:{Journal.Instance.journalImages.Count}");
        // change journalImage on the left side
        if (curr_JournalIndex < Journal.Instance.journalImages.Count)
        {
            leftImage.sprite = Journal.Instance.journalImages[curr_JournalIndex].imageSource;
        }
        // change journalImage on the right side
        if (curr_JournalIndex + 1 < Journal.Instance.journalImages.Count)
        {
            rightImage.sprite = Journal.Instance.journalImages[curr_JournalIndex + 1].imageSource;
            var tempColor = rightImage.color;
            tempColor.a = 1.0f;
            rightImage.color = tempColor;
        }
        // if empty, then set null on the imageSource
        else
        {
            // turn the color to invisible
            rightImage.sprite = null;
            var tempColor = rightImage.color;
            tempColor.a = 0.0f;
            rightImage.color = tempColor;
        }
        
       
    }

    public void On_TriggerPrevPageJournal()
    {
        // checks if the next page returns back to the first page / journalImage
        if (Journal.Instance.journalImages.Count > 2 && (curr_JournalIndex-=2) < 0)
        {
            Debug.LogError($"Jump to last page: {Journal.Instance.journalImages.Count}");
            if (Journal.Instance.journalImages.Count % 2 == 0)
            {
                curr_JournalIndex = Journal.Instance.journalImages.Count - 2;
            }
            else
            {
                curr_JournalIndex = Journal.Instance.journalImages.Count - 1;
            }
        }
        else if (Journal.Instance.journalImages.Count <= 2)
        {
            // if there are no more pages behind, skip the process
            return;
        }
        
        // play page flip sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);
        Debug.LogError($"Pages: {curr_JournalIndex}:{Journal.Instance.journalImages.Count}");
        // change journalImage on the left side
        if (curr_JournalIndex < Journal.Instance.journalImages.Count)
        {
            leftImage.sprite = Journal.Instance.journalImages[curr_JournalIndex].imageSource;
        }
        // change journalImage on the right side
        if (curr_JournalIndex + 1 < Journal.Instance.journalImages.Count)
        {
            rightImage.sprite = Journal.Instance.journalImages[curr_JournalIndex + 1].imageSource;
            var tempColor = rightImage.color;
            tempColor.a = 1.0f;
            rightImage.color = tempColor;
        }
        // if empty, then set null on the imageSource
        else
        {
            // turn the color to invisible
            rightImage.sprite = null;
            var tempColor = rightImage.color;
            tempColor.a = 0.0f;
            rightImage.color = tempColor;
        }
    }
}