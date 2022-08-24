using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointsHandler : MonoBehaviour, IDataPersistence
{
    private static RespawnPointsHandler _instance;

    public static RespawnPointsHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RespawnPointsHandler>();

                if (_instance == null)
                {
                    _instance = new RespawnPointsHandler();
                }
            }

            return _instance;
        }
    }

    public static RespawnPoints CurrentRespawnPoint;

    public void LoadData(GameData data)
    {
        CurrentRespawnPoint = (RespawnPoints)data.currentRespawnPoint;
    }

    public void SaveData(GameData data)
    {
        data.currentRespawnPoint = (int)CurrentRespawnPoint;
    }
}
