using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//ENUM CHOICE FOR COLOR
public enum ColorCode
{
    RED = 0,
    BLUE,
    YELLOW,
    ORANGE,
    VIOLET,
    WHITE,
    BLACK
};

// Class for ColoredPotion Absorption Mechanic
public class PotionAbsorption : MonoBehaviour
{
    //Player properties:
    [Header("Player Properties")]
    // Property class of 'Player'
    public PlayerProperty _playerProperty;
    // Interactable UI(Icon); Heads Up Display
    [HideInInspector]public GameObject interactableIcon;
    // Gameobject that holds the Color Tag
    [HideInInspector]public GameObject ColorInteractableGO;
    // Interactable UI(Circular Bar); Heads Up Display
    [HideInInspector]public Image interactableFillIcon;
    // reference to the gameObject's Object_ID instance; for monologue?
    [HideInInspector]public Object_ID object_ID;
    
    // Color.color reference for the recently absorbed color
    private Color hitTransformColor;
    // the accumulated time while absorbing a color
    private float timePress = 0.0f;
    // used in conditional expression for absorbing colors
    [HideInInspector]public bool canAbsorb = false;
    // used in conditional expression for absorbing colors
    [HideInInspector]public bool isAbsorbing = false;
    // rotation speed when absorbing
    [SerializeField]private float rotSpeed = 0.2f;
    // reference to the PlayerSFX_Manager Static Instance
    PlayerSFX_Manager playerSFX;
    // Potion Absorption Struct - contains the script properties
    public struct PAStruct
    {
        public PAStruct(PotionAbsorption potionAbsorptionSC)
        {
            this.potionAbsorptionSC = potionAbsorptionSC;
            this.playerInventory = null;
            this.interactAgain = true;
        }
        // reference to the Inventory instance from 'Player'
        public Inventory playerInventory;
        // used in conditional expression for absorbing colors
        public bool interactAgain;
        public PotionAbsorption potionAbsorptionSC;
    }
    
    // struct objects
    private PAStruct PAStruct_obj;
    
    // static DictionaryList of corresponding Color List
    public static Dictionary<ColorCode, Color> color_Code_To_UColor = new Dictionary<ColorCode, Color>();

    void Start()
    {
        // Struct Object allocation
        PAStruct_obj = new PAStruct(this);
        // reference to the Inventory instance from 'Player'
        PAStruct_obj.playerInventory = GameObject.FindGameObjectWithTag("PlayerScripts").GetComponent<Inventory>();
        // reference to the PlayerSFX_Manager Static Instance
        playerSFX = PlayerSFX_Manager.Instance;
        // assigns all the corresponding UColor for each color_code(enum)
        color_Code_To_UColor.Add(ColorCode.RED, Color.red);
        color_Code_To_UColor.Add(ColorCode.BLUE, Color.blue);
        color_Code_To_UColor.Add(ColorCode.YELLOW, Color.yellow);
        color_Code_To_UColor.Add(ColorCode.ORANGE, new Color(1f, 0.64f, 0f));
        color_Code_To_UColor.Add(ColorCode.VIOLET, new Color(0.50f, 0f, 1f));
        color_Code_To_UColor.Add(ColorCode.WHITE, Color.white);
        color_Code_To_UColor.Add(ColorCode.BLACK, Color.black);
    }

    // Update is called once per frame
    public void update(MainPlayerSc mainPlayer)
    {
        if (canAbsorb == true)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                // Reset some properties
                ResetProperties(ref mainPlayer, true);
            }
            else if (Input.GetKeyDown(KeyCode.E) && PAStruct_obj.interactAgain)
            {
                isAbsorbing = true;
                mainPlayer.playerAngelaAnim.IH_ConsumeAnim(ref mainPlayer, isAbsorbing);
                // clamp the rotation of the player to the angle where it would face the object in-front
                mainPlayer.playerMovementSc.ClampToObject(ref mainPlayer, this.ColorInteractableGO);
            }
            else if (Input.GetKey(KeyCode.E) && PAStruct_obj.interactAgain)
            {
                // increment time to 'timePress'
                timePress += Time.deltaTime * rotSpeed;
                // increment fill Image bar
                interactableFillIcon.fillAmount = timePress / 1.0f;

                // if UI fill is full
                if (interactableFillIcon.fillAmount == 1.0f)
                {
                    // Reset some properties
                    ResetProperties(ref mainPlayer, false);
                    // Absorb Animation with Delay
                    GameObject.Find("Inventory").GetComponent<Animator>().SetBool("isAbsorb", true);
                    playerSFX.findSFXSourceByLabel("Absorb").PlayOneShot(playerSFX.findSFXSourceByLabel("Absorb").clip);
                    Invoke("resetGrowAnim", 0.4f);
                    // Checks the Current tag(Color Tag) of this objects
                    /*
                     *NOTE: The passed ColorMixer object is coming from a new instantiated class obj which means that
                    we're always creating a copy of the color we want. This will have a slight effect on the performance
                    since instantiation time is repeated for each newly created obj. To resolve this, we should transform
                    this process to object pooling.
                     */
                    switch (ColorInteractableGO.transform.tag)
                    {
                        // These color cases are the colors that are available/seen in the world to be absorbed
                        case "RED POTION":
                            AssignColor(ref PAStruct_obj, new ColorMixer
                                (color_Code_To_UColor[ColorCode.RED], ColorCode.RED));
                            break;

                        case "YELLOW POTION":
                            AssignColor(ref PAStruct_obj, new ColorMixer
                                (color_Code_To_UColor[ColorCode.YELLOW], ColorCode.YELLOW));
                            break;

                        case "BLUE POTION":
                            AssignColor(ref PAStruct_obj, new ColorMixer
                                (color_Code_To_UColor[ColorCode.BLUE], ColorCode.BLUE));
                            break;
                    }
                    // call Monologue Text methods 
                    if (object_ID != null)
                        TextControl.textInstance.Interact(
                            (TextControl.textType)System.Enum.Parse(typeof(TextControl.textType),
                                object_ID.objectCode.ToString())
                            );
                    TextControl.textInstance.delayReset();
                }
            }

        }
        // Resets the timepress when the player left the area
        else
        {
            timePress = 0;
            PAStruct_obj.interactAgain = true;
        }
    }
    // Add Color Struct - an expansible struct for assigning color to the player 
    public void AssignColor(ref PAStruct PAStruct_obj, ColorMixer color)
    {
        PAStruct_obj.interactAgain = false;
        Debug.LogError($"Assigning COLOR!");
        // assigns the new color to the 'player_property'
        PAStruct_obj.potionAbsorptionSC._playerProperty.currentColor = color.color;
        // calls the method that checks if there's a combination in the colors
        // Call only if the Inventory slot is not vacant and the player recently absorbed a color
        // the colors will only be combined if the player released the absorb key
        // as well as the UI, it will only fade if the user released the absorb key
        var script = FindObjectOfType<MainPlayerSc>();
        PAStruct_obj.playerInventory.CheckColorCombination(ref script, color);
    }
    // Method that reset properties for ColorAbsorption mechanic
    private void ResetProperties(ref MainPlayerSc mainPlayer, bool isInteractAgain)
    {
        // Properties Reset
        timePress = 0;
        interactableFillIcon.fillAmount = 0.0f;
        PAStruct_obj.interactAgain = isInteractAgain;
        // Stops the animation of 'absorbing' if the player release the absorb key early(not fill up)
        isAbsorbing = false;
        mainPlayer.playerAngelaAnim.IH_ConsumeAnim(ref mainPlayer, isAbsorbing);

    }
    // this method is for the growAnimation
    private void resetGrowAnim()
    {
        GameObject.Find("Inventory").GetComponent<Animator>().SetBool("isAbsorb", false);
    }
}
