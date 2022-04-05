using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//USED FOR POTION PREFABS FOR IDENTIFICATION PURPOSES
public class Potion_ID : MonoBehaviour
{
    //OBJECT NAME
    [HideInInspector]public string objectName;

    public ColorCode colorCode;

    void Awake()
    {
        objectName = this.gameObject.name;
    }

}
