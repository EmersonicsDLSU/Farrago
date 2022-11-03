using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawnerCollection : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnerObjectList = new List<GameObject>();
    [SerializeField] private List<RatSpawnerIdentification> spawnerList = new List<RatSpawnerIdentification>();
    public Dictionary<RatSpawnerArea, GameObject> spawnerCollection = new Dictionary<RatSpawnerArea, GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        AssignTimelineCollection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //adds each playableDirector's GO from the level to this list; also adds the list of timeline triggers
    private void AssignTimelineCollection()
    {
        //adds to the dictionary
        int i = 0;
        foreach (var obj in spawnerList)
        { 
            spawnerCollection.Add(obj.spawnerType, spawnerObjectList[i++]);
        }
    }
}
