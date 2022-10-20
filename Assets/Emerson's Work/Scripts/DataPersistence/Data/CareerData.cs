using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CareerData
{
    public long lastUpdated;
    public int total_deaths;
    public int total_visit;
    public float total_time_played;

    // Settings value
    public int screen_size;
    public int quality;
    public int is_fullscreen;
    public float bgm_volume;
    public float sfx_volume;
    

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public CareerData()
    {
        total_deaths = 0;
        total_visit = 0;
        total_time_played = 0;

        // Settings default values
        screen_size = 2;
        quality = 0;
        is_fullscreen = 1;
        bgm_volume = 1;
        sfx_volume = 1;
    }
}