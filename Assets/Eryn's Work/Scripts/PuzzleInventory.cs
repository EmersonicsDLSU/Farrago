using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInventory : MonoBehaviour
{
    public List<GameObject> puzzleItems;
    
    public void AddToInventory(GameObject obj)
    {
        this.puzzleItems.Add(obj);
    }

    public void DeleteFromInventory(string name)
    {
        for (int i = 0; i < puzzleItems.Count; i++)
        {
            if (this.puzzleItems[i].gameObject.name == name)
            {
                this.puzzleItems[i] = null;
                this.puzzleItems.RemoveAt(i);
                break;
            }
        }
    }

    public bool FindInInventory(string name)
    {
        for (int i = 0; i < this.puzzleItems.Count; i++)
        {
            if (this.puzzleItems[i].gameObject.name == name)
            {
                Debug.LogWarning("FOUND OBJECT: " + name);
                return true;
            }
        }
        return false;
    }

}
