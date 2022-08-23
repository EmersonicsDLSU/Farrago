using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class to be inherited by puzzle Item class
public abstract class PuzzleItemInteraction : MonoBehaviour
{
    public List<PuzzleItem> objectsRequired;
    
    public void Update()
    {
        InheritorsUpdate();
    }

    // Default Update content
    public virtual void InheritorsUpdate()
    {

    }

    // Function for checking if all the required items are obtained
    public virtual bool CheckRequired()
    {
        for (int i = 0; i < objectsRequired.Count; i++)
        {
            if (!PuzzleInventory.Instance.FindInInventory(objectsRequired[i]))
                return false;
        }
        return true;
    }
}
