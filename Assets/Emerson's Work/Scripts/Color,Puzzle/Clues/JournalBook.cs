using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class JournalBook : MonoBehaviour
{
    private int curr_JournalIndex = 0;
    private HUD_Controller hud;

    public Button clueBookmark;
    public Button objectivesBookmark;
    public GameObject[] pageButtons;

    public Image leftImage;
    public Image rightImage;
    public Image displayClueImage;
    public GameObject leftObjectivesPanel;
    // Start is called before the first frame update
    void Awake()
    {
        hud = FindObjectOfType<HUD_Controller>();
    }

    private bool isRecentPageIsClue = true;
    public void ActivateClueButtons(bool show)
    {
        isRecentPageIsClue = show;
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);

        foreach (var t in pageButtons)
            t.SetActive(show);
        if (FindObjectOfType<R2_Journal>().journalImages.Count <= 2)
        {
            foreach (var t in pageButtons)
                t.SetActive(false);
        }
        leftImage.transform.gameObject.SetActive(show);
        rightImage.transform.gameObject.SetActive(show);

        leftObjectivesPanel.SetActive(!show);
    }
    public void ShowObjectives()
    {
        FindObjectOfType<ObjectivePool>().itemPool.ReleaseAllPoolable();
        FindObjectOfType<QuestGiver>().UpdateObjectiveList();
    }

    public void displayJournalPics()
    {
        // display the current journal images in the journalBook
        leftImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex].imageSource;
        leftImage.enabled = true;
        rightImage.enabled = true;
        if (curr_JournalIndex + 1 < FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            rightImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex + 1].imageSource;
            var tempColor = rightImage.color;
            tempColor.a = 1.0f;
            rightImage.color = tempColor;
        }
    }
    
    public void On_OpenJournal()
    {
        //turning off the journal flash anim
        //GameObject.Find("JournalHelp").GetComponent<Animator>().SetBool("isClueObtained", false);
        
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
        ActivateClueButtons(isRecentPageIsClue);
        if (!isRecentPageIsClue)
        {
            ShowObjectives();
        }
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
        
        leftObjectivesPanel.SetActive(false);
        hud.On_CloseJournal();
    }
    public void On_TriggerNextPageJournal()
    {
        // checks if the next page returns back to the first page / journalImage
        if (FindObjectOfType<R2_Journal>().journalImages.Count > 2 && (curr_JournalIndex+=2) >= FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            //Debug.LogError($"Back to first page: {curr_JournalIndex}:{Journal.Instance.journalImages.Count}");
            curr_JournalIndex = 0;
        }
        else if (FindObjectOfType<R2_Journal>().journalImages.Count <= 2)
        {
            // if there are no more pages infront, skip the process
            return;
        }
        
        // play page flip sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);
        // change journalImage on the left side
        if (curr_JournalIndex < FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            leftImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex].imageSource;
        }
        // change journalImage on the right side
        if (curr_JournalIndex + 1 < FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            rightImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex + 1].imageSource;
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
        if (FindObjectOfType<R2_Journal>().journalImages.Count > 2 && (curr_JournalIndex-=2) < 0)
        {
            //Debug.LogError($"Jump to last page: {Journal.Instance.journalImages.Count}");
            if (FindObjectOfType<R2_Journal>().journalImages.Count % 2 == 0)
            {
                curr_JournalIndex = FindObjectOfType<R2_Journal>().journalImages.Count - 2;
            }
            else
            {
                curr_JournalIndex = FindObjectOfType<R2_Journal>().journalImages.Count - 1;
            }
        }
        else if (FindObjectOfType<R2_Journal>().journalImages.Count <= 2)
        {
            // if there are no more pages behind, skip the process
            return;
        }
        
        // play page flip sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("Journal").clip);
        //Debug.LogError($"Pages: {curr_JournalIndex}:{FindObjectOfType<R2_Journal>().journalImages.Count}");
        // change journalImage on the left side
        if (curr_JournalIndex < FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            leftImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex].imageSource;
        }
        // change journalImage on the right side
        if (curr_JournalIndex + 1 < FindObjectOfType<R2_Journal>().journalImages.Count)
        {
            rightImage.sprite = FindObjectOfType<R2_Journal>().journalImages[curr_JournalIndex + 1].imageSource;
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
