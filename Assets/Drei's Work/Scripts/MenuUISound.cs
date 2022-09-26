using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUISound : MonoBehaviour
{
    [SerializeField] private AudioClip menu_open_clip;
    [SerializeField] private AudioClip menu_close_clip;
    [SerializeField] private AudioClip menu_ui_click_clip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMenuOpenSound()
    {
        audioSource.PlayOneShot(menu_open_clip, 0.6f);
    }

    public void PlayMenuCloseSound()
    {
        audioSource.PlayOneShot(menu_close_clip, 0.6f);
    }

    public void PlayMenuClickSound()
    {
        audioSource.PlayOneShot(menu_ui_click_clip, 0.6f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
