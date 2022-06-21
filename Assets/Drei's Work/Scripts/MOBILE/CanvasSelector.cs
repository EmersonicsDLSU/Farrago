using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum AppPlatform
{
    WINDOWS,
    ANDROID
};

public class CanvasSelector : MonoBehaviour
{
    public static CanvasSelector Instance;

    [SerializeField] private GameObject windowsCanvas;
    [SerializeField] private GameObject androidCanvas;
    [HideInInspector] public AppPlatform currentAppPlatform;

    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);


        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            windowsCanvas.SetActive(true);
            androidCanvas.SetActive(false);
            currentAppPlatform = AppPlatform.WINDOWS;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            windowsCanvas.SetActive(false);
            androidCanvas.SetActive(true);
            currentAppPlatform = AppPlatform.ANDROID;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
