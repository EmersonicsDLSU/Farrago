using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CareerData
{
    public int total_deaths = 0;
    public int total_visit = 0;
    public float total_time_played = 0;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public CareerData()
    {
        total_deaths = 0;
        total_visit = 0;
        total_time_played = 0;
    }
}