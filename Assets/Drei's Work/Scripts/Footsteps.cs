using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnStep()
    {
        audioSource.PlayOneShot(clip);
    }
    
}
