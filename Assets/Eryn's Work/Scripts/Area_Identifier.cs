using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum areaType
{
    LEVEL = 0,
    CHASE,
    SWARM,
};

public class Area_Identifier : MonoBehaviour
{
    
    public int level = 0;
    public areaType area;

    private bool levelClear = false;

    [TextArea]
    public string[] Texts;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {       

        if (other.CompareTag("Player"))
        {
            if(area == areaType.LEVEL)
            {
                
                    AudioClip clip = BGM_Manager.Instance.getClipByLabel("BGMLevel" + level);
                    StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }
            else if (area == areaType.CHASE)
            {
                Debug.LogWarning("Hit chase");
                AudioClip clip = BGM_Manager.Instance.getClipByLabel("Chase");
                StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }
            else if (area == areaType.SWARM)
            {
                Debug.LogWarning("Hit swarm");
                AudioClip clip = BGM_Manager.Instance.getClipByLabel("Swarm");
                StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }

            TextControl.Instance.queueLevelText(Texts, level);
            levelClear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextControl.Instance.clearQueue();
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
