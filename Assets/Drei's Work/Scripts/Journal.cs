using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public sealed class Journal
{
    public Dictionary<string, Image> journalEntries = new Dictionary<string, Image>();
    public List<Tuple<Image, Image>> journalPages = new List<Tuple<Image, Image>>();
    public bool isJournalObtained = false;
    
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

    //Call this to organize and recreate a List of Tuple
    public void pairImages()
    {
        string key1 = "J0";
        string key2 = "J1";
        
        //iterate each journal Images and pair them
        for (int i = 0; i < journalEntries.Count; i+=2)
        {
            key1 = "J" + (i).ToString();
            key2 = "J" + (i + 1).ToString();
            this.journalPages.Add(new Tuple<Image, Image>(journalEntries[key1],checkIfNext(key2)));
        }
    }

    //check if there's an existing journal for the next string(key2), else return null
    private Image checkIfNext(string possibleString)
    {
        if (journalEntries.ContainsKey(possibleString))
        {
            return journalEntries[possibleString];
        }
        else
        {
            return null;
        }
    }

}
