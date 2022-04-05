using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using Cinemachine;
using TMPro;

public class TextControl : MonoBehaviour
{
    public TextMesh textHolder;
    public TMP_Text textPro;

    public static TextControl textInstance;

    public Camera camRef;

    private CinemachineVirtualCamera CVCam;
    private CinemachineBrain cinemachineBrain;

    private float defaultCharSize = 0.05f;

    public int currentLevel = 0;
    public int currentTextLevel = 0;

    private float textFireDelay = 0.0f;
    private float idleTime = 0.0f;

    public Queue<string> levelMonologue;

    public Animator animator;
    private TimelineLevel TimelineLevel;


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

    private void Awake()
    {
        if (textInstance != null && textInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        textInstance = this;
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        animator = GameObject.Find("TextCanvas").GetComponentInChildren<Animator>();
        TimelineLevel = GameObject.Find("TimeLines").GetComponent<TimelineLevel>();
        levelMonologue = new Queue<string>();
        cinemachineBrain = camRef.GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        try
        {
            CVCam = (CinemachineVirtualCamera)this.cinemachineBrain.ActiveVirtualCamera;
            if (CVCam.GetCinemachineComponent<CinemachineFramingTransposer>() != null)
            {
                textHolder.gameObject.SetActive(true);
                textHolder.GetComponentInParent<Transform>().rotation = CVCam.transform.rotation;
                var test1 = CVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                var test2 = CVCam.m_Lens;
                //Debug.LogError($"LIVE VCAM Distance: {test1.m_CameraDistance}");
                textHolder.characterSize = defaultCharSize * (test1.m_CameraDistance / 5.0f) * (test2.FieldOfView / 60) * 0.75f;
            }
            else if (CVCam.GetCinemachineComponent<CinemachineComposer>() != null)
            {
                textHolder.gameObject.SetActive(true);
                var test1 = CVCam.GetCinemachineComponent<CinemachineComposer>();
                textHolder.GetComponentInParent<Transform>().rotation = CVCam.transform.rotation; 
                textHolder.characterSize = defaultCharSize * (this.transform.position.x - CVCam.transform.position.x) * 0.3f;
            }
            else
            {
                textHolder.gameObject.SetActive(false);
                textHolder.characterSize = defaultCharSize;
            }
            
        }
        catch
        {
            //Debug.LogError($"Error In: {this.transform.name}");
        }
        
        /*
        CVCam = (CinemachineVirtualCamera)this.cinemachineBrain.ActiveVirtualCamera;
        var test1 = CVCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        var test2 = CVCam.m_Lens;
        //Debug.LogError($"LIVE VCAM Distance: {test1.m_CameraDistance}");
        textHolder.characterSize = defaultCharSize * (test1.m_CameraDistance / 5.0f) * (test2.FieldOfView / 60) * 0.75f;
        */

        //Debug.Log(levelMonologue.Count);
        if (levelMonologue.Count != 0 && !TimelineLevel.isTimelinePlayed)
        {
            textFireDelay += Time.deltaTime;

            //Debug.LogWarning(textFireDelay);

            if (textFireDelay >= 4.0)
            {
                displayQueue();
            }
        }

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
        else
            Debug.Log("None");

        idleTime = 0;

        fireText();
    }

    public void setText(string text)
    {
        textHolder.text = text;

        fireText();
    }

    public void fireText()
    {
        triggerTextAnimation();

        Invoke("unTriggerTextAnimation", 2.0f);
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
        for (int i = 0; i < levelMonologue.Count; i++)
        {
            setText(levelMonologue.Dequeue());
        }

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
