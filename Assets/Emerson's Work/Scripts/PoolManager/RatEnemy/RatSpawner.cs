using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is an exmaple of our spawn type
public class RatSpawner : MonoBehaviour , IPoolFunctions
{
    //the type of pool and the originalObj; both are required, set you're preferred values for the constructors(maxSize, isFixAllocation?)
    [HideInInspector] public ObjPools enemyPool;
    [SerializeField] protected List <GameObject> originalObjs = new List<GameObject>();

    //transform locations of the poolStorage and spawn locations
    [HideInInspector] protected Transform poolableLocation;
    [HideInInspector] protected List <Transform> spawnLocations = new List<Transform>();

    //max size of the pool and if its size isDynamic
    [SerializeField] protected int maxPoolSizePerObj = 20; //default
    [SerializeField] protected int existingSpawnSize = 2; //default
    [SerializeField] protected bool fixedAllocation = true; //default

    //time count and time interval for the spawning
    protected float ticks = 0.0f;
    protected float spawn_interval = 0.01f;

    [SerializeField] protected GameObject spawnsSet;

    void Start()
    {
        assignSpawnLocations();
        enemyPool = new ObjPools(maxPoolSizePerObj, fixedAllocation,
            spawnLocations, Pool_Type.ENEMY, GetComponent<IPoolFunctions>());
        poolableLocation = transform;
        OStart();
    }
    
    public virtual void OStart()
    {
        enemyPool.Initialize(ref originalObjs, poolableLocation, this);
    }

    // Update is called once per frame
    void Update()
    {
        OUpdate();
    }

    public virtual void OUpdate()
    {
        if (this.ticks < spawn_interval)
        {
            this.ticks += Time.deltaTime;
        }
        //checks if the pool storage is not empty and the required existing size still complies
        else if (this.enemyPool.HasObjectAvailable(1) && this.enemyPool.usedObjects.Count < this.existingSpawnSize)
        {
            this.ticks = 0.0f;
            this.enemyPool.RequestPoolable();

            this.spawn_interval = Random.Range(0.15f, 0.5f);
        }
    }

    private void assignSpawnLocations()
    {
        //Get the spawnSet Obj and get all of its child objs which are the spawn transform points
        for (int i = 0; i < spawnsSet.transform.childCount; i++)
        {
            spawnLocations.Add(spawnsSet.transform.GetChild(i).transform);
            //Debug.LogError($"SpawnLocations count: {spawnLocations[i].transform.position.x}:{spawnLocations[i].transform.position.y}:{spawnLocations[i].transform.position.z}");
        }
    }

    //start of "IPoolFunctions" functions 
    //**
    public void onRequestGo(List<Transform> spawnLocations)
    {
        OOnRequestGo(spawnLocations);
    }
    public void onReleaseGo()
    {
        OOnReleaseGo();
    }
    //**
    //end of "IPoolFunctions" functions 

    public virtual void OOnRequestGo(List<Transform> spawnLocations)
    {
        //Debug.LogError($"spawnLocations count: {spawnLocations.Count}");
        int randIndex = Random.Range(0, spawnLocations.Count);
        //Debug.LogError($"random index: {randIndex}");
        //random spawn location
        this.enemyPool.usedObjects[this.enemyPool.usedObjects.Count - 1].transform.position = spawnLocations[randIndex].position;
        this.enemyPool.usedObjects[this.enemyPool.usedObjects.Count - 1].transform.SetParent(spawnLocations[randIndex]);
        this.enemyPool.usedObjects[this.enemyPool.usedObjects.Count - 1].gameObject.
            GetComponentInChildren<EnemyPatrolling>().assignDestinations(
                spawnLocations[randIndex].GetComponent<SpawnerDestinationSet>().DestinationSet);
    }
    public virtual void OOnReleaseGo()
    {

    }
}
