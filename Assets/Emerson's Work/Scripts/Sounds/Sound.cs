using UnityEngine;
using UnityEngine.Audio;

public enum SoundCode
{
    NONE = -1,
    PLAYER_FOOTSTEP,
    PLAYER_ABSORB,
    PLAYER_CLEANSE,
    UI_OPEN_JOURNAL
}

[System.Serializable]
public class Sound
{
    public SoundCode code = SoundCode.NONE;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range (.1f, 3f)]
    public float pitch = 1.0f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
     
}