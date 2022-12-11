using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_R9_Grow : TimelineTrigger
{
    public override void OAwake()
    {

    }

    public override void OStart()
    {

    }
    
    public override void ODelegates()
    {
        D_Start += Event1;
        D_End += E_Event1;
    }

    public void OnDestroy()
    {
        D_Start -= Event1;
        D_End -= E_Event1;
    }

    [SerializeField] private RatSpawner ratSpawner;
    [SerializeField] private GameObject destinationSet;
    [SerializeField] private GameObject[] deActivateDestinations;
    [SerializeField] private GameObject[] ActivateDestinations;
    

    private void Event1(C_Event e)
    {
        StartCoroutine(AssignNewDestinations());
    }

    private IEnumerator AssignNewDestinations()
    {
        yield return new WaitForSeconds(10.0f);   
        // remove some rats destination points
        Debug.LogError($"Final Rat New Destinations");
        foreach (var pos in deActivateDestinations)
        {
            pos.SetActive(false);
        }
        foreach (var pos in ActivateDestinations)
        {
            pos.SetActive(true);
        }
        foreach (var rat in ratSpawner.enemyPool.usedObjects)
        {
            rat.GetComponentInChildren<EnemyPatrolling>().enemy_property.walkSpeed /= 1.5f;
            rat.GetComponentInChildren<EnemyPatrolling>().assignDestinations(destinationSet);
        }
    }

    public GameObject winPanel;
    public GameObject HUD;
    public GameObject quitConfirmationPanel;

    private void E_Event1(C_Event e)
    {
        Time.timeScale = 0;

        HUD.SetActive(false);
        winPanel.SetActive(true);
        quitConfirmationPanel.SetActive(false);
    }
    
    public override void CallEndTimelineEvents()
    {
        // add the current respawnPoint
        respawnPointsHandler.CurrentRespawnPosition = transform.position;

        GetComponent<BoxCollider>().enabled = false;
        isCompleted = true;
        // call the delegate of this clue
        if (D_End != null)
        {
            D_End(new C_Event());
        }
        // show and disable objects
        foreach (var obj in End_ObjectsToHide)
        {
            obj.SetActive(false);
        }
        foreach (var obj in End_ObjectsToShow)
        {
            obj.SetActive(true);
        }
    }

    public override void OOnTriggerEnter(Collider other)
    {

    }

    private bool onceUse = false;

    public override void OOnTriggerStay(Collider other)
    {
        // if the cure potion was obtained
        if (!onceUse && !FindObjectOfType<R9_CurePotion>().isActive)
        {
            onceUse = true;
            base.OOnTriggerEnter(other);
        }
    }
    public override void OOnTriggerExit(Collider other)
    {
        base.OOnTriggerExit(other);
    }
    public override void OOnResetScene()
    {
        base.OOnResetScene();
    }
    public override void OLoadData(GameData data)
    {
        base.OLoadData(data);
    }
    
    public override void OSaveData(GameData data)
    {
        base.OSaveData(data);
    }

}
