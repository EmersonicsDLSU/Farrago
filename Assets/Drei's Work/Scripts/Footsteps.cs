using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
