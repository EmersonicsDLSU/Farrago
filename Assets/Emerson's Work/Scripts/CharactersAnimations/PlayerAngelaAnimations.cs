using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAngelaAnimations : MonoBehaviour, IHumanAnimations<MainPlayerSc>
{
    public void IH_MoveAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetFloat("playerMove", 
            Mathf.Abs(mainPlayer.playerMovementSc.MovementX) + Mathf.Abs(mainPlayer.playerMovementSc.MovementY));
    }
    public void IH_RunAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isRun", mainPlayer.playerMovementSc._playerProperty.isRun);
    }

    public void IH_SneakAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isSneak", mainPlayer.playerMovementSc._playerProperty.isSneak);
    }

    public void IH_JumpAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isJump", mainPlayer.playerMovementSc._playerProperty.isJump);
    }

    public void IH_IsGroundAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isGround", mainPlayer.playerMovementSc._playerProperty.isGround);
    }
    public void IH_IsInteractAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isInteract", mainPlayer.playerMovementSc._playerProperty.isInteract);
    }

    public void IH_DeathAnim(ref MainPlayerSc mainPlayer)
    {
        mainPlayer.playerAnim.SetBool("isDead", mainPlayer.playerMovementSc._playerProperty.isDead);
    }

    public void IH_ConsumeAnim(ref MainPlayerSc mainPlayer, bool isConsuming)
    {
        mainPlayer.playerAnim.SetBool("isConsume", isConsuming);
    }
}
