using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectCollision : MonoBehaviour
{
    [HideInInspector] public bool isPlayerCaptured = false;

    private void Start()
    {
        //reset to properties everytime the scene is loaded
        isPlayerCaptured = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log($"Obj: {collision.transform.name} is captured!");
            this.isPlayerCaptured = true;
        }
    }
}
