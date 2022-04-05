using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//SLOTS HOLD PROPERTIES OF AN INVENTORY SLOT
[System.Serializable]
public class Slots
{
    //ISFULL CHECKS IF INVENTORY SLOT IS OCCUPIED
    public bool isFull;
    //INVENTORYSLOT IS FOR UI INVENTORY SLOT
    public GameObject inventorySlot;
    //COLORITEM IS FOR POTION PREFAB HOLDER
    public GameObject colorItem;
    // ColorMixer Object
    public ColorMixer color;
}
public class Inventory : MonoBehaviour
{
    [Header("Cleanse")]
    public GameObject cleanseUI;
    public Image cleanseFill;
    [Space]
    [Header("Inventory Slots")]
    // A Lis collection of the colors absorbed
    public List<Slots> inventorySlots = new List<Slots>();
    //FOR CLEANSE PROGRESS
    private float timeCheck;
    private float cleanseLength = 1.0f;
    private bool canCleanse = true;
    // PlayerSFXManager Instance reference - used for absorbed sfx
    private PlayerSFX_Manager playerSFX;
    //InventoryPool script component
    [SerializeField] private InventoryPool inventoryPoolSc;

    private void Start()
    {
        if(inventoryPoolSc == null)
        {
            inventoryPoolSc = FindObjectOfType<InventoryPool>();
        }
        playerSFX = PlayerSFX_Manager.Instance;
    }
    //FUNCTION CLEANSES WHOLE INVENTORY
    void Cleanse(MainPlayerSc mainPlayer)
    {
        // Cleanse every color GO in the color slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].colorItem != null)
            {
                //Destroy(inventorySlots[i].colorItem);
                this.inventoryPoolSc.itemPool.ReleasePoolable(inventorySlots[0].colorItem);
                inventorySlots[0].isFull = false;
                inventorySlots[0].colorItem = null;
            }

        }
        // Reset Color Slots properties
        mainPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.white;
        timeCheck = 0.0f;
        cleanseFill.fillAmount = 0.0f;
        canCleanse = false;
        // Fire the Cleanse SFX clip
        playerSFX.findSFXSourceByLabel("Cleanse").PlayOneShot(playerSFX.findSFXSourceByLabel("Cleanse").clip);

    }

    //FUNCTION IF COLOR COMBINATION IS DETECTED
    public void CheckColorCombination(ref MainPlayerSc mainPlayer, ColorMixer color)
    {
        // If the player have a previous color in its slot, then it's combine time
        if (inventorySlots[0].isFull == true)
        {
            assignColor(inventorySlots[0].color + color);
        }
        else // simply assigns the new color
        {
            assignColor(color);
        }
    }
    //Borrows the requested object from the pool and assign the parent to the UI slot
    public void assignColor(ColorMixer color)
    {
        // assigns the new color for the mainPlayer's skinMeshRenderer
        var playerSkinColor = GameObject.FindGameObjectWithTag("Player").
            GetComponentInChildren<SkinnedMeshRenderer>();
        playerSkinColor.material.color = color.color;
        // Release color UI ICON pool
        this.inventoryPoolSc.itemPool.ReleasePoolable(inventorySlots[0].colorItem);
        inventorySlots[0].isFull = true;
        // Request color UI ICON pool
        Debug.LogError($"Color IS: {color.color_code}");
        this.inventoryPoolSc.itemPool.RequestPoolable(color.color_code);
        this.inventoryPoolSc.setCurrentColorPosition(inventorySlots[0].inventorySlot.transform);
        inventorySlots[0].colorItem = 
            this.inventoryPoolSc.itemPool.usedObjects[this.inventoryPoolSc.itemPool.usedObjects.Count - 1];
        inventorySlots[0].color = color;
    }
    //PUBLIC FUNCTION FOR CHECKING IF INVENTORY IS FULL
    //CAN BE USED IN OTHER SCRIPTS FOR CHECKING INVENTORY STATUS
    public bool isInventoryFull()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].isFull != true)
                return false;
        }
        return true;
    }

    public void update(MainPlayerSc mainPlayer)
    {
        // If Cleanse Key is Released
        if (Input.GetKeyUp(KeyCode.R))
        {
            timeCheck = 0.0f;
            cleanseFill.fillAmount = 0.0f;
            canCleanse = true;
            cleanseUI.SetActive(false);
        }
        // Cleanse Key is still on press
        else if (Input.GetKey(KeyCode.R) && canCleanse)
        {
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
