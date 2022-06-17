using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Settings_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    List<int> widths = new List<int>() { 3860, 2560, 1920, 1280 };
    List<int> heights = new List<int>() { 2160, 1440, 1080, 720 };

    [SerializeField] private RenderPipelineAsset[] renderPipelines;

    void Awake()
    {
        if(!PlayerPrefs.HasKey("ScreenSize"))
            SetScreenSize(2);
        else
            SetScreenSize(PlayerPrefs.GetInt("ScreenSize"));

        if(!PlayerPrefs.HasKey("Quality"))
            SetQuality(5);
        else
            SetQuality(PlayerPrefs.GetInt("Quality"));

        SetFullScreen(true);

        if (!PlayerPrefs.HasKey("BGMVolume"))
            SetBGMVolume(1);
        else
            SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume"));

        if (!PlayerPrefs.HasKey("SFXVolume"))
            SetSFXVolume(1);
        else
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void SetScreenSize(int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullscreen);
        PlayerPrefs.SetInt("ScreenSize", index);
    }

    public void SetFullScreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        QualitySettings.renderPipeline = renderPipelines[index];
        PlayerPrefs.SetInt("Quality", index);
    }

    public void SetBGMVolume(float value)
    {
        Audio_Transmitter.Instance.BGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        Audio_Transmitter.Instance.SFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
