using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointsHandler : MonoBehaviour, IDataPersistence
{
    private static RespawnPointsHandler instance = null;

    public static RespawnPointsHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RespawnPointsHandler();
            }

            return instance;
        }
    }

    public RespawnPoints CurrentRespawnPoint = RespawnPoints.NONE;

    public void LoadData(GameData data)
    {
        CurrentRespawnPoint = (RespawnPoints)data.currentRespawnPoint;
    }

    public void SaveData(GameData data)
    {
        data.currentRespawnPoint = (int)CurrentRespawnPoint;
    }
}
