using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private ButtonActionManager buttonActionManagerRef;
    private float holdTicks = 0.0f;
    private MainPlayerSc mainPlayer;

    // Start is called before the first frame update
    void Start()
    {
        buttonActionManagerRef = ButtonActionManager.Instance;
        mainPlayer = FindObjectOfType<MainPlayerSc>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject lastPressedButton;
        lastPressedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        //IS INTERACT HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.interactButton.gameObject)
        {
            buttonActionManagerRef.isInteractHeldDown = true;
            
            if(mainPlayer.PotionAbsorptionSC.canAbsorb == true)
                mainPlayer.PotionAbsorptionSC.GetEKeyDown(mainPlayer);
        }

        //IS RUN HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.runButton.gameObject)
        {
            buttonActionManagerRef.isRunHeldDown = true;
            mainPlayer.playerMovementSc.GetKeyDownRun(mainPlayer);
        }

        //IS SNEAK HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.sneakButton.gameObject)
        {
            buttonActionManagerRef.isSneakHeldDown = true;
            mainPlayer.playerMovementSc.GetKeyUpSneak(mainPlayer);
            
        }

        //IS CLEANSE HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.cleanseButton.gameObject)
        {
            buttonActionManagerRef.isCleanseHeldDown = true;
            
        }

        //IS JUMP PRESSED
        if (lastPressedButton == buttonActionManagerRef.jumpButton.gameObject)
        {
            buttonActionManagerRef.isJumpPressed = true;
            mainPlayer.playerMovementSc.GetKeyDownJump();
        }

        //IS JOURNAL PRESSED
        if (lastPressedButton == buttonActionManagerRef.journalButton.gameObject)
        {
            buttonActionManagerRef.isJournalPressed = true;
        }

        //IS PAUSE PRESSED
        if (lastPressedButton == buttonActionManagerRef.pauseButton.gameObject)
        {
            buttonActionManagerRef.isPausePressed = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject lastPressedButton;
        lastPressedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        //IS INTERACT HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.interactButton.gameObject)
        {
            buttonActionManagerRef.isInteractHeldDown = false;

            if (mainPlayer.PotionAbsorptionSC.canAbsorb == true)
                mainPlayer.PotionAbsorptionSC.GetEKeyUp(mainPlayer);
        }

        //IS RUN HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.runButton.gameObject)
        {
            buttonActionManagerRef.isRunHeldDown = false;
            mainPlayer.playerMovementSc.GetKeyUpRun(mainPlayer);
        }

        //IS SNEAK HELD DOWN
        if (lastPressedButton == buttonActionManagerRef.sneakButton.gameObject)
        {
            buttonActionManagerRef.isSneakHeldDown = false;
            mainPlayer.playerMovementSc.GetKeyDownSneak(mainPlayer);
        }

        //IS CLEANSE HELD DOWN
        if (lastPressedButton== buttonActionManagerRef.cleanseButton.gameObject)
        {
            buttonActionManagerRef.isCleanseHeldDown = false;
        }
    }

}
