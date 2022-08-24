using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monologues : MonoBehaviour
{
    
    private static Monologues _instance;

    public static Monologues Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Monologues>();
                
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<Monologues>();
                }
            }
            
            return _instance;
        }
    }

    //FOR MONOLOGUES
    private List<string> characterResponsesCorrect = new List<string>();
    private List<string> characterResponsesIncorrect = new List<string>();
    
    private GameObject colorPuzzleUIText;

    // Start is called before the first frame update
    void Start()
    {
        //FOR MONOLOGUES
        colorPuzzleUIText = GameObject.Find("PuzzleInteractText");

        // Correct Interaction
        characterResponsesCorrect.Add("Brilliant!");
        characterResponsesCorrect.Add("I knew it");
        characterResponsesCorrect.Add("I knew that was right");
        characterResponsesCorrect.Add("That makes sense!");
        characterResponsesCorrect.Add("All that research pays off");
        // Incorrect Interaction
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

    }

    
    private void closePuzzleUITextIncorrect()
    {
        colorPuzzleUIText.GetComponent<Animator>().SetBool("isColorCorrect", true);
    }

    public void triggerPuzzleUITextIncorrect()
    {
        int randomMonologueHolder = Random.Range(0, characterResponsesIncorrect.Count - 1);

        colorPuzzleUIText.GetComponent<Text>().text = characterResponsesIncorrect[randomMonologueHolder];

        colorPuzzleUIText.GetComponent<Animator>().SetBool("isColorCorrect", false);
        Invoke("closePuzzleUITextIncorrect", 2.0f);
    }

    private void closePuzzleUITextCorrect()
    {
        colorPuzzleUIText.GetComponent<Animator>().SetBool("toTriggerCorrect", false);
    }

    public void triggerPuzzleUITextCorrect()
    {
        int randomMonologueHolder = Random.Range(0, characterResponsesCorrect.Count - 1);

        colorPuzzleUIText.GetComponent<Text>().text = characterResponsesCorrect[randomMonologueHolder];

        colorPuzzleUIText.GetComponent<Animator>().SetBool("toTriggerCorrect", true);
        Invoke("closePuzzleUITextCorrect", 2.0f);
    }
}
