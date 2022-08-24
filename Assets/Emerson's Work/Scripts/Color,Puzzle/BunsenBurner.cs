using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BunsenBurner : PuzzleItemInteraction
{
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    public Animator anim;
    private GameObject colorPuzzleUIText;
    
    // Start is called before the first frame update
    void Start()
    {
        ma = ParticleSystem.main;
        
        //FOR MONOLOGUES
        colorPuzzleUIText = GameObject.Find("PuzzleInteractText");

        anim.SetBool("will fade", false);

        //DISABLE FIRE PARTICLE SYSTEM
        if(ParticleSystem.gameObject.transform.parent.tag == "Interactable Fire")
            ParticleSystem.Stop();
    }
    
    public override void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_R3_OnCompletedFire += (c_onCompletedFire) =>
        {
            // Check if color is correct
            if (FindObjectOfType<Inventory>().inventorySlots[0].color.color_code == ColorCode.RED)
            {
                // set the fire objective as completed
                QuestCollection.Instance.questDict[QuestDescriptions.tutorial_color_r3]
                    .descriptiveObjectives[DescriptiveQuest.R3_COMPLETED_FIRE] = true;
                // Update the objectiveList as well; double update 
                FindObjectOfType<ObjectivePool>().itemPool.ReleaseAllPoolable();
                FindObjectOfType<QuestGiver>().UpdateObjectiveList();
                FindObjectOfType<ObjectivePool>().EnabledAnimation(true);
            
                // Check if all objectives are completed
                if (FindObjectOfType<QuestGiver>().currentQuest != null &&
                    QuestCollection.Instance.questDict[FindObjectOfType<QuestGiver>().currentQuest.questID].descriptiveObjectives.Values.All(e => e == true))
                {
                    FindObjectOfType<QuestGiver>().currentQuest.neededGameObjects.Clear();
                }
                
                //TRIGGER CORRECT MONOLOGUE
                Monologues.Instance.triggerPuzzleUITextCorrect();
                
                //CHANGE FIRE COLOR
                ParticleSystem.Play();
                ParticleSystem.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                ma.startColor = GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color;

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
        };
    }
    
}
