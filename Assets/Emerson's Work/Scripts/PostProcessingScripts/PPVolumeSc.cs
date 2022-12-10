using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPVolumeSc : MonoBehaviour
{
    [SerializeField] private List<Volume> volumeMain;
    private List<Vignette> vignetteProfile = new List<Vignette>();
    
    //external scripts
    public TimelineLevel timelineLevelSc = null;
    [HideInInspector] public RespawnPointsHandler respawnPointsHandler = null;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < volumeMain.Count; i++)
        {
            Vignette temp = null;
            volumeMain[i].profile.TryGet(out temp);
            vignetteProfile.Add(temp);
        }
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
        if (respawnPointsHandler == null)
        {
            if (FindObjectOfType<RespawnPointsHandler>() != null) respawnPointsHandler = FindObjectOfType<RespawnPointsHandler>();
            else Debug.LogError($"Missing \"RespawnPointsHandler script\" in {this.gameObject.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //VignetteEffect();
        if (IsDeathEffect)
            DeathVignetteEffect();
    }
    
    [SerializeField] float VigIntensityTickInterval = 15.0f;
    [SerializeField] float Vig_Death_Trans_Speed = 2.0f;
    private bool IsDoneLevel1Intro = false;
    [HideInInspector]public bool IsDeathEffect = false;
    private float death_effect_ticks = 0.0f;


    public void TurnOffDeathVignetteEffect()
    {
        death_effect_ticks = 0.0f;
        IsDeathEffect = false;
    }

    // death transition vignette effect
    private void DeathVignetteEffect()
    {
        int index = (int) respawnPointsHandler.CurrentRespawnPoint;
        if (index >= 4) index -= 1;

        if (death_effect_ticks >= 8.0f)
        {
            TurnOffDeathVignetteEffect();
            return;
        }
        else if (death_effect_ticks < 2.5f)
        {
            death_effect_ticks += Time.deltaTime;
            vignetteProfile[index].color.Override(Color.black);
            Vector2 closing = new Vector2(0.5f, 0.5f);
            vignetteProfile[index].center.value = closing;
            vignetteProfile[index].intensity.value += VigIntensityTickInterval * Time.deltaTime * Vig_Death_Trans_Speed;
            vignetteProfile[index].smoothness.value = 
                Mathf.Clamp(vignetteProfile[index].intensity.value, 0.3f, 1.0f);
        }
        else if (death_effect_ticks > 2.5f && death_effect_ticks < 6.0f)
        {
            death_effect_ticks += Time.deltaTime;
            vignetteProfile[index].color.Override(Color.black);
            Vector2 closing = new Vector2(-1.0f, -1.0f);
            vignetteProfile[index].center.value = closing;
            vignetteProfile[index].intensity.value += VigIntensityTickInterval * Time.deltaTime * Vig_Death_Trans_Speed;
            vignetteProfile[index].intensity.value =
                Mathf.Clamp(vignetteProfile[index].intensity.value, 0.3f, 1.0f);
        }
        else if (death_effect_ticks > 6.0f && death_effect_ticks < 7.9f)
        {
            death_effect_ticks += Time.deltaTime;
            vignetteProfile[index].color.Override(new Color(28.0f / 255.0f, 33.0f / 255.0f, 46.0f / 255.0f));
            Vector2 closing = new Vector2(0.5f, 0.5f);
            vignetteProfile[index].center.value = closing;
            vignetteProfile[index].intensity.value -= VigIntensityTickInterval * Time.deltaTime * Vig_Death_Trans_Speed;
            vignetteProfile[index].intensity.value =
                Mathf.Clamp(vignetteProfile[index].intensity.value, 0.3f, 1.0f);
        }
        else
        {
            death_effect_ticks += Time.deltaTime;
            vignetteProfile[index].color.Override(new Color(28.0f / 255.0f, 33.0f / 255.0f, 46.0f / 255.0f));
            Vector2 closing = new Vector2(0.5f, 0.5f);
            vignetteProfile[index].center.value = closing;
            vignetteProfile[index].intensity.value -= VigIntensityTickInterval * Time.deltaTime * Vig_Death_Trans_Speed;
            vignetteProfile[index].intensity.value =
                Mathf.Clamp(vignetteProfile[index].intensity.value, 0.3f, 1.0f);
        }
    }

    //this is a specific function for the eye opening effect in the 1STlevelIntro Cutscene
    private void VignetteEffect()
    {
        if (timelineLevelSc.currentSceneType == CutSceneTypes.Level1Intro && 
            !timelineLevelSc.timelinePlayIsFinished)
        {
            if (timelineLevelSc.currentTimeline.time < 4.0f)
            {
                if (timelineLevelSc.currentTimeline.time < 0.5f)
                {
                    vignetteProfile[0].color.Override(Color.black);
                    Vector2 closing = new Vector2(-1.0f, -1.0f);
                    vignetteProfile[0].center.value = closing;
                    vignetteProfile[0].intensity.value = 1.0f;
                    vignetteProfile[0].smoothness.value = 1.0f;
                }
                else if (timelineLevelSc.currentTimeline.time > 1.5f && timelineLevelSc.currentTimeline.time < 2.5f)
                {
                    Vector2 closing = new Vector2(0.5f, 0.5f);
                    vignetteProfile[0].center.value = closing;
                    vignetteProfile[0].intensity.value += VigIntensityTickInterval * Time.deltaTime;
                    vignetteProfile[0].smoothness.value = 
                        Mathf.Clamp(vignetteProfile[0].intensity.value, 0.0f, 1.0f);
                }
                else
                {
                    Vector2 closing = new Vector2(0.5f, 0.5f);
                    vignetteProfile[0].center.value = closing;
                    vignetteProfile[0].intensity.value -= VigIntensityTickInterval * Time.deltaTime;
                    vignetteProfile[0].intensity.value =
                        Mathf.Clamp(vignetteProfile[0].intensity.value, 0.0f, 1.0f);
                }
            }
            else
            {
                vignetteProfile[0].color.Override(new Color(28.0f / 255.0f, 33.0f / 255.0f, 46.0f / 255.0f));
                Vector2 closing = new Vector2(0.5f, 0.5f);
                vignetteProfile[0].center.value = closing;
                vignetteProfile[0].intensity.value = 0.3f;
                vignetteProfile[0].smoothness.value = 0.0f;
                IsDoneLevel1Intro = true;
            }
        }
    }

    public void closeVignette()
    {
        //Debug.LogError($"Closing Vignette");
        vignetteProfile[0].color.Override(Color.black);
        Vector2 closing = new Vector2(-1.0f, -1.0f);
        vignetteProfile[0].center.value = closing;
        vignetteProfile[0].intensity.value = 1.0f;
        vignetteProfile[0].smoothness.value = 1.0f;
    }

    public void openVignette()
    {
        //Debug.LogError($"Opening Vignette");
        vignetteProfile[0].color.Override(new Color(28.0f / 255.0f, 33.0f / 255.0f, 46.0f / 255.0f));
        Vector2 closing = new Vector2(0.5f, 0.5f);
        vignetteProfile[0].center.value = closing;
        vignetteProfile[0].intensity.value = 0.3f;
        vignetteProfile[0].smoothness.value = 0.0f;
        IsDoneLevel1Intro = true;
    }
}