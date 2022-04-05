using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSFX_Manager
{
    public List<SFX> SFXClips = new List<SFX>();

    public AudioMixer PlayerSFXMixer;

    float _multiplier = 30.0f;

    private static PlayerSFX_Manager _instance;
    public static PlayerSFX_Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerSFX_Manager();
            return _instance;
        }
    }

    public void Initialize(List<SFX> SFXInput)
    {
        clearAndReset();
        addSFXClipsToList(SFXInput);
    }

    public void addSFXClipsToList(List<SFX> SFXInput)
    {
        if(SFXInput.Count != 0)
        {
            foreach (var a in SFXInput)
                this.SFXClips.Add(a);
        }
    }

    public bool isPlaying(AudioSource source)
    {
        return source.isPlaying;
    }

    public void clearAndReset()
    {
        SFXClips.Clear();
    }

    public AudioSource findSFXSourceByLabel(string label)
    {
        foreach (var a in SFXClips)
        {
            if (a.clipLabel == label)
            {
                return a.source;
            }
        }
        return null;
    }

    public AudioSource findSFXSourceByTrack(AudioClip clip)
    {
        foreach (var a in SFXClips)
        {
            if (a.clip == clip)
            {
                return a.source;
            }
        }
        return null;
    }

    public void Play(AudioSource source)
    {
        source.Play();
    }

    public void Pause(AudioSource source)
    {
        source.Pause();
    }
    public void Stop(AudioSource source)
    {
        source.Stop();
    }

    public void volume(float value)
    {
        PlayerSFXMixer.SetFloat("PlayerSFXMasterVol", Mathf.Log10(value) * _multiplier);
    }

    public void Destroy()
    {
        this.Destroy();
    }
}
