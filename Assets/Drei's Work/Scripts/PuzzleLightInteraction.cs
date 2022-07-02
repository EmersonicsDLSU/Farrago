using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpecialLightInteraction
{
    NONE = -1,
    L6_LEFT_WIRE,
    L6_RIGHT_WIRE,
    L6_LAMP_DESK
};

public class PuzzleLightInteraction : MonoBehaviour
{
    private QuestGiver questGiver;
    [SerializeField] private GameObject level5CutsceneTrigger;
    [SerializeField] private GameObject level6CutsceneTrigger;
    [SerializeField] private GameObject assignedVine;

    [SerializeField] private SpecialLightInteraction special_lightInteractionType;

    public bool isWireRepaired;
    public GameObject [] ratDestinationToRemove;
    public GameObject [] ratDestinationToAdd;
    private bool L6_hasVineReachedMaxLength = false;
    public GameObject Level6DeadTrigger;

    // Start is called before the first frame update
    void Start()
    {
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        GetComponent<Light>().enabled = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (questGiver.lastQuestDone.questID == questDescriptions.color_r5 && special_lightInteractionType == SpecialLightInteraction.NONE)
        {
            Debug.LogError($"None");
            GetComponent<Light>().enabled = true;
            level5CutsceneTrigger.SetActive(true);
        }

        else if (questGiver.currentQuest.questID == questDescriptions.color_r6)
        {
            //if the wire connected to the light is repaired, turn on light
            if (isWireRepaired == true)
            {
                GetComponent<Light>().enabled = true;
            }
            
            //if level 6 left wire is repaired
            if (getLightBool() == true && this.special_lightInteractionType == SpecialLightInteraction.L6_LEFT_WIRE)
            {
                //enable anim of correct vine length
                assignedVine.GetComponent<Animator>().SetBool("isLeftOn", true);
                Invoke("stopVineAnim", 3.0f);
            }

            //if level 6 right wire is repaired PROVIDED THAT left wire is repaired, nothing will happen if right wire is repaired first
            else if (getLightBool() == true && this.special_lightInteractionType == SpecialLightInteraction.L6_RIGHT_WIRE && isLeftWireRepaired() == true)
            {
                //enable vine anim to make vine reach max length
                if (L6_hasVineReachedMaxLength == false)
                {
                    enableVineAnim();
                    // only for rightWire
                    if (Level6DeadTrigger != null)
                    {
                        Level6DeadTrigger.SetActive(true);
                    }
                }
                assignedVine.GetComponent<Animator>().SetBool("isLeftOn", false);
                assignedVine.GetComponent<Animator>().SetBool("isRightOn", true);

                //delay stop of animation after 12 secs, weird, but this somehow works, pacheck nalang
                Invoke("stopVineAnim", 12.0f);
                Invoke("setL6_VineAtMax", 12.0f);
            }

            //if level 6 desk lamp is repaired
            else if (getLightBool() == true && this.special_lightInteractionType == SpecialLightInteraction.L6_LAMP_DESK)
            {
                // set the transform for the rat
                for (int i = 0; i < ratDestinationToAdd.Length; i++)
                {
                    ratDestinationToAdd[i].SetActive(true);
                }
                for (int i = 0; i < ratDestinationToRemove.Length; i++)
                {
                    ratDestinationToRemove[i].SetActive(false);
                }
                // Invoke anim
                assignedVine.GetComponent<Animator>().SetBool("willGrow", true);
                Invoke("stopVineAnim", 3.0f);
                questGiver.setQuestComplete();

                if (GameObject.Find("TimeLines").GetComponent<TimelineLevel>().lastPlayedSceneType == CutSceneTypes.Level6Transition)
                {
                    level6CutsceneTrigger.SetActive(false);
                }
                else
                {
                    level6CutsceneTrigger.SetActive(true);
                }
            }
        }
    }

    private bool getLightBool()
    {
        return GetComponent<Light>().enabled;
    }

    private bool isLeftWireRepaired()
    {
        return GameObject.FindGameObjectWithTag("L6_LeftLight").GetComponent<PuzzleLightInteraction>().isWireRepaired;
    }

    private void stopVineAnim()
    {
        assignedVine.GetComponent<Animator>().enabled = false;
    }

    private void enableVineAnim()
    {
        assignedVine.GetComponent<Animator>().enabled = true;
    }

    private void setL6_VineAtMax()
    {
        L6_hasVineReachedMaxLength = true;
    }

    
}
