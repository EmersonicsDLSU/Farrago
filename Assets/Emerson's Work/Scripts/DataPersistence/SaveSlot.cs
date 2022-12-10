using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private Text dateCreatedText;
    [SerializeField] private Text timeCreatedText;
    [SerializeField] private Text percentageCompleteText;

    [SerializeField] private Button saveSlotButton;

    private void Awake() 
    {

    }

    public void SetData(GameData data) 
    {
        //Debug.LogError($"Setting Data: {data == null}");
        // there's no data for this profileId
        if (data == null) 
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        // there is data for this profileId
        else 
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            dateCreatedText.text = data.dateCreated;
            timeCreatedText.text = data.timeCreated;
            percentageCompleteText.text = data.GetPercentageComplete() + "% COMPLETE";
        }
    }

    public string GetProfileId() 
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}