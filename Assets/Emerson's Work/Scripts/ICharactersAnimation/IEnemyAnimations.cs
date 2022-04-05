using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAnimations<T>
{
    public void remainingDistance(ref T agent);
    public void deadAnim(ref T agent);

    public void eatingAnim(ref T agent);

    public void idleAnim(ref T agent);

    public void runningAnim(ref T agent);

    public void walkingAnim(ref T agent);
}
