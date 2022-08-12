using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePool : MonoBehaviour, IPoolFunctions
{
    
    //the type of pool and the originalObj; both are required, set you're preferred values for the constructors(maxSize, isFixAllocation?)
    [HideInInspector] public ObjPools itemPool;
    public List<GameObject> originalObjs = new List<GameObject>();

    //transform location of the poolStorage and spawn locations
    private Transform poolableLocation;
    private List<Transform> spawnLocations = new List<Transform>();

    //max size of the pool and if its size isDynamic
    [SerializeField] private int maxPoolSizePerObj = 20; //default
    [SerializeField] private bool fixedAllocation = true; //default

    void Start()
    {
        itemPool = new ObjPools(this.maxPoolSizePerObj, this.fixedAllocation,
            this.spawnLocations, Pool_Type.OBJECTIVE, this.GetComponent<IPoolFunctions>());
        poolableLocation = this.transform;
        this.itemPool.Initialize(ref originalObjs, poolableLocation, this);
    }

    //start of "IPoolFunctions" functions 
    //**
    public void onRequestGo(List<Transform> spawnLocations)
    {

    }
    public void onReleaseGo()
    {

    }
    //**
    //end of "IPoolFunctions" functions 
}