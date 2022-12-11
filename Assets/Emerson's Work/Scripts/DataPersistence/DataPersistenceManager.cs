using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private bool isAutoSave = false;
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;
    private CareerData careerData;
    private List<IDataPersistence> dataPersistenceObjects;
    private List<ICareerDataPersistence> careerDataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";
    private string selectedCareerId = "CareerData";

    private Coroutine autoSaveCoroutine;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            //Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence) 
        {
            //Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        this.careerDataPersistenceObjects = FindAllCareerDataPersistenceObjects();
        LoadCareer();

        if (isAutoSave)
        {
            // start up the auto saving coroutine
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }

            autoSaveCoroutine = StartCoroutine(AutoSave());
        }

    }

    public void ChangeSelectedProfileId(string newProfileId) 
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
    }

    public void DeleteProfileData(string profileId = null) 
    {
        // delete the current profile
        if (profileId == null)
        {
            profileId = selectedProfileId;
        }
        // delete the data for this profile id
        dataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    private void InitializeSelectedProfileId() 
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId) 
        {
            this.selectedProfileId = testSelectedProfileId;
            //Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame() 
    {
        this.gameData = new GameData();
        //Debug.LogError("Create New Game");
    }
    public void NewCareer() 
    {
        this.careerData = new CareerData();
        //Debug.LogError("Create New Career");
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence) 
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull) 
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (this.gameData == null) 
        {
            //Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
        
        //Debug.LogError("Load Game");
    }
    
    public void LoadCareer()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence) 
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.careerData = dataHandler.LoadCareer(selectedCareerId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.careerData == null) 
        {
            //Debug.LogError("Creating Career");
            NewCareer();
        }

        // if no data can be loaded, don't continue
        if (this.careerData == null) 
        {
            //Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (ICareerDataPersistence dataPersistenceObj in careerDataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(careerData);
        }
        
        //Debug.LogError("Load Career");
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence) 
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.gameData == null) 
        {
            //Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // timestamp the data so we know when it was last saved
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(gameData, selectedProfileId);
    }
    
    public void SaveCareer()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence) 
        {
            //Debug.LogError("SAVE CANCEL");
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.careerData == null) 
        {
            //Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (ICareerDataPersistence dataPersistenceObj in careerDataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(careerData);
        }

        // timestamp the data so we know when it was last saved
        careerData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(careerData, selectedCareerId);
        //Debug.LogError("SETTINGS SAVED");
    }
    private void OnApplicationQuit() 
    {
        //SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    private List<ICareerDataPersistence> FindAllCareerDataPersistenceObjects() 
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<ICareerDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<ICareerDataPersistence>();

        return new List<ICareerDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData() 
    {
        return gameData != null;
    }
    public GameData GetGameData() 
    {
        return gameData;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData() 
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            //Debug.Log("Auto Saved Game");
        }
    }
}