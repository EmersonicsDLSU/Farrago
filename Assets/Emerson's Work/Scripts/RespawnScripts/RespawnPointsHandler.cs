using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointsHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<GameObject> respawnPointList = new List<GameObject>();
    public Dictionary<RespawnPoints, RespawnTrigger> respawnPointCollection = new Dictionary<RespawnPoints, RespawnTrigger>();
    public RespawnPoints CurrentRespawnPoint;
    public Vector3 CurrentRespawnPosition = new Vector3(-249.49f, 14.79f, -2.98f);

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.LogError($"Assign TimelineCollection!");
        AssignTimelineCollection();
    }
    
    //adds each playableDirector's GO from the level to this list; also adds the list of timeline triggers
    private void AssignTimelineCollection()
    {
        //adds to the dictionary
        int i = 0;
        foreach (var obj in respawnPointList)
        { 
            respawnPointCollection.Add(obj.GetComponent<RespawnTrigger>().respawnPointEnum, obj.GetComponent<RespawnTrigger>());
        }
    }

    private void ActivateRespawnEvent()
    {
        if (CurrentRespawnPoint != RespawnPoints.NONE)
        {
            //Debug.LogError($"CurrentRespawnPoint: {CurrentRespawnPoint}");
            respawnPointCollection[CurrentRespawnPoint].CallStartTimelineEvents();

        }
    }

    public void LoadData(GameData data)
    {
        CurrentRespawnPoint = (RespawnPoints)data.currentRespawnPoint;
        CurrentRespawnPosition = data.respawnPoint;
        ActivateRespawnEvent();
    }

    public void SaveData(GameData data)
    {
        //Debug.LogError($"Saved Respawn Point!!");
        data.currentRespawnPoint = (int)CurrentRespawnPoint;
        Vector3 offset = new Vector3(CurrentRespawnPosition.x,
            CurrentRespawnPosition.y + 2.0f, CurrentRespawnPosition.z);
        data.respawnPoint = offset;
    }
}
