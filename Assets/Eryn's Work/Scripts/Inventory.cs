using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// SLOTS HOLD PROPERTIES OF AN INVENTORY SLOT
[System.Serializable]
public class Slot
{
    // ISFULL CHECKS IF INVENTORY SLOT IS OCCUPIED
    public bool isFull;
    // INVENTORYSLOT IS FOR UI INVENTORY SLOT
    public GameObject inventorySlot;
    // COLORITEM IS FOR POTION PREFAB HOLDER
    public GameObject colorItem;
    // ColorMixer Object
    public ColorMixer colorMixer;
}
public class Inventory : MonoBehaviour
{
    [Header("Cleanse")]
    public GameObject cleanseUI;
    public Image cleanseFill;
    [Space]
    [Header("Inventory Slots")]
    // A Lis collection of the colors absorbed
    public List<Slot> inventorySlots = new List<Slot>();
    // FOR CLEANSE PROGRESS
    private float timeCheck;
    private float cleanseLength = 1.0f;
    private bool canCleanse = true;
    // PlayerSFXManager Instance reference - used for absorbed sfx
    private PlayerSFX_Manager playerSFX;
    // InventoryPool script component
    private InventoryPool inventoryPoolSc;

    //DEFAULT ANGELA COLORS
    private Color coatBaseColor;
    private GameObject coat;
    private Color emissionColor;

    private void Start()
    {
        // add delegate
        Gameplay_DelegateHandler.D_OnDeath += (c_onDeath) =>
        {
            Cleanse(FindObjectOfType<MainPlayerSc>());
        };
        if(inventoryPoolSc == null)
        {
            inventoryPoolSc = FindObjectOfType<InventoryPool>();
        }
        playerSFX = PlayerSFX_Manager.Instance;

        coat = GameObject.FindGameObjectWithTag("Player_Coat");
        coatBaseColor = coat.GetComponent<SkinnedMeshRenderer>().materials[6].color;
    }
    // FUNCTION CLEANSES WHOLE INVENTORY
    void Cleanse(MainPlayerSc mainPlayer)
    {
        // Cleanse every color GO in the color slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].colorItem != null)
            {
                // Destroy(inventorySlots[i].colorItem);
                this.inventoryPoolSc.itemPool.ReleasePoolable(inventorySlots[0].colorItem);
                inventorySlots[i].isFull = false;
                inventorySlots[i].colorItem = null;
                inventorySlots[i].colorMixer.color = Color.white;
                inventorySlots[i].colorMixer.color_code = ColorCode.BLACK;
            }
        }
        // Reset Color Slots properties
        coat = GameObject.FindGameObjectWithTag("Player_Coat");
        coat.GetComponent<SkinnedMeshRenderer>().materials[6].color = coatBaseColor;
        FindObjectOfType<MainPlayerSc>().playerLightSc.ConfigureFlashlightColor(Color.white);
        //coat.GetComponent<SkinnedMeshRenderer>().materials[6].DisableKeyword("_EMISSION");

        timeCheck = 0.0f;
        cleanseFill.fillAmount = 0.0f;
        canCleanse = false;
        isCleansing = false;
        

    }

    // FUNCTION IF COLOR COMBINATION IS DETECTED
    public void CheckColorCombination(ref MainPlayerSc mainPlayer, ColorMixer color)
    {
        // If the player have a previous color in its slot, then it's combine time
        if (inventorySlots[0].isFull == true)
        {
            // terminate the function if the same color is absorbed
            if (color.color_code == inventorySlots[0].colorMixer.color_code)
                return;
            //Debug.LogError($"Combine!!!");
            AssignColor(inventorySlots[0].colorMixer + color);
        }
        else // simply assigns the new color
        {
            AssignColor(color);
        }
    }
    // Borrows the requested object from the pool and assign the parent to the UI slot
    public void AssignColor(ColorMixer color)
    {

        // assigns the new color for the mainPlayer's skinMeshRenderer
        var coatColor = GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>();
        coatColor.materials[6].color = color.color;

        //assign emission to coat color
        //emissionColor = coatColor.materials[6].color;
        //coatColor.materials[6].SetColor("_EmissionColor", emissionColor);
        //coatColor.materials[6].EnableKeyword("_EMISSION");

        FindObjectOfType<MainPlayerSc>().playerLightSc.ConfigureFlashlightColor(color.color);

        // Release color UI ICON pool
        this.inventoryPoolSc.itemPool.ReleasePoolable(inventorySlots[0].colorItem);
        inventorySlots[0].isFull = true;
        // Request color UI ICON pool
        this.inventoryPoolSc.itemPool.RequestPoolable(color.color_code, inventorySlots[0].inventorySlot.transform);
        //this.inventoryPoolSc.setCurrentColorPosition(inventorySlots[0].inventorySlot.transform);
        inventorySlots[0].colorItem = 
            this.inventoryPoolSc.itemPool.usedObjects[this.inventoryPoolSc.itemPool.usedObjects.Count - 1];
        inventorySlots[0].colorMixer = color;
    }
    // PUBLIC FUNCTION FOR CHECKING IF INVENTORY IS FULL
    // CAN BE USED IN OTHER SCRIPTS FOR CHECKING INVENTORY STATUS
    public bool isInventoryFull()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].isFull != true)
                return false;
        }
        return true;
    }

    [HideInInspector] public bool isCleansing = false;

    public void update(MainPlayerSc mainPlayer)
    {
        // If Cleanse Key is Released
        if (Input.GetKeyUp(KeyCode.R) && !mainPlayer.PotionAbsorptionSC.isAbsorbing)
        {
            // stop the Cleanse SFX clip
            playerSFX.findSFXSourceByLabel("Cleanse").Stop();

            timeCheck = 0.0f;
            cleanseFill.fillAmount = 0.0f;
            isCleansing = false;
            canCleanse = true;
            cleanseUI.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.R) && canCleanse && !mainPlayer.PotionAbsorptionSC.isAbsorbing)
        {
            // Fire the Cleanse SFX clip
            playerSFX.findSFXSourceByLabel("Cleanse").Play();
        }
        // Cleanse Key is still on press
        else if (Input.GetKey(KeyCode.R) && canCleanse && !mainPlayer.PotionAbsorptionSC.isAbsorbing)
        {
            isCleansing = true;
            timeCheck += Time.deltaTime;
            cleanseUI.SetActive(true);
            cleanseFill.fillAmount = timeCheck / cleanseLength;

            if(timeCheck >= cleanseLength)
                Cleanse(mainPlayer);
        }
        // Cleanse Key is still on press, but there's nothing to cleanse
        else if(Input.GetKey(KeyCode.R) && !canCleanse)
        {
            cleanseUI.SetActive(false);
        }
    }
}
