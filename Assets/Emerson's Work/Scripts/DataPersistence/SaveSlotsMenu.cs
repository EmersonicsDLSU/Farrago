using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [SerializeField] private SaveSlot[] saveSlots;
    [SerializeField] private SavePanelSc savePanelSc;

    private void Awake() 
    {

    }
    
    public void OnSaveSlot(SaveSlot saveSlot) 
    {
        // disable all buttons
        DisableMenuButtons();

        // update the selected profile id to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        
        // create a new game - which will initialize our data to a clean slate
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();

        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        Loader.loadinstance.LoadLevel(2);
    }
    public void OnLoadSlot(SaveSlot saveSlot) 
    {
        // disable all buttons
        DisableMenuButtons();

        // update the selected profile id to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        
        // load the game, which will use that profile, updating our game data accordingly
        DataPersistenceManager.instance.LoadGame();

        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        Loader.loadinstance.LoadLevel(2);
    }

    public void OnDeleteFileSlot(SaveSlot saveSlot) 
    {
        //DisableMenuButtons();

        DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu();
    }

    public void ActivateMenu() 
    {
        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        int index = 0;
        // loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null)
            {
                savePanelSc.btnsDeleteFile[index++].interactable = false;
            }
            else
            {
                savePanelSc.btnsDeleteFile[index++].interactable = true;
            }
        }
    }

    public bool HasAFile(SaveSlot saveSlot)
    {
        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        GameData profileData = null;
        profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

        if (profileData != null)
        {
            return true;
        }

        return false;
    }

    private void DisableMenuButtons() 
    {
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
    }
}