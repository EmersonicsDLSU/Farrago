using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VineExtender : MonoBehaviour
{
    [Space] [Header("Interactables")] public GameObject interactableParent;
    public Image interactableFill;

    private bool isColorCorrect;
    private Color colorToAssign;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.interactableParent.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if ((ButtonActionManager.Instance.isInteractHeldDown == true))
            {
                switch (this.transform.tag)
                {
                    case "Interactable Vine":
                        isColorCorrect = colorChecker(this.transform.tag, GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color);
                        Debug.Log("is color correct: " + isColorCorrect);

                        if (isColorCorrect == true)
                        {
                            colorToAssign = GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color;


                            // ------- TEMP CODE FOR LEVEL 6, REMOVE SOON -------------
                            interactableParent.SetActive(false);
                            this.gameObject.GetComponent<Animator>().SetBool("willGrow", true);
                            Invoke("stopVineAnim", 3.0f);
                            // --------------------------------------------------------



                            //CHANGE VINE COLOR
                            this.gameObject.GetComponent<Renderer>().material.color = colorToAssign;

                        }
                        break;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.interactableParent.SetActive(false);
        }
    }

    bool colorChecker(string interactableType, Color characterCurrColor)
    {
        float H, S, V;
        if (interactableType == "Interactable Fire")
        {
            Color.RGBToHSV(characterCurrColor, out H, out S, out V);
            Debug.Log("H: " + H + " S: " + S + " V: " + V);

            if ((H >= 16.0f / 360.0f && H <= 48.0f / 360.0f) && (S >= 34.0f / 100.0f && S <= 100.0f / 100.0f) && (V >= 58.0f / 100.0f && V <= 100.0f / 100.0f))
            {
                return true;
            }
        }

        else if (interactableType == "Interactable Electricity")
        {
            if (characterCurrColor == Color.yellow)
            {
                return true;
            }
        }

        else if (interactableType == "Interactable Vine")
        {
            if (characterCurrColor == Color.green)
            {
                return true;
            }
        }

        return false;
    }

    private void stopVineAnim()
    {
        this.gameObject.GetComponent<Animator>().enabled = false;
    }
}
