using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This is an exmaple of our spawn type
public class InventoryPool : MonoBehaviour, IPoolFunctions
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
            this.spawnLocations, Pool_Type.COLOR, this.GetComponent<IPoolFunctions>());
        poolableLocation = this.transform;
        this.itemPool.Initialize(ref originalObjs, poolableLocation, this);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void setCurrentColorPosition(Transform trans = null)
    {
        if(trans != null)
        {
            this.itemPool.usedObjects[this.itemPool.usedObjects.Count - 1].transform.SetParent(trans);
            this.itemPool.usedObjects[this.itemPool.usedObjects.Count - 1].transform.position = trans.position;
        }
        else
        {

        }
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
