using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class JournalImage
{
    public JournalImage(E_ClueInteraction clueIdentification, Sprite imageSource)
    {
        this.clueIdentification = clueIdentification;
        this.imageSource = imageSource;
    }
    public E_ClueInteraction clueIdentification;
    public Sprite imageSource;
}

public sealed class Journal
{
    //public Dictionary<string, Image> journalEntries = new Dictionary<string, Image>();
    //public List<JournalImage> journalImages = new List<JournalImage>();
    //public bool isJournalObtained = false;
    
    private static Journal instance = null;
        
    public static Journal Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Journal();
                Debug.LogWarning("instance created");
            }
            return instance;
        }
    }
}
