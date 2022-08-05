using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MainPlayerSc : MonoBehaviour, IDataPersistence
{
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public CharacterController playerCharController;
    //[HideInInspector] public Rigidbody agentRigid;
    //[HideInInspector] public MeshRenderer agentMesh;
    [HideInInspector] public SkinnedMeshRenderer playerSkinMesh;
    public Animator playerAnim;
    [HideInInspector] public PlayerAngelaAnimations playerAngelaAnim;

    //other player scripts
    [HideInInspector] public PlayerMovement playerMovementSc;
    [HideInInspector] public Inventory playerInventory;
    [HideInInspector] public PuzzleInventory player_puzzleInventory;
    [HideInInspector] public CharacterControllerDetection characterControllerDetection = null;
    [HideInInspector] public PotionAbsorption PotionAbsorptionSC = null;

    //external scripts
    public TimelineLevel timelineLevelSc = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerTrans = this.GetComponent<Transform>(); ;
        playerCharController = this.GetComponentInChildren<CharacterController>();
        playerSkinMesh = this.GetComponentInChildren<SkinnedMeshRenderer>();
        if(playerAnim == null)
        {
            playerAnim = this.GetComponentInChildren<Animator>();
        }
        playerAngelaAnim = this.GetComponentInChildren<PlayerAngelaAnimations>();
        playerMovementSc = this.GetComponentInChildren<PlayerMovement>();
        playerInventory = this.GetComponentInChildren<Inventory>();
        player_puzzleInventory = this.GetComponentInChildren<PuzzleInventory>();
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
        characterControllerDetection = this.GetComponentInChildren<CharacterControllerDetection>();
        PotionAbsorptionSC = this.GetComponentInChildren<PotionAbsorption>();

    }

    void Start()
    {
        Debug.LogError("Start to Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementSc.update(this);
        playerInventory.update(this);
        PotionAbsorptionSC.update(this);
    }

    public void LoadData(GameData data)
    {
        /*
        Debug.LogError($"Total Tries :{DataPersistenceManager.instance.currentLoadedData.total_tries}");
        // if first try, then we do not translate the player 
        if (DataPersistenceManager.instance.currentLoadedData.total_tries != 0)
        {
            Debug.LogError($"Translate to :{data.respawnPoint}");
            this.transform.position = data.respawnPoint;
        }
        */
    }
    
    public void SaveData(GameData data)
    {
        data.respawnPoint = this.transform.position;
        var temp = DateTime.Now;
        data.dateCreated = $"{temp.Day}/{temp.Month}/{temp.Year}";
        data.timeCreated = $"{temp.Hour}:{temp.Minute}";
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded; //You add your method to the delegate
    }
     
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
        {
            var data = DataPersistenceManager.instance.currentLoadedData;
            Debug.LogError($"Translate to :{data.respawnPoint} to {this.name}");
            this.transform.position = data.respawnPoint;
        }
    }
}
