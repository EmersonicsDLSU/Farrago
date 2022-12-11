using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDetection : MonoBehaviour
{
    [HideInInspector] public bool isPlayerCaptured = false;
    [SerializeField] private float distance_captured = 0.5f;

    private void Start()
    {
        //reset to properties everytime the scene is loaded
        isPlayerCaptured = false;
    }


    //Checks if an enemy touches Angele; Gameover
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Enemy"))
        {
            //checks the distance once hit
            float distance = Vector3.Distance(this.transform.position, hit.transform.position);
            //Debug.LogError($"Distance between: {distance}!");
            //checks the condition of the touch distance
            if (distance <= this.distance_captured)
            {
                this.isPlayerCaptured = true;
                //restarts the rat to another position
                if(hit.transform.parent.GetComponent<AIAgent>().ratSpawnerSc != null)
                {
                    hit.transform.parent.GetComponent<AIAgent>().ratSpawnerSc.enemyPool.ReleasePoolable(hit.transform.parent.gameObject);
                    Debug.LogError($"Obj: {hit.transform.name} is touched!");
                }
                else if (hit.transform.parent.GetComponent<AIAgent>().ratChaseSpawnerSc != null)
                {
                    FindObjectOfType<RatChaseSpawner>().enemyPool.ReleaseAllPoolable();
                    FindObjectOfType<TimelineLevel>().ResetCutscene(CutSceneTypes.Level4RatCage);
                }

                
                // resets the 'isPlayerCaptured' boolean
                Gameplay_DelegateHandler.D_OnDeath(new Gameplay_DelegateHandler.C_OnDeath(isPlayerCaptured:false));
            }
        }
    }

    private void OnControllerColliderExit()
    {

    }
}