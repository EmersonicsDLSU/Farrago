using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List <GameObject> respawnObjs = new List<GameObject>();
    public List <CutSceneTypes> respawnTypes = new List<CutSceneTypes>();
    public Dictionary<CutSceneTypes, GameObject> respawnDictionary = new Dictionary<CutSceneTypes, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < respawnObjs.Count; i++)
        {
            this.respawnDictionary.Add(this.respawnTypes[i], this.respawnObjs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
