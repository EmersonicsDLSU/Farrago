using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterAudio_Manager
{
    public AudioMixer MasterMixer;

    float _multiplier = 30.0f;

    private static MasterAudio_Manager _instance;
    public static MasterAudio_Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MasterAudio_Manager();
            return _instance;
        }
    }

    public void volume(float value)
    {
        MasterMixer.SetFloat("MasterVol", Mathf.Log10(value) * _multiplier);
    }

    public void Destroy()
    {
        this.Destroy();
    }
}
