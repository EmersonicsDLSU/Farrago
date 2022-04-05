using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRatAnimations : MonoBehaviour, IEnemyAnimations <AIAgent>
{
    [SerializeField] private Animator enemyAnim;
    private void Start()
    {

    }

    public void remainingDistance(ref AIAgent agent)
    {
        enemyAnim.SetFloat("remainingDistance", agent.navMeshAgent.remainingDistance);
    }
    public void deadAnim(ref AIAgent agent)
    {

    }

    public void eatingAnim(ref AIAgent agent)
    {

    }

    public void idleAnim(ref AIAgent agent)
    {

    }

    public void runningAnim(ref AIAgent agent)
    {
        if(agent.enemyPatrol != null)
            enemyAnim.SetBool("isRun", agent.enemyPatrol.isRun);
        if(agent.enemyChaseSc != null)
            enemyAnim.SetBool("isRun", agent.enemyChaseSc.isRun);
    }

    public void walkingAnim(ref AIAgent agent)
    {
        if(agent.enemyPatrol != null)
            enemyAnim.SetBool("isWalk", agent.enemyPatrol.isWalk);
        if(agent.enemyChaseSc != null)
            enemyAnim.SetBool("isWalk", agent.enemyChaseSc.isWalk);
    }
}
