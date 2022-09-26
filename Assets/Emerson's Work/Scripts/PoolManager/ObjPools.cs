using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPools
{
    //transform location of the poolStorage and spawn locations
    public Transform poolableLocation;
    private List<Transform> spawnLocations = new List<Transform>();

    //max size of the pool and if its size isDynamic
    private int maxPoolSize = 20; //default
    private bool fixedAllocation = true; //default

    //collection of the available and used objects currently in the game
    public List<GameObject> availableObjects = new List<GameObject>();
    public List<GameObject> usedObjects = new List<GameObject>();

    //spawnerType
    private Pool_Type poolType;
    //spawnerType
    public Pool_Type PoolType
    {
        get { return this.poolType; }
        private set { poolType = value; }
    }

    private IPoolFunctions poolFunctions;

    //constructor; even though the parameters have default values, it is necessary to assign a value to it in your instantiation
    public ObjPools(int maxPoolSize = 20, bool fixedAllocation = true, List<Transform> spawnLocations = null, 
        Pool_Type pool_type = Pool_Type.NONE, IPoolFunctions poolFunc = null)
    {
        if(spawnLocations != null)
        {
            this.maxPoolSize = maxPoolSize;
            this.fixedAllocation = fixedAllocation;
            this.spawnLocations = spawnLocations;
            this.PoolType = pool_type;
            this.poolFunctions = poolFunc;
        }
        else
        {
            Debug.LogError("NO Spawn Locations Found for this Spawner!");
        }
    }

    public int MaxPoolSize
    {
        get { return maxPoolSize; }
    }

    //call this in the awake or start of every spawnType you've created
    public void Initialize<T>(ref List <GameObject> objs, Transform poolableLocation, T refSpawner)
    {
        this.poolableLocation = poolableLocation;
        int count = 0;
        for (int k = 0; k < objs.Count; k++)
        {
            PoolableObj copy = objs[k].GetComponent<PoolableObj>();
            for (int i = 0; i < MaxPoolSize; i++)
            {
                availableObjects.Add(copy.createCopy(this));
                if(typeof(T).Equals(typeof(RatSpawner)))
                {
                    availableObjects[i].GetComponent<AIAgent>().ratSpawnerSc = 
                        (RatSpawner)System.Convert.ChangeType(refSpawner, typeof(RatSpawner));
                    //Debug.LogError($"Stored: {availableObjects[i].GetComponent<AIAgent>().ratSpawnerSc != null}");
                }
                else if(typeof(T).Equals(typeof(RatChaseSpawner)))
                    availableObjects[i].GetComponent<AIAgent>().ratChaseSpawnerSc =
                        (RatChaseSpawner)System.Convert.ChangeType(refSpawner, typeof(RatChaseSpawner));
                /*
                else if(typeof(T).Equals(typeof(InventoryPool)))
                    availableObjects[i].GetComponent<AIAgent>().ratChaseSpawnerSc = (InventoryPool)System.Convert.ChangeType(refSpawner, typeof(InventoryPool));
                */
                availableObjects[count++].SetActive(false);
        }   }

    }

    //increase the maxSize of the pool
    public void increaseMaxPoolSize(int addSize)
    {
        if(!fixedAllocation)
        {
            maxPoolSize += addSize;
        }
        else
        {
            Debug.Log("Fixed Allocation is set to True!");
        }
    }

    public bool HasObjectAvailable(int size)
    {
        if (this.availableObjects.Count >= size && size > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject RequestPoolable()
    {
        usedObjects.Add(availableObjects[0]);
        availableObjects.RemoveAt(0);
        // calls the onActivate func of the poolable
        this.poolFunctions.onRequestGo(this.spawnLocations);
        //poolable now exist in the game
        usedObjects[usedObjects.Count - 1].SetActive(true);
        return usedObjects[usedObjects.Count - 1];
    }
    public void RequestPoolable(Enemy_Type enemy_type)
    {
        for (int i = 0; i < availableObjects.Count; i++)
        {
            if(availableObjects[i].transform.GetComponentInChildren<EnemyStatistics>().EnemyType == enemy_type)
            {
                usedObjects.Add(availableObjects[i]);
                availableObjects.RemoveAt(i);
                //poolable now exist in the game
                usedObjects[usedObjects.Count - 1].SetActive(true);
                //sets the onActivate func of the poolable
                this.poolFunctions.onRequestGo(this.spawnLocations);
            }
        }
    }

    public void RequestPoolable(ColorCode color_code, Transform inventoryTransform)
    {
        for (int i = 0; i < availableObjects.Count; i++)
        {
            if (availableObjects[i].GetComponent<Potion_ID>().colorCode == color_code)
            {
                usedObjects.Add(availableObjects[i]);
                availableObjects.RemoveAt(i);
                //sets the onActivate func of the poolable
                this.poolFunctions.onRequestGo(this.spawnLocations);
                // assign the object to its parent first
                usedObjects[usedObjects.Count - 1].transform.SetParent(inventoryTransform);
                usedObjects[usedObjects.Count - 1].transform.position = inventoryTransform.position;
                usedObjects[usedObjects.Count - 1].transform.localScale = new Vector3(1, 1, 1);
                //poolable now exist in the game
                usedObjects[usedObjects.Count - 1].SetActive(true);
                break;
            }
        }
    }

    public void ReleasePoolable(int index)
    {
        availableObjects.Add(usedObjects[index]);
        usedObjects.RemoveAt(index);
        availableObjects[availableObjects.Count - 1].transform.SetParent(this.poolableLocation);
        this.poolFunctions.onReleaseGo();
        availableObjects[availableObjects.Count - 1].SetActive(false);
    }
    public void ReleaseAllPoolable()
    {
        while (usedObjects.Count > 0)
        {
            ReleasePoolable(usedObjects[0]);
        }
    }
    public void ReleasePoolable(GameObject go)
    {
        if(usedObjects.Contains(go) || go != null)
        {
            availableObjects.Add(go);
            usedObjects.Remove(go);
            availableObjects[availableObjects.Count - 1].transform.SetParent(this.poolableLocation);
            this.poolFunctions.onReleaseGo();
            availableObjects[availableObjects.Count - 1].SetActive(false);
        }
        else
        {
            //Debug.LogError($"Object doesn't exist in the list.");
        }
    }
}
