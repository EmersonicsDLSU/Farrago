using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum areaType
{
    LEVEL = 0,
    CHASE
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

        if (other.CompareTag("PlayerModel"))
        {
            if(area == areaType.LEVEL)
            {
                AudioClip clip = BGM_Manager.Instance.getClipByLabel("BGMLevel" + level);
                StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }
            else if(area == areaType.CHASE)
            {
                AudioClip clip = BGM_Manager.Instance.getClipByLabel("Chase");
                StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }
            

            TextControl.textInstance.queueLevelText(Texts, level);
            levelClear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            if (area == areaType.CHASE)
            {
                AudioClip clip = BGM_Manager.Instance.getClipByLabel("BGMLevel" + level);
                StartCoroutine(BGM_Manager.Instance.SwapTrack(clip));
            }
            TextControl.textInstance.clearQueue();
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
