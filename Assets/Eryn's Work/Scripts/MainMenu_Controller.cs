using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Controller : MonoBehaviour
{
    [SerializeField]
    public GameObject mainMenu;
    public GameObject loadPanel;
    public GameObject settingsPanel;

    void disableAll()
    {
        mainMenu.SetActive(false);
        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void on_Load()
    {
        disableAll();
        loadPanel.SetActive(true);
        if (loadPanel.activeSelf != false)
        {
            Animator animator = loadPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }
    }

    public void on_Settings()
    {
        disableAll();
        settingsPanel.SetActive(true);

        if (settingsPanel.activeSelf != false)
        {
            Animator animator = settingsPanel.GetComponent<Animator>();

            if(animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);

            }
        }
    }

    public void on_New()
    {
        LoaderScript.loadScene(1, SceneManager.sceneCountInBuildSettings - 1);
    }

    public void on_Quit()
    {
        Application.Quit();
    }

    public void on_Return()
    {
        if (KeybindManager.Instance.waitingForInput)
        {
            return;
        }

        if (settingsPanel.activeSelf != false)
        {
            Animator animator = settingsPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        if (loadPanel.activeSelf != false)
        {
            Animator animator = loadPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");

                animator.SetBool("Open", !isOpen);
            }
        }

        Invoke("disableAll", 1);

        Invoke("activateMenu", 1.01f);
    }

    public void to_MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void activateMenu()
    {
        mainMenu.SetActive(true);
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        /*
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(vKey))
            {
                Debug.Log(vKey.ToString());
            }
        }
        */
    }

}
