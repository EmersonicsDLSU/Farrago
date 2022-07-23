using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HW_RadialTrigger : MonoBehaviour
{
    public float rad = 0.5f;
    public Transform radialCenter;
    public Transform trigObj;
    public double displacement;
    public double unityCalculatedDisplacement;

    // Start is called before the first frame update
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(trigObj.position, Vector3.forward, rad / 10);

        displacement = Math.Sqrt(
            (radialCenter.position.x - trigObj.position.x) * (radialCenter.position.x - trigObj.position.x) +
            (radialCenter.position.y - trigObj.position.y) * (radialCenter.position.y - trigObj.position.y));
        unityCalculatedDisplacement = (radialCenter.position - trigObj.position).magnitude;

        Handles.color = displacement > rad ? Color.red : Color.green;
        Handles.DrawWireDisc(radialCenter.position, Vector3.forward, rad);
        Handles.DrawLine(radialCenter.position, trigObj.position);
    }
}
