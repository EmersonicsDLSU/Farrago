using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Manager
{
    public List<SFX> SFXClips = new List<SFX>();

    public AudioMixer SFXMixer;

    float _multiplier = 30.0f;

    // Start is called before the first frame update
    private static SFX_Manager _instance;
    public static SFX_Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SFX_Manager();
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
        if (SFXInput.Count != 0)
        {
            foreach (var a in SFXInput)
                this.SFXClips.Add(a);
        }
    }

    public AudioSource findSFXSourceByLabel(string label)
    {
        foreach (var a in SFXClips)
        {
            if(a.clipLabel == label)
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
        SFXMixer.SetFloat("SFXMasterVol", Mathf.Log10(value) * _multiplier);
    }

    public void clearAndReset()
    {
        SFXClips.Clear();
    }

    public void Destroy()
    {
        this.Destroy();
    }
}
