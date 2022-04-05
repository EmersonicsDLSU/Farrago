using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource PlayerAudio;
    public AudioSource EnvironmentAudio;
    public AudioSource EnemyAudio;

    [Space]

    [Header("UI")]
    public AudioClip ButtonClick;

    [Space]

    [Header("Player")]
    public AudioClip Move;
    public AudioClip Fire;
    public AudioClip Water;
    public AudioClip Electric;
    public AudioClip Ground;

    [Space]

    [Header("Player")]
    public AudioClip EnemyAttack;

    public static SFXManager SFXInstance;

    private void Awake()
    {
        if (SFXInstance != null && SFXInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SFXInstance = this;
        DontDestroyOnLoad(this);

    }

    public void PlayerplaySFX(AudioClip audio)
    {

        SFXManager.SFXInstance.PlayerAudio.volume = 1.0f;
        SFXManager.SFXInstance.PlayerAudio.pitch = 1.0f;
        SFXManager.SFXInstance.PlayerAudio.PlayOneShot(audio);
    }
    public void EnvironmentplaySFX(AudioClip audio)
    {

        SFXManager.SFXInstance.EnvironmentAudio.volume = 1.0f;
        SFXManager.SFXInstance.EnvironmentAudio.pitch = 1.0f;
        SFXManager.SFXInstance.EnvironmentAudio.PlayOneShot(audio);
    }
    public void EnemyplaySFX(AudioClip audio)
    {

        SFXManager.SFXInstance.EnemyAudio.volume = 1.0f;
        SFXManager.SFXInstance.EnemyAudio.pitch = 1.0f;
        SFXManager.SFXInstance.EnemyAudio.PlayOneShot(audio);
    }

    public void playMove()
    {
        SFXManager.SFXInstance.PlayerAudio.volume = Random.Range(0.8f, 1.0f);
        SFXManager.SFXInstance.PlayerAudio.pitch = Random.Range(0.8f, 1.0f);
        SFXManager.SFXInstance.PlayerAudio.PlayOneShot(SFXManager.SFXInstance.Move);
    }

    public bool isPlay(AudioClip audio)
    {
        if (SFXManager.SFXInstance.PlayerAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.PlayerAudio.isPlaying == true)
                return true;
            else
                return false;
        }
        if (SFXManager.SFXInstance.EnvironmentAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.EnvironmentAudio.isPlaying == true)
                return true;
            else
                return false;
        }
        if (SFXManager.SFXInstance.EnemyAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.EnemyAudio.isPlaying == true)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public void stopSFX(AudioClip audio)
    {
        if (SFXManager.SFXInstance.PlayerAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.PlayerAudio.isPlaying == true)
                SFXManager.SFXInstance.PlayerAudio.Stop();
        }
        else if (SFXManager.SFXInstance.EnvironmentAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.EnvironmentAudio.isPlaying == true)
                SFXManager.SFXInstance.EnvironmentAudio.Stop();
        }
        else if (SFXManager.SFXInstance.EnemyAudio.name == audio.name)
        {
            if (SFXManager.SFXInstance.EnemyAudio.isPlaying == true)
                SFXManager.SFXInstance.EnemyAudio.Stop();
        }
    }
}
