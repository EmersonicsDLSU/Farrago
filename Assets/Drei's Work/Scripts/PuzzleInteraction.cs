using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PuzzleInteraction : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private ParticleSystem ParticleSystem;
    private ParticleSystem.MainModule ma;
    private ParticleSystem.MainModule subEmitter;
    private ParticleSystem.TrailModule tr;
    public PlayerProperty _playerProperty;
    private Color colorToAssign;
    
    
    public Animator anim;

    [Space]
    [Header("Interactables")]
    public GameObject interactableParent;
    public Image interactableFill;

    [HideInInspector]public bool canInteract = false;
    private bool isColorCorrect = false;
    private float timePress;
    private bool interactAgain = true;

    private GameObject colorPuzzleUIText;
    
    //FOR MONOLOGUES
    private List<string> characterResponsesCorrect = new List<string>();
    private List<string> characterResponsesIncorrect = new List<string>();
    private int randomMonologueHolder;

    private MainPlayerSc mainPlayer;


    void Awake()
    {
        characterResponsesCorrect.Add("Brilliant!");
        characterResponsesCorrect.Add("I knew it");
        characterResponsesCorrect.Add("I knew that was right");
        characterResponsesCorrect.Add("That makes sense!");
        characterResponsesCorrect.Add("All that research pays off");

        characterResponsesIncorrect.Add("That’s not it…");
        characterResponsesIncorrect.Add("That can’t be it…");
        characterResponsesIncorrect.Add("I don’t think this is working");
        characterResponsesIncorrect.Add("There has to be another way…");
        characterResponsesIncorrect.Add("Maybe… something else");
        characterResponsesIncorrect.Add("I’ll try something else");
        characterResponsesIncorrect.Add("Need to think of something better…");
        characterResponsesIncorrect.Add("I’m sure there’s a better solution");
        characterResponsesIncorrect.Add("This doesn’t make sense…");
        characterResponsesIncorrect.Add("This isn’t it");
        characterResponsesIncorrect.Add("I’m… guessing this isn’t right");

        ma = ParticleSystem.main;
        tr = ParticleSystem.trails;

        mainPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerSc>();

        //FOR MONOLOGUES
        colorPuzzleUIText = GameObject.Find("PuzzleInteractText");

    }

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("will fade", false);

        //DISABLE FIRE PARTICLE SYSTEM
        if(ParticleSystem.gameObject.transform.parent.tag == "Interactable Fire")
            ParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract == true)
        {
            interactableParent.SetActive(true);
            if (Input.GetKeyUp(KeyCode.E))
            {
                timePress = 0;
                interactableFill.fillAmount = 0.0f;
                interactAgain = true;
            }
            if (Input.GetKey(KeyCode.E) && interactAgain)
            {
                mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.gameObject);
                timePress += Time.deltaTime;
                interactableFill.fillAmount = timePress / 2.0f;

                if (interactableFill.fillAmount == 1.0f)
                {
                    Debug.Log($"Hitted obj: {this.transform.name}");
                    /*GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>().material.color =
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SkinnedMeshRenderer>().material.color;*/
                    switch (this.transform.tag)
                    {
                        case "Interactable Fire":
                            isColorCorrect = colorChecker(this.transform.tag, GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color);
                            Debug.Log("is color correct: " + isColorCorrect);

                            if (isColorCorrect == true)
                            {
                                //MARK OBJECTIVE AS COMPLETE
                                GameObject.Find("QuestGiver").GetComponent<QuestGiver>().completedObjectives.Add("completeFire");
                                GameObject.Find("QuestGiver").GetComponent<QuestGiver>().strikethroughTextByKey("completeFire");

                                //TRIGGER CORRECT MONOLOGUE
                                triggerPuzzleUITextCorrect();

                                //CHANGE FIRE COLOR
                                ParticleSystem.Play();
                                ParticleSystem.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                                ma.startColor = GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color;
                                anim.SetBool("will fade", true);
                                anim.gameObject.GetComponent<BoxCollider>().enabled = false;
                                if (!anim.gameObject.GetComponent<AudioSource>().isPlaying)
                                    anim.gameObject.GetComponent<AudioSource>().Play();
                            }
                            else
                            {
                                //TRIGGER INCORRECT MONOLOGUE
                                triggerPuzzleUITextIncorrect();
                            }
                            break;

                        case "Interactable Electricity":
                            isColorCorrect = colorChecker(this.transform.tag, GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color);
                            Debug.Log("is color correct: " + isColorCorrect);

                            if (isColorCorrect == true)
                            {
                                colorToAssign = GameObject.FindGameObjectWithTag("Player_Coat").GetComponent<SkinnedMeshRenderer>().material.color;

                                //MARK OBJECTIVE AS COMPLETE
                                GameObject.Find("QuestGiver").GetComponent<QuestGiver>().currentQuest.wiresRepairedAmount++;

                                if (GameObject.Find("QuestGiver").GetComponent<QuestGiver>().currentQuest.wiresRepairedAmount == GameObject.Find("QuestGiver").GetComponent<QuestGiver>().currentQuest.wiresToRepairAmount)
                                {
                                    GameObject.Find("QuestGiver").GetComponent<QuestGiver>().completedObjectives.Add("repairWire");
                                    GameObject.Find("QuestGiver").GetComponent<QuestGiver>().strikethroughTextByKey("repairWire");

                                    // ------ FOR LEVEL 5 TEMP CODE, DELETE SOON -----------
                                    GameObject.Find("QuestGiver").GetComponent<QuestGiver>().completedObjectives.Add("onLight");
                                    GameObject.Find("QuestGiver").GetComponent<QuestGiver>().strikethroughTextByKey("onLight");
                                    // -----------------------------------------------------
                                }


                                //TRIGGER CORRECT MONOLOGUE
                                triggerPuzzleUITextCorrect();

                                //CHANGE ELECTRICITY COLOR
                                ma.startColor = colorToAssign;
                                tr.colorOverLifetime = colorToAssign;
                                ParticleSystem.GetComponent<Renderer>().materials[1].color = colorToAssign;
                                subEmitter = ParticleSystem.subEmitters.GetSubEmitterSystem(0).main;
                                subEmitter.startColor = colorToAssign;

                                //if (!anim.gameObject.GetComponent<AudioSource>().isPlaying)
                                //anim.gameObject.GetComponent<AudioSource>().Play();
                            }
                            else
                            {
                                //TRIGGER INCORRECT MONOLOGUE
                                triggerPuzzleUITextIncorrect();
                            }
                            break;

                    }

                    timePress = 0;
                    interactableFill.fillAmount = 0.0f;
                    interactAgain = false;
                }
            }
        }
        else if (canInteract == false)
        {
            timePress = 0;
            interactableFill.fillAmount = 0.0f;
            interactableParent.SetActive(false);
        }
    }

    bool colorChecker(string interactableType, Color characterCurrColor)
    {
        float H, S, V;
        if (interactableType == "Interactable Fire")
        {
            Color.RGBToHSV(characterCurrColor, out H, out S, out V);
            Debug.Log("H: " + H + " S: " + S + " V: " + V);

            if ((H >= 16.0f/360.0f && H <= 48.0f/360.0f) && (S >= 34.0f/100.0f && S <= 100.0f/100.0f) && (V >= 58.0f/100.0f && V <= 100.0f/100.0f))
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
        return false;
    }

    private void closePuzzleUITextIncorrect()
    {
        colorPuzzleUIText.GetComponent<Animator>().SetBool("isColorCorrect", true);
    }

    private void triggerPuzzleUITextIncorrect()
    {
        randomMonologueHolder = Random.Range(0, characterResponsesIncorrect.Count - 1);

        colorPuzzleUIText.GetComponent<Text>().text = characterResponsesIncorrect[randomMonologueHolder];

        colorPuzzleUIText.GetComponent<Animator>().SetBool("isColorCorrect", false);
        Invoke("closePuzzleUITextIncorrect", 2.0f);
    }

    private void closePuzzleUITextCorrect()
    {
        colorPuzzleUIText.GetComponent<Animator>().SetBool("toTriggerCorrect", false);
    }

    private void triggerPuzzleUITextCorrect()
    {
        randomMonologueHolder = Random.Range(0, characterResponsesCorrect.Count - 1);

        colorPuzzleUIText.GetComponent<Text>().text = characterResponsesCorrect[randomMonologueHolder];

        colorPuzzleUIText.GetComponent<Animator>().SetBool("toTriggerCorrect", true);
        Invoke("closePuzzleUITextCorrect", 2.0f);
    }
}
