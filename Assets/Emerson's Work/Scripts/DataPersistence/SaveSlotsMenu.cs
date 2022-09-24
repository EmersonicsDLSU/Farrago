using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [SerializeField] private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake() 
    {

    }

    public void OnSaveSlotClicked(SaveSlot saveSlot) 
    {
        // disable all buttons
        DisableMenuButtons();

        // update the selected profile id to be used for data persistence
        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!isLoadingGame) 
        {
            // create a new game - which will initialize our data to a clean slate
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            // load the game, which will use that profile, updating our game data accordingly
            DataPersistenceManager.instance.LoadGame();
        }

        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        Loader.loadinstance.LoadLevel(2);
    }
    public void OnClearClicked(SaveSlot saveSlot) 
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
        ActivateMenu(isLoadingGame);
    }

    public void ActivateMenu(bool isLoadingGame) 
    {
        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // loop through each save slot in the UI and set the content appropriately
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame) 
            {
                saveSlot.SetInteractable(false);
            }
            else 
            {
                saveSlot.SetInteractable(true);
            }
        }
    }
    
    private void DisableMenuButtons() 
    {
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
    }
}