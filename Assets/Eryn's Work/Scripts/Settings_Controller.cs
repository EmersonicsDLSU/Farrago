using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Settings_Controller : MonoBehaviour, ICareerDataPersistence
{
    // Start is called before the first frame update
    List<int> widths = new List<int>() { 3860, 2560, 1920, 1280 };
    List<int> heights = new List<int>() { 2160, 1440, 1080, 720 };

    [SerializeField] private RenderPipelineAsset[] renderPipelines;

    void Awake()
    {

    }
    

    public void SetScreenSize(int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullscreen);
        //PlayerPrefs.SetInt("ScreenSize", index);

        screen_size = index;
    }

    public void SetFullScreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
        is_fullscreen = _fullscreen;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        QualitySettings.renderPipeline = renderPipelines[index];
        //PlayerPrefs.SetInt("Quality", index);

        //Debug.LogError("GRAPHICS IS CHANGED");

        quality = index;
    }

    public void SetBGMVolume(float value)
    {
        Audio_Transmitter.Instance.BGMVolume(value);
        //PlayerPrefs.SetFloat("BGMVolume", value);

        bgm_volume = value;
    }

    public void SetSFXVolume(float value)
    {
        Audio_Transmitter.Instance.SFXVolume(value);
        //PlayerPrefs.SetFloat("SFXVolume", value);

        sfx_volume = value;
    }

    public void SaveSettingsData()
    {
        DataPersistenceManager.instance.SaveCareer();
    }

    // Settings value
    private int screen_size;
    private int quality;
    private bool is_fullscreen;
    private float bgm_volume;
    private float sfx_volume;

    // Settings components
    [SerializeField] private Slider bgm_slider;
    [SerializeField] private Slider sfx_slider;
    [SerializeField] private Dropdown resolution_dropdown;
    [SerializeField] private Dropdown quality_dropdown;
    [SerializeField] private Toggle fullScreenToggle;

    public void LoadData(CareerData data)
    {
        SetScreenSize(data.screen_size);
        resolution_dropdown.value = data.screen_size;

        SetQuality(data.quality);
        quality_dropdown.value = data.quality;
        //Debug.LogError($"Quality Settings: {quality_dropdown.value}");

        var isFullscreen = (data.is_fullscreen == 1) ? true : false;
        SetFullScreen(isFullscreen);
        fullScreenToggle.isOn = isFullscreen;

        SetBGMVolume(data.bgm_volume);
        bgm_slider.value = data.bgm_volume;


        SetSFXVolume(data.sfx_volume);
        sfx_slider.value = data.sfx_volume;
    }

    public void SaveData(CareerData data)
    {
        data.screen_size = screen_size;
        data.quality = quality;
        data.is_fullscreen = is_fullscreen ? 1 : 0;;
        data.bgm_volume = bgm_volume;
        data.sfx_volume = sfx_volume;
    }
}
