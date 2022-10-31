using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class R8_BunsenBurner : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    public Animator anim;

    // References
    private Inventory inventory;
    private ObjectivePool objectivePool;
    private QuestGiver questGiver;
    
    public override void OAwake()
    {
        // set the item identification
        Item_Identification = PuzzleItem.R8_BUNSEN_BURNER;

        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"Missing Script: Inventory.cs");
        }
        
        objectivePool = FindObjectOfType<ObjectivePool>();
        if (objectivePool == null)
        {
            Debug.LogError($"Missing Script: ObjectivePool.cs");
        }

        questGiver = FindObjectOfType<QuestGiver>();
        if (questGiver == null)
        {
            Debug.LogError($"Missing Script: QuestGiver.cs");
        }

        ma = ParticleSystem.main;
        
        anim.SetBool("will fade", false);

        //DISABLE FIRE PARTICLE SYSTEM
        if(ParticleSystem.gameObject.transform.parent.tag == "Interactable Fire")
        {
            ParticleSystem.Stop();
            ParticleSystem.gameObject.transform.GetChild(4).gameObject.SetActive(false);
        }
            
    }

    // Subscribe event should only be called once to avoid duplication
    public override void ODelegates()
    {
        D_Item += Event1;
    }

    public void OnDestroy()
    {
        D_Item -= Event1;
    }
    
    private void Event1(C_Item e)
    {
        // Check if color is correct
        if (inventory.inventorySlots[0].colorMixer.color_code == ColorCode.ORANGE)
        {
            // disables the interactable UI
            interactableParent.SetActive(false);
            isActive = false;

            /* Start of Objective Completion / Setting strikethrough to the text's fontStyle*/
            // set the fire objective as completed
            //QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3]
            //   .descriptiveObjectives[DescriptiveQuest.R3_COMPLETED_FIRE] = true;

            // Update the objectiveList as well; double update 
            //objectivePool.itemPool.ReleaseAllPoolable();
            //questGiver.UpdateObjectiveList();

            // Check if all objectives are completed
            //if (questGiver.currentQuest != null && QuestCollection.Instance.questDict[questGiver.currentQuest.questID].
            //        descriptiveObjectives.Values.All(e => e == true))
            //{
            //    questGiver.currentQuest.neededGameObjects.Clear();
            //}
            /* End of Objective Completion */

            //TRIGGER CORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextCorrect();
                
            //CHANGE FIRE COLOR
            ParticleSystem.Play();
            ParticleSystem.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            ParticleSystem.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            ParticleSystem.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            ParticleSystem.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            ParticleSystem.gameObject.transform.GetChild(4).gameObject.SetActive(true);

            ParticleSystem.ColorOverLifetimeModule colMod0;
            ParticleSystem.ColorOverLifetimeModule colMod1;
            ParticleSystem.ColorOverLifetimeModule colMod2;
            ParticleSystem.ColorOverLifetimeModule colMod3;
            colMod0 = ParticleSystem.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
            colMod1 = ParticleSystem.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime;
            colMod2 = ParticleSystem.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime;
            colMod3 = ParticleSystem.gameObject.transform.GetChild(3).GetComponent<ParticleSystem>().colorOverLifetime;
            colMod0.color = inventory.inventorySlots[0].colorMixer.color;
            colMod1.color = inventory.inventorySlots[0].colorMixer.color;
            colMod2.color = inventory.inventorySlots[0].colorMixer.color;
            colMod3.color = inventory.inventorySlots[0].colorMixer.color;


            //FOR ICE ANIM
            if (anim != null)
            {
                anim.SetBool("will fade", true);
                anim.gameObject.GetComponent<BoxCollider>().enabled = false;
                if (!anim.gameObject.GetComponent<AudioSource>().isPlaying)
                    anim.gameObject.GetComponent<AudioSource>().Play();
            }
                               
        }
        else
        {
            //TRIGGER INCORRECT MONOLOGUE
            Monologues.Instance.triggerPuzzleUITextIncorrect();
        }
    }

    public override void OLoadData(GameData data)
    {
        // disables the interactable UI
        interactableParent.SetActive(false);
        isActive = false;

        /* Start of Objective Completion / Setting strikethrough to the text's fontStyle*/
        // set the fire objective as completed
        //QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3]
        //    .descriptiveObjectives[DescriptiveQuest.R3_COMPLETED_FIRE] = true;
        
        // Check if all objectives are completed
        //if (questGiver.currentQuest != null && QuestCollection.Instance.questDict[questGiver.currentQuest.questID].
        //        descriptiveObjectives.Values.All(e => e == true))
        //{
        //    questGiver.currentQuest.neededGameObjects.Clear();
        //}
        /* End of Objective Completion */
        
        //CHANGE FIRE COLOR
        ParticleSystem.Play();
        ParticleSystem.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        ParticleSystem.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        ParticleSystem.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        ParticleSystem.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        ParticleSystem.gameObject.transform.GetChild(4).gameObject.SetActive(true);

        ParticleSystem.ColorOverLifetimeModule colMod0;
        ParticleSystem.ColorOverLifetimeModule colMod1;
        ParticleSystem.ColorOverLifetimeModule colMod2;
        ParticleSystem.ColorOverLifetimeModule colMod3;
        colMod0 = ParticleSystem.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime;
        colMod1 = ParticleSystem.gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime;
        colMod2 = ParticleSystem.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime;
        colMod3 = ParticleSystem.gameObject.transform.GetChild(3).GetComponent<ParticleSystem>().colorOverLifetime;
        colMod0.color = inventory.inventorySlots[0].colorMixer.color;
        colMod1.color = inventory.inventorySlots[0].colorMixer.color;
        colMod2.color = inventory.inventorySlots[0].colorMixer.color;
        colMod3.color = inventory.inventorySlots[0].colorMixer.color;

        //FOR ICE ANIM
        if (anim != null)
        {
            anim.SetBool("will fade", true);
            anim.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
