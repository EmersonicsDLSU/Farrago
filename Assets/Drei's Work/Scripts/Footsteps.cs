using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioClip stepClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        audioSource.spatialBlend = 0.95f;
    }

    private void OnStep()
    {
        audioSource.volume = Random.Range(0.8f, 1);
        audioSource.pitch = Random.Range(0.8f, 1.1f);
        audioSource.PlayOneShot(stepClip, 2f);
    }
    private void OnDeath()
    {
        audioSource.volume = Random.Range(0.8f, 1);
        audioSource.pitch = Random.Range(0.8f, 1.1f);
        audioSource.PlayOneShot(stepClip, 2f);
    }

    private void OnJump()
    {
        if (Random.Range(0, 10) >= 7)
        {
            audioSource.pitch = Random.Range(1.0f, 1.25f);
            audioSource.PlayOneShot(jumpClip, 2f);
        }
    }
}
