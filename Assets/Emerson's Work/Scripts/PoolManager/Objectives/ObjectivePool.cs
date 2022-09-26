using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivePool : MonoBehaviour, IPoolFunctions
{
    // parent transform
    [SerializeField] private Transform ParentTransform; 
    //the type of pool and the originalObj; both are required, set you're preferred values for the constructors(maxSize, isFixAllocation?)
    [HideInInspector] public ObjPools itemPool;
    [SerializeField] private List<GameObject> originalObjs = new List<GameObject>();

    //transform location of the poolStorage and spawn locations
    private Transform poolableLocation;
    private List<Transform> spawnLocations = new List<Transform>();

    //max size of the pool and if its size isDynamic
    [SerializeField] private int maxPoolSizePerObj = 20; //default
    [SerializeField] private bool fixedAllocation = true; //default

    void Awake()
    {
        spawnLocations.Add(ParentTransform);
        itemPool = new ObjPools(this.maxPoolSizePerObj, this.fixedAllocation,
            this.spawnLocations, Pool_Type.OBJECTIVE, this.GetComponent<IPoolFunctions>());
        poolableLocation = this.transform;
        this.itemPool.Initialize(ref originalObjs, poolableLocation, this);
        
    }

    public GameObject RequestAndChangeText(string text)
    {
        var go = itemPool.RequestPoolable();
        go.GetComponent<TextMeshProUGUI>().text = text;
        go.GetComponent<TextMeshProUGUI>().rectTransform.localScale = Vector3.one;

        return go;
    }

    //start of "IPoolFunctions" functions 
    //**
    public void onRequestGo(List<Transform> spawnLocations)
    {
        itemPool.usedObjects[itemPool.usedObjects.Count - 1].transform.parent = ParentTransform.transform;
    }
    public void onReleaseGo()
    {
        itemPool.availableObjects[itemPool.availableObjects.Count - 1].transform.parent = this.transform;
    }
    //**
    //end of "IPoolFunctions" functions 
}