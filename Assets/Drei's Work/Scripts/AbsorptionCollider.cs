using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// NOTE: High Memory Cost, what if we have a lot of interactable objects in the scene
// This class has its own reference in each interactable objects
public class AbsorptionCollider : MonoBehaviour
{
    //MainPlayerSc script compoenent
    [SerializeField] private MainPlayerSc MainPlayerScript;
    private PotionAbsorption potionAbsSc;

    void Start()
    {
        if (FindObjectOfType<MainPlayerSc>() != null)
        {
            MainPlayerScript = FindObjectOfType<MainPlayerSc>();
        }
        else
        {
            Debug.LogError($"Missing MainPlayerSc Script in {this.gameObject.name}");
        }

        if (MainPlayerScript.PotionAbsorptionSC != null)
        {
            this.potionAbsSc = MainPlayerScript.PotionAbsorptionSC;
        }
        else
        {
            Debug.LogError($"Missing PotionAbsorption Script in {this.gameObject.name}");
        }
    }
    //checks if the player enters the collider of this obj
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // assigns the parent gameobject of this gameobject
            potionAbsSc.ColorInteractableGO = this.transform.parent.gameObject;
            // assigns the parent gameobject of this gameobject
            potionAbsSc.object_ID = this.transform.parent.gameObject.GetComponent<Object_ID>();
            // assigns the Intertactable icon gameobject
            potionAbsSc.interactableIcon = this.transform.parent.GetChild(0).gameObject;
            // assigns the Intertactable icon gameobject
            potionAbsSc.interactableFillIcon = this.transform.parent.GetChild(0).gameObject.
                transform.GetChild(1).GetComponent<Image>();
            this.potionAbsSc.canAbsorb = true;
            this.potionAbsSc.interactableIcon.SetActive(true);
        }
    }
    //checks if the player is inside the collider of this obj
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MainPlayerScript.playerMovementSc.MovementX != 0 || 
                MainPlayerScript.playerMovementSc.MovementY != 0 || 
                MainPlayerScript.playerMovementSc._playerProperty.isJump)
            {
                //Debug.LogError($"Player is Moving, can't interact with {this.gameObject.name}: {MainPlayerScript.playerMovementSc.MovementX}:{MainPlayerScript.playerMovementSc.MovementY}");
                this.potionAbsSc.canAbsorb = false;
                this.potionAbsSc.isAbsorbing = false;
                MainPlayerScript.playerAngelaAnim.IH_ConsumeAnim(ref MainPlayerScript, this.potionAbsSc.isAbsorbing);
            }
            else
            {
                //Debug.LogError($"Can Interact");
                this.potionAbsSc.canAbsorb = true;
            }
        }
    }
    //checks if the player leaves the collider of this obj
    private void OnTriggerExit(Collider other)
    {
        //isInsideCollider = false;
        if (other.CompareTag("Player"))
        {
            // reset PotionAbsorption properties
            this.potionAbsSc.canAbsorb = false;
            this.potionAbsSc.interactableIcon.SetActive(false);
            this.potionAbsSc.interactableFillIcon.fillAmount = 0.0f;
        }
    }
}
