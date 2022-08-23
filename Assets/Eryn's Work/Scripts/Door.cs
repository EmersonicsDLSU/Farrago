using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : PuzzleItemInteraction
{
    //Interact
    private bool canInteract = false;
    private float timePress;
    private bool interactAgain = true;
    private bool interacted = false;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    private MainPlayerSc mainPlayer;

    void Start()
    {
        mainPlayer = FindObjectOfType<MainPlayerSc>();
    }

    public override void InheritorsUpdate ()
    {
        if (canInteract && !interacted)
        {
            interactableParent.SetActive(true);
            if (CheckRequired())
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    timePress = 0;
                    interactableFill.fillAmount = 0.0f;
                    interactAgain = true;
                }
                else if (Input.GetKey(KeyCode.E) && interactAgain)
                {
                    mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                    timePress += Time.deltaTime;
                    interactableFill.fillAmount = timePress / 2.0f;

                    if (interactableFill.fillAmount == 1.0f && !this.GetComponent<AudioSource>().isPlaying)
                    {
                        this.GetComponent<AudioSource>().Play();
                        //Basically dito yung ano man action dapat nung puzzle
                        //pwedeng something na customized function from a script tas iccall lang dito kunwari yung door magoopen ganun
                        if (this.gameObject.activeSelf != false)
                        {
                            Animator animator = this.GetComponent<Animator>();

                            if (animator != null)
                            {
                                animator.SetTrigger("Interact");
                            }
                        }

                        timePress = 0;
                        interactableFill.fillAmount = 0.0f;
                        interactAgain = false;
                        interacted = true;
                    }
                }
            }
            
        }
        else
        {
            timePress = 0;
            interactableFill.fillAmount = 0.0f;
            interactableParent.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            this.canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("EXIT");
        if (other.CompareTag("Player"))
            this.canInteract = false;
    }
}
