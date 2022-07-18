using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum SaveFile
{
    NONE = -1,
    FILE_0,
    FILE_1,
    FILE_2
}

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Player File Storage Config")]
    [SerializeField] private string playerFileName_0;
    [SerializeField] private bool useEncryption_0;
    [SerializeField] private string playerFileName_1;
    [SerializeField] private bool useEncryption_1;
    [SerializeField] private string playerFileName_2;
    [SerializeField] private bool useEncryption_2;
    [Header("Career File Storage Config")]
    [SerializeField] private string careerFileName_3;
    [SerializeField] private bool useEncryption_3;

    public GameData currentLoadedData;
    public CareerData currentLoadedCareerData;
    private GameData gameData_0;
    private GameData gameData_1;
    private GameData gameData_2;
    private CareerData careerData;
    private List<IDataPersistence> dataPersistenceObjects;
    private List<ICareerDataPersistence> careerDataPersistenceObjects;
    private FileDataHandler dataHandler_0;
    private FileDataHandler dataHandler_1;
    private FileDataHandler dataHandler_2;
    private FileDataHandler careerDataHandler;

    [HideInInspector] public SaveFile currentSaveFile = SaveFile.NONE;

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
        this.dataHandler_0 = new FileDataHandler(Application.persistentDataPath, playerFileName_0, useEncryption_0);
        this.dataHandler_1 = new FileDataHandler(Application.persistentDataPath, playerFileName_1, useEncryption_1);
        this.dataHandler_2 = new FileDataHandler(Application.persistentDataPath, playerFileName_2, useEncryption_2);
        this.careerDataHandler = new FileDataHandler(Application.persistentDataPath, careerFileName_3, useEncryption_3);
        // Load Career File
        SearchForPersistenceCareerObjInScene();
        LoadCareerData();
    }

    public bool CheckIfSaveFileExist(SaveFile saveFile)
    {
        bool isExist = false;
        switch (saveFile)
        {
            case SaveFile.FILE_0:
            {
                isExist = (dataHandler_0.LoadPlayerData() != null) ? 
                    true : false;
            }
                break;
            case SaveFile.FILE_1:
            {
                isExist = (dataHandler_1.LoadPlayerData() != null) ? 
                    true : false;
            }
                break;
            case SaveFile.FILE_2:
            {
                isExist = (dataHandler_2.LoadPlayerData() != null) ? 
                    true : false;
            }
                break;
        }

        return isExist;
    }

    public void SearchForPersistenceObjInScene()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void SearchForPersistenceCareerObjInScene()
    {
        this.careerDataPersistenceObjects = FindAllCareerDataPersistenceObjects();
    }

    public void DeleteFile(SaveFile saveFile)
    {
        switch (saveFile)
        {
            case SaveFile.FILE_0:
            {
                dataHandler_0.DeleteFile();
            }
                break;
            case SaveFile.FILE_1:
            {
                
                dataHandler_1.DeleteFile();
            }
                break;
            case SaveFile.FILE_2:
            {
                
                dataHandler_2.DeleteFile();
            }
                break;
        }
    }

    public void NewGame(SaveFile saveFile) 
    {
        Debug.LogError($"Create new Game in: {saveFile}");
        switch (saveFile)
        {
            case SaveFile.FILE_0:
            {
                this.gameData_0 = new GameData();
                currentSaveFile = SaveFile.FILE_0;
            }
                break;
            case SaveFile.FILE_1:
            {
                this.gameData_1 = new GameData();
                currentSaveFile = SaveFile.FILE_1;
            }
                break;
            case SaveFile.FILE_2:
            {
                this.gameData_2 = new GameData();
                currentSaveFile = SaveFile.FILE_2;
            }
                break;
        }
    }
    public void NewCareer() 
    {
        this.careerData = new CareerData();
    }

    public void LoadGame(SaveFile saveFile)
    {
        
        switch (saveFile)
        {
            case SaveFile.FILE_0:
            {
                // load any saved data from a file using the data handler
                if (dataHandler_0.LoadPlayerData() != null)
                    this.gameData_0 = dataHandler_0.LoadPlayerData();
                // if no PLAYER data can be loaded, initialize to a new game
                if (this.gameData_0 == null) 
                {
                    Debug.Log("No player data was found.");
                }
                SearchForPersistenceObjInScene();
                currentLoadedData = gameData_0;
                if (dataPersistenceObjects != null)
                {
                    // push the loaded data to all other scripts that need it
                    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                    {
                        dataPersistenceObj.LoadData(gameData_0);
                    }
                }
            }
                break;
            case SaveFile.FILE_1:
            {
                // load any saved data from a file using the data handler
                if (dataHandler_1.LoadPlayerData() != null)
                    this.gameData_1 = dataHandler_1.LoadPlayerData();
                // if no PLAYER data can be loaded, initialize to a new game
                if (this.gameData_1 == null) 
                {
                    Debug.Log("No player data was found.");
                }
                SearchForPersistenceObjInScene();
                currentLoadedData = gameData_1;
                if (dataPersistenceObjects != null)
                {
                    // push the loaded data to all other scripts that need it
                    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                    {
                        dataPersistenceObj.LoadData(gameData_1);
                    }
                }
            }
                break;
            case SaveFile.FILE_2:
            {
                // load any saved data from a file using the data handler
                if (dataHandler_2.LoadPlayerData() != null)
                    this.gameData_2 = dataHandler_2.LoadPlayerData();
                // if no PLAYER data can be loaded, initialize to a new game
                if (this.gameData_2 == null) 
                {
                    Debug.Log("No player data was found.");
                }
                SearchForPersistenceObjInScene();
                currentLoadedData = gameData_2;
                if (dataPersistenceObjects != null)
                {
                    // push the loaded data to all other scripts that need it
                    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                    {
                        dataPersistenceObj.LoadData(gameData_2);
                    }
                }
            }
                break;
        }
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
        currentLoadedCareerData = careerData;
        if (careerDataPersistenceObjects != null)
        {
            // push the loaded data to all other scripts that need it
            foreach (ICareerDataPersistence dataPersistenceObj in careerDataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(careerData);
            }
        }
    }

    public void SaveGame(SaveFile saveFile)
    {
        switch (saveFile)
        {
            case SaveFile.FILE_0:
            {
                // pass the data to other scripts so they can update it
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                {
                    dataPersistenceObj.SaveData(gameData_0);
                }
                // save that data to a file using the data handler
                dataHandler_0.Save(gameData_0);
            }
                break;
            case SaveFile.FILE_1:
            {
                // pass the data to other scripts so they can update it
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                {
                    dataPersistenceObj.SaveData(gameData_1);
                }
                // save that data to a file using the data handler
                dataHandler_1.Save(gameData_1);
            }
                break;
            case SaveFile.FILE_2:
            {
                // pass the data to other scripts so they can update it
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
                {
                    dataPersistenceObj.SaveData(gameData_2);
                }
                // save that data to a file using the data handler
                dataHandler_2.Save(gameData_2);
            }
                break;
        }
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
