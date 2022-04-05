using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGM_Manager
{
    public List<AudioClips> BGMClips = new List<AudioClips>();
    public AudioSource BGMSource1, BGMSource2;
    public bool isPlayingSource1 = true;
    public AudioMixer BGMMixer;

    float _multiplier = 30.0f;

    private static BGM_Manager _instance;
    public static BGM_Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BGM_Manager();
            return _instance;
        }
    }

    public void Initialize(List<AudioClips> BGMInput, AudioSource source1, AudioSource source2)
    {
        clearAndReset();
        addBGMClipsToList(BGMInput);

        BGMSource1 = source1;
        BGMSource2 = source2;

        if (BGMClips.Count != 0)
        {
            BGMSource1.clip = BGMClips[0].clip;
            if (BGMClips.Count > 1)
                BGMSource2.clip = BGMClips[1].clip;
        }
    }

    public void addBGMClipsToList(List<AudioClips> BGMInput)
    {
        if (BGMInput.Count != 0)
        {
            foreach (var a in BGMInput)
                this.BGMClips.Add(a);
        }
    }

    public AudioClip getClipByLabel(string label)
    {
        foreach (var a in BGMClips)
        {
            if (a.clipLabel == label)
            {
                return a.clip;
            }
        }
        return null;
    }

    public void Play()
    {
        if (isPlayingSource1)
            this.BGMSource1.Play();
        else
            this.BGMSource2.Play();
    }

    public void Pause()
    {
        if (isPlayingSource1)
            this.BGMSource1.Pause();
        else
            this.BGMSource2.Pause();
    }

    public void Stop()
    {
        if (isPlayingSource1)
            this.BGMSource1.Stop();
        else
            this.BGMSource2.Stop();

        //isPlayingSource1 = !isPlayingSource1;
    }

    public IEnumerator SwapTrack(AudioClip track)
    {
        float timeToFade = 1.25f;
        float timeElapsed = 0;
        Debug.Log("Yes");

        if (isPlayingSource1)
        {
            BGMSource2.clip = track;
            BGMSource2.Play();

            while (timeElapsed < timeToFade)
            {
                BGMSource2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                BGMSource1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            BGMSource1.Stop();
        }
        else
        {
            BGMSource1.clip = track;
            BGMSource1.Play();

            while (timeElapsed < timeToFade)
            {
                BGMSource1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                BGMSource2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            BGMSource2.Stop();
        }

        isPlayingSource1 = !isPlayingSource1;
    }

    public void volume(float value)
    {
        Debug.Log(value);
        BGMMixer.SetFloat("BGMMasterVol", Mathf.Log10(value) * _multiplier);
    }

    public void clearAndReset()
    {
        BGMClips.Clear();
        BGMSource1 = null;
        BGMSource2 = null;
        isPlayingSource1 = true;
    }

    public void Destroy()
    {
        this.Destroy();
    }
}
