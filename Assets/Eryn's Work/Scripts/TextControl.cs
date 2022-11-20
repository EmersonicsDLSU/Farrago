using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Cinemachine;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Drawing;
//using static UnityEngine.InputManagerEntry;
using System.Security.Cryptography;
using Unity.VisualScripting;

public class TextControl : MonoBehaviour
{
    public TMP_Text textPro;

    public static TextControl _instance;

    public int currentLevel = 0;
    public int currentTextLevel = 0;

    private float textFireDelay = 0.0f;
    private float idleTime = 0.0f;

    public Queue<string> levelMonologue;

    private Animator animator;
    private TimelineLevel TimelineLevel;

    private RespawnPointsHandler respawnHandler_ref;


    public enum textType
    {
        redLines = 0,
        yellowLines,
        orangeLines,
        puzzleBeaker,
        normalBeaker,
        levelIdle
    }


    private string[] redLines = 
        { "I’m red", 
        "Red liquid makes me red",
        "Red.", 
        "Now I’m red" };

    private string[] yellowLines =
        { "I’m yellow now!",
		"Yellow means yellow",
		"Yellow." };

    private string[] orangeLines =
    {
        "Yellow and red makes orange",
        "Now i’m orange",
        "What to do with this…",
        "Orange.",
        "Orange now"
    };

    /*
    private string[] correctSolution =
    {
        "Brilliant!",
        "I knew it",
        "I knew that was right",
        "That makes sense",
        "All that research pays off"
    };

    private string[] wrongSolution =
    {
        "That’s not it…",
        "That can’t be it…",
        "I don’t think this is working",
        "There has to be another way…",
        "Maybe… something else",
        "I’ll try something else",
        "Need to think of something better…",
        "I’m sure there’s a better solution",
        "This doesn’t make sense…",
        "This isn’t it",
        "I’m… guessing this isn’t right"
    };
    */

    private string[] puzzleBeaker =
    {
        "Colored liquid…",
	    "The liquid is so vibrant… almost pure…",
	    "I wonder what will happen if I drink it…",
	    "Another beaker",
	    "More liquid",
	    "This one is a different color this time",
	    "Is this what we were all working on?",
	    "Why do I feel the need to consume it…",
	    "Why do I think drinking it is the right idea…"
    };

    private string[] normalBeaker =
    {
        "Just a beaker",
        "Nothing important in this one",
        "This is a beaker",
        "Beaker.",
        "I’ve seen my fair share of beakers…",
        "Another beaker",
    };

    private string[] level1Idle =
    {
        "If I can just get to the other side of this room…",
        "I think there’s a window on the far right side of the room",
        "If I just keep exploring maybe i can find a way out",
        "I should just keep going right"
    };

    private string[] level2Idle =
    {
        "I need to get to that window over there",
		"The other side of the room leads to a lab I think"
    };

    private string[] level3Idle =
    {
        "Maybe I can get that fire to work somehow…",
	    "That key can probably open that door… if only I can get it out of that ice",
	    "There’s beakers filled with unknown liquid over here… maybe that can help me somehow…",
	    "How do I bring life back into this fire…"
    };

    private string[] level4Idle =
    {
        "I know this hallway",
        "The room I need is just on the other side of this hallway"
    };

    private string[] level5Idle =
    {
        "I just need to get to the other side",
        "The next room is all the way up there",
        "How do I get up there…"
    };

    private string[] level6Idle =
    {
        "The plants grow under the light…",

        "That rat looks dangerous",
		"How do I get over there.."
    };

    private string[] level7Idle =
    {
        "It’s coming back to me now",
		"My old room…",
		"Maybe I can find out more about this place"
    };

    public static TextControl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TextControl>();
                
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<TextControl>();
                }
            }
            
            return _instance;
        }
    }

    private void Start()
    {
        animator = textPro.gameObject.GetComponent<Animator>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();
        levelMonologue = new Queue<string>();
        respawnHandler_ref = GameObject.Find("RespawnTrigger").GetComponent<RespawnPointsHandler>();
    }

    void Update()
    {   
        //For firing level texts
        if (levelMonologue.Count != 0 && !TimelineLevel.isTimelinePlayed)
        {
            textFireDelay += Time.deltaTime;

            if (textFireDelay >= 6.0)
            {
                displayQueue();
            }
        }

        //For Idle Text
        else
        {
            idleTime += Time.deltaTime;

            if (idleTime >= 5.0f)
                Interact(textType.levelIdle);
        }
    }

    public void idleReset()
    {
        idleTime = 0;
    }

    public void delayReset()
    {
        textFireDelay = 0;
    }


    public void Interact(textType text)
    {
        if (text == textType.redLines)
            setText(redLines[Random.Range(0, redLines.Length - 1)]);
        else if (text == textType.yellowLines)
            setText(yellowLines[Random.Range(0, yellowLines.Length - 1)]);
        else if (text == textType.orangeLines)
            setText(orangeLines[Random.Range(0, orangeLines.Length - 1)]);
        else if (text == textType.puzzleBeaker)
            setText(puzzleBeaker[Random.Range(0, puzzleBeaker.Length - 1)]);
        else if (text == textType.normalBeaker)
            setText(normalBeaker[Random.Range(0, normalBeaker.Length - 1)]);
        else if (text == textType.levelIdle && levelMonologue.Count == 0)
            idleText();

    }

    public void idleText()
    {
        if (currentTextLevel == 1)
            setText(level1Idle[Random.Range(0, level1Idle.Length - 1)]);
        else if (currentTextLevel == 2)
            setText(level2Idle[Random.Range(0, level2Idle.Length - 1)]);
        else if (currentTextLevel == 3)
            setText(level3Idle[Random.Range(0, level3Idle.Length - 1)]);
        else if (currentTextLevel == 4)
            setText(level4Idle[Random.Range(0, level4Idle.Length - 1)]);
        else if (currentTextLevel == 5)
            setText(level5Idle[Random.Range(0, level5Idle.Length - 1)]);
        else if (currentTextLevel == 6)
            setText(level6Idle[Random.Range(0, level6Idle.Length - 1)]);
        else if (currentTextLevel == 7)
            setText(level7Idle[Random.Range(0, level7Idle.Length - 1)]);

        idleTime = 0;

        fireText();
    }

    public void setText(string text)
    {
        if (respawnHandler_ref.CurrentRespawnPoint == RespawnPoints.LEVEL7 || respawnHandler_ref.CurrentRespawnPoint == RespawnPoints.LEVEL8)
            textPro.text = "Angela: " + text;
        else
            textPro.text = "???: " + text;

        fireText();
    }

    public void fireText()
    {
        triggerTextAnimation();

        Invoke("unTriggerTextAnimation", 4.0f);
    }

    public void queueLevelText(string[] levelText, int level)
    {
        for (int i = 0; i < levelText.Length; i++)
        {
            levelMonologue.Enqueue(levelText[i]);
            currentLevel = level;
            currentTextLevel = level;
        }
    }

    public void displayQueue()
    {
        /*
        for (int i = 0; i < levelMonologue.Count; i++)
        {
            setText(levelMonologue.Dequeue());
        }
        */
        setText(levelMonologue.Dequeue());

        textFireDelay = 0;
    }

    public void clearQueue()
    {
        levelMonologue.Clear();
    }

    public void incrementLevel()
    {
        currentLevel++;
    }

    public void triggerTextAnimation()
    {
        animator.SetBool("isOpen", true);
    }

    public void unTriggerTextAnimation()
    {
        animator.SetBool("isOpen", false);
    }
}
