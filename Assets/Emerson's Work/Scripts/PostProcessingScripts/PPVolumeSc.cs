using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPVolumeSc : MonoBehaviour
{
    private Volume volumeMain = null;
    private Vignette vignetteProfile = null;
    
    //external scripts
    public TimelineLevel timelineLevelSc = null;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Volume>() != null)
        {
            volumeMain = GetComponent<Volume>();
            volumeMain.profile.TryGet(out vignetteProfile);
        }
        else
        {
            Debug.LogError($"Missing Volume component in {this.gameObject.name}");
        }
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        VignetteEffect();
    }
    
    [SerializeField] float VigIntensityTickInterval = 15.0f;
    private bool IsDoneLevel1Intro = false;

    //this is a specific function for the eye opening effect in the 1STlevelIntro Cutscene
    private void VignetteEffect()
    {
        if (timelineLevelSc.currentSceneType == CutSceneTypes.Level1Intro && 
            !timelineLevelSc.timelinePlayIsFinished)
        {
            if (this.timelineLevelSc.currentTimeline.time < 4.0f)
            {
                if (this.timelineLevelSc.currentTimeline.time < 0.5f)
                {
                    Vector2 closing = new Vector2(-1.0f, -1.0f);
                    this.vignetteProfile.center.value = closing;
                    this.vignetteProfile.intensity.value = 1.0f;
                    this.vignetteProfile.smoothness.value = 1.0f;
                }
                else if (this.timelineLevelSc.currentTimeline.time > 1.5f && this.timelineLevelSc.currentTimeline.time < 2.5f)
                {
                    Vector2 closing = new Vector2(0.5f, 0.5f);
                    this.vignetteProfile.center.value = closing;
                    this.vignetteProfile.intensity.value += VigIntensityTickInterval * Time.deltaTime;
                    this.vignetteProfile.smoothness.value = 
                        Mathf.Clamp(this.vignetteProfile.intensity.value, 0.0f, 1.0f);
                }
                else
                {
                    Vector2 closing = new Vector2(0.5f, 0.5f);
                    this.vignetteProfile.center.value = closing;
                    this.vignetteProfile.intensity.value -= VigIntensityTickInterval * Time.deltaTime;
                    this.vignetteProfile.intensity.value =
                        Mathf.Clamp(this.vignetteProfile.intensity.value, 0.0f, 1.0f);
                }
            }
            else
            {
                Vector2 closing = new Vector2(-1.0f, -1.0f);
                this.vignetteProfile.center.value = closing;
                this.vignetteProfile.intensity.value = 0.0f;
                this.vignetteProfile.smoothness.value = 0.0f;
                this.IsDoneLevel1Intro = true;
            }
        }
    }

    public void closeVignette()
    {
        Debug.LogError($"Closing Vignette");
        Vector2 closing = new Vector2(-1.0f, -1.0f);
        this.vignetteProfile.center.value = closing;
        this.vignetteProfile.intensity.value = 1.0f;
        this.vignetteProfile.smoothness.value = 1.0f;
    }

    public void openVignette()
    {
        Vector2 closing = new Vector2(-1.0f, -1.0f);
        this.vignetteProfile.center.value = closing;
        this.vignetteProfile.intensity.value = 0.0f;
        this.vignetteProfile.smoothness.value = 0.0f;
        this.IsDoneLevel1Intro = true;
    }
}