using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("Player File Storage Config")]
    [SerializeField] private string playerFileName;
    [SerializeField] private bool useEncryption_0;
    [Header("Career File Storage Config")]
    [SerializeField] private string careerFileName;
    [SerializeField] private bool useEncryption_1;

    public GameData currentLoadedData;
    public CareerData currentLoadedCareerData;
    private GameData gameData;
    private CareerData careerData;
    private List<IDataPersistence> dataPersistenceObjects;
    private List<ICareerDataPersistence> careerDataPersistenceObjects;
    private FileDataHandler dataHandler;
    private FileDataHandler careerDataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, playerFileName, useEncryption_0);
        this.careerDataHandler = new FileDataHandler(Application.persistentDataPath, careerFileName, useEncryption_1);
        // Load Career File
        SearchForPersistenceCareerObjInScene();
        LoadCareerData();
    }

    public void SearchForPersistenceObjInScene()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void SearchForPersistenceCareerObjInScene()
    {
        this.careerDataPersistenceObjects = FindAllCareerDataPersistenceObjects();
    }

    public void NewGame() 
    {
        this.gameData = new GameData();
    }
    public void NewCareer() 
    {
        this.careerData = new CareerData();
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = dataHandler.LoadPlayerData();
        
        // if no PLAYER data can be loaded, initialize to a new game
        if (this.gameData == null) 
        {
            Debug.Log("No player data was found.");
            // NewGame should not be place here; place it in a UI button event
            NewGame();
        }
        SearchForPersistenceObjInScene();
        if (dataPersistenceObjects != null)
        {
            // push the loaded data to all other scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        currentLoadedData = gameData;
    }

    public void LoadCareerData()
    {
        // load any saved data from a file using the data handler
        this.careerData = careerDataHandler.LoadCareerData();
        
        // if no CAREER data can be loaded, initialize to a new game
        if (this.careerData == null) 
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewCareer();
        }

        if (careerDataPersistenceObjects != null)
        {
            // push the loaded data to all other scripts that need it
            foreach (ICareerDataPersistence dataPersistenceObj in careerDataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(careerData);
            }
        }

        currentLoadedCareerData = careerData;
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }
    public void SaveCareerGame()
    {
        // pass the data to other scripts so they can update it
        foreach (ICareerDataPersistence dataPersistenceObj in careerDataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(careerData);
        }

        // save that data to a file using the data handler
        careerDataHandler.Save(careerData);
    }
    

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    private List<ICareerDataPersistence> FindAllCareerDataPersistenceObjects() 
    {
        IEnumerable<ICareerDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<ICareerDataPersistence>();

        return new List<ICareerDataPersistence>(dataPersistenceObjects);
    }
}
