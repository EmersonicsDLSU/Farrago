using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Public class for puzzle Item
public class C_PuzzleItem
{
    // holds the enum and gameboject of the puzzleItem
    public PuzzleItem item_enum;
    public GameObject item_GO;
    public C_PuzzleItem(PuzzleItem item_enum, GameObject item_GO)
    {
        this.item_enum = item_enum;
        this.item_GO = item_GO;
    }
}

// Singleton Class
public class PuzzleInventory : MonoBehaviour
{
    private static PuzzleInventory _instance;
    
    public static Dictionary<PuzzleItem, C_PuzzleItem> puzzleItems;

    public static PuzzleInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PuzzleInventory();
                puzzleItems = new Dictionary<PuzzleItem, C_PuzzleItem>(){};
            }
            return _instance;
        }
    }
    
    

    public void AddToInventory(PuzzleItem identity, GameObject obj)
    {
        Debug.LogWarning($"Added Item: {identity} : {obj.name}");
        puzzleItems.Add(identity, new C_PuzzleItem(identity, obj));
    }

    public void DeleteFromInventory(PuzzleItem item)
    {
        puzzleItems.Remove(item);
    }

    public bool FindInInventory(PuzzleItem item)
    {
        if (puzzleItems.ContainsKey(item))
        {
            Debug.LogWarning("FOUND OBJECT: " + name);
            return true;
        }
        return false;
    }

}
