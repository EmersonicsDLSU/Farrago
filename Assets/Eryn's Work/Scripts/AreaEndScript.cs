using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Debug = System.Diagnostics.Debug;

public class AreaEndScript : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject HUD;
    public GameObject quitConfirmationPanel;

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;

        HUD.SetActive(false);
        winPanel.SetActive(true);
        quitConfirmationPanel.SetActive(false);
        /*
        //Set here for area end either load async a new scene or what
        if (other.CompareTag("PlayerModel"))
            Loader.loadinstance.LoadLevel(0);
        */
    }
}
