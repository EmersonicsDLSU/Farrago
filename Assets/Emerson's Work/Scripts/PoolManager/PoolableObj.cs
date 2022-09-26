using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObj : APoolable
{
    [SerializeField] private Pool_Type poolType;

    public Pool_Type PoolType
    {
        get { return this.poolType; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public override void initialize()
    {
        this.transform.SetParent(this.poolRef.poolableLocation);
    }

    public override void onRelease()
    {

    }

    public override void onActivate()
    {

    }

    public GameObject createCopy(ObjPools pool)
    {
        //duplicate the origin copy
        GameObject go = 
            GameObject.Instantiate(this.gameObject, this.transform.position, Quaternion.identity) as GameObject;

        //awake
        go.GetComponent<PoolableObj>().poolRef = pool;
        go.GetComponent<PoolableObj>().initialize();
        return go;
    }
}
