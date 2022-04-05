using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ChaseRatBehavior : MonoBehaviour
{
    public EnemyProperty enemy_property;

    [HideInInspector] public bool isWalk = false;
    [HideInInspector] public bool isRun = false;

    //For the LineRenderer Display
    private LineRenderer lineRenderer;
    private List<Vector3> pointsList;

    // Start is called before the first frame update
    void Start()
    {
        //assignDestinations();
        lineRenderer = GetComponent<LineRenderer>();
        pointsList = new List<Vector3>();
    }
    
    //Display Line Renderer for Rat Pathfinding debug
    public void DisplayLineDestinations(ref AIAgent agent)
    {
        //test
        if (agent.navMeshAgent.path.corners.Length < 2) return;

        int i = 1;
        while (i < agent.navMeshAgent.path.corners.Length)
        {
            lineRenderer.positionCount = agent.navMeshAgent.path.corners.Length;
            pointsList = agent.navMeshAgent.path.corners.ToList();
            for (int j = 0; j < pointsList.Count; j++)
            {
                lineRenderer.SetPosition(j, pointsList[j]);
            }

            i++;
        }
        //test
    }
    

    private bool isOnChase = false;

    // Update is called once per frame
    public void update(AIAgent agent)
    {
        //updates the lineRenderer
        //DisplayLineDestinations(ref agent);
        
        agent.navMeshAgent.SetDestination(agent.mainPlayerSc.transform.position);
        //run animation
        this.isRun = true;
        agent.agentAnimFunc.remainingDistance(ref agent);
        agent.agentAnimFunc.walkingAnim(ref agent);
        agent.agentAnimFunc.runningAnim(ref agent);
    }

    [SerializeField]
    protected float debugDrawRadius = 1.0f;

    
    //Draws a gizmos for the Agent
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
    
}
