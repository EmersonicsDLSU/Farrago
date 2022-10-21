using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHumanAnimations<T>
{
    public void IH_MoveAnim(ref T className);
    public void IH_RunAnim(ref T className);
    public void IH_SneakAnim(ref T className);
    public void IH_JumpAnim(ref T className);
    public void IH_IsGroundAnim(ref T className);
    public void IH_IsInteractAnim(ref T className);
    public void IH_DeathAnim(ref T className);
    public void IH_ConsumeAnim(ref T className, bool isConsuming);
}
