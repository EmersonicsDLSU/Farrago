using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioClips
{
    public string clipLabel;
    public AudioClip clip;
}

[System.Serializable]
public class SFX
{
    public string clipLabel;
    public AudioClip clip;
    public AudioSource source;
}

public class Audio_Transmitter : MonoBehaviour
{
    public List<AudioClips> BGMClips;

    public AudioSource BGMSource1, BGMSource2;

    [Space]

    public List<SFX> SFXClips;

    [Space]

    public List<SFX> AmbienceClips;

    [Space]

    public List<SFX> playerSFXClips;

    [Space]

    public AudioMixer MasterMixer;
    public AudioMixer BGMMixer;
    public AudioMixer SFXMixer;
    public AudioMixer AmbienceMixer;
    public AudioMixer PlayerSFXMixer;

    MasterAudio_Manager MasterAudioInstance;
    BGM_Manager BGMInstance;
    SFX_Manager SFXInstance;
    PlayerSFX_Manager PlayerSFXInstance;


    public static Audio_Transmitter Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(this);
        
        Initialize();
    }

    public void Initialize()
    {
        MasterAudioInstance = MasterAudio_Manager.Instance;
        BGMInstance = BGM_Manager.Instance;
        SFXInstance = SFX_Manager.Instance;
        PlayerSFXInstance = PlayerSFX_Manager.Instance;

        MasterAudioInstance.MasterMixer = MasterMixer;
        BGMInstance.BGMMixer = BGMMixer;
        SFXInstance.SFXMixer = SFXMixer;
        PlayerSFXInstance.PlayerSFXMixer = PlayerSFXMixer;

        BGMInstance.Initialize(BGMClips, BGMSource1, BGMSource2);
        SFXInstance.Initialize(SFXClips);
        PlayerSFXInstance.Initialize(playerSFXClips);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BGMInstance.SwapTrack(BGMInstance.BGMClips[0].clip));

        /*Just for testing will be removed
        BGMInstance.BGMSource1 = BGMSources[0];
        BGMInstance.BGMSource2 = BGMSources[1];
        StartCoroutine(BGMInstance.SwapTrack(BGMInstance.BGMClips[0].clip));
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MasterVolume(float value)
    {
        MasterAudioInstance.volume(value);
    }

    public void BGMVolume(float value)
    {
        BGMInstance.volume(value);
    }

    public void SFXVolume(float value)
    {
        SFXInstance.volume(value);
        PlayerSFXInstance.volume(value);
    }

    public void AmbienceVolume(float value)
    {

    }

    public void DestroyAllSingleton()
    {
        MasterAudioInstance.Destroy();
        BGMInstance.Destroy();
        SFXInstance.Destroy();
        PlayerSFXInstance.Destroy();
    }
}
