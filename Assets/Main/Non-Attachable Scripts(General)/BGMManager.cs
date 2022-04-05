using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource BGM;

    [Space]

    [Header("BGM Sources")]
    public AudioClip MainMenu;
    public AudioClip Level1;

    public static BGMManager BGMInstance;

    private void Awake()
    {
        if (BGMInstance != null && BGMInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        BGMInstance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        BGMManager.BGMInstance.play(BGMManager.BGMInstance.MainMenu);
    }

    public void pauseBGM()
    {
        BGMManager.BGMInstance.BGM.Pause();
    }
    public void playBGM()
    {
        BGMManager.BGMInstance.BGM.UnPause();
    }
    public void stopBGM()
    {
        BGMManager.BGMInstance.BGM.Stop();
    }

    public void play(AudioClip audio)
    {
        BGMManager.BGMInstance.BGM.volume = 1.0f;
        BGMManager.BGMInstance.BGM.clip = audio;
        BGMManager.BGMInstance.BGM.Play();
    }

}
