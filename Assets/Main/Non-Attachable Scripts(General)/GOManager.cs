using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOManager
{
    static void changeColor(ref GameObject go, Color col)
    {
        if(go.GetComponent<MeshRenderer>() == null)
        {
            Debug.LogError("GOManager Script: GameObject doesn't contain a MeshRenderer component!");
        }
        else
        {
            go.GetComponent<MeshRenderer>().material.color = col;
        }
    }

}
