using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// NOTE: High Memory Cost, what if we have a lot of interactable objects in the scene
// This class has its own reference in each interactable objects
public class AbsorptionCollider : MonoBehaviour
{
    // MainPlayerSc script component
    private MainPlayerSc MainPlayerScript;
    private PotionAbsorption potionAbsSc;
    // Interactable UI(Icon); Heads Up Display
    [HideInInspector]public GameObject interactableIcon;

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
        
        interactableIcon = transform.parent.GetChild(0).gameObject;
        if (interactableIcon == null)
        {
            Debug.LogError($"Missing Interactable Icon in {this.gameObject.name}");
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
            potionAbsSc.interactableFillIcon = this.transform.parent.GetChild(0).gameObject.
                transform.GetChild(1).GetComponent<Image>();
            this.potionAbsSc.canAbsorb = true;
            interactableIcon.SetActive(true);
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
                this.potionAbsSc.canAbsorb = false;
                this.potionAbsSc.isAbsorbing = false;
                //MainPlayerScript.playerAngelaAnim.IH_ConsumeAnim(ref MainPlayerScript, this.potionAbsSc.isAbsorbing);
            }
            else
            {
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
            potionAbsSc.canAbsorb = false;
            interactableIcon.SetActive(false);
            potionAbsSc.interactableFillIcon.fillAmount = 0.0f;
            // TODO: TEMPORARY
            potionAbsSc.absorbingFillIcon.fillAmount = 0.0f;
            potionAbsSc.ResetProperties(ref MainPlayerScript, true);
        }
    }
}
