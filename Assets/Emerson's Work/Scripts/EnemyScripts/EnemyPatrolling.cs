using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolling : MonoBehaviour
{
    [HideInInspector] public List <Transform> destinations = new List<Transform>();
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
    
    //should be called when then enemy is placed again in the world(poolable obj)
    public void assignDestinations(GameObject destinationSet)
    {
        /* //old code
        //Where it assigns the next transform point goal of the AI
        string destinationsName = "RatDestination1";
        GameObject destinationSet = GameObject.Find(destinationsName);
        */
        //clear up list for the new destinations
        if (this.destinations.Count != 0)
        {
            this.destinations.Clear();
        }

        for (int i = 0; i < destinationSet.transform.childCount; i++)
        {
            if (destinationSet.transform.GetChild(i).gameObject.activeSelf)
                destinations.Add(destinationSet.transform.GetChild(i));
        }
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

        //checks if the agent found the goal / target
        if(pickup == null)
        {
            pickup = FindPickup(agent);
            //if it found the goal / target
            if(pickup != null)
            {
                CollectPickUp(agent);
                //Debug.Log($"Chasing Speed: {agent.navMeshAgent.speed}");
                this.isOnChase = true;
            }
            //if its not found
            else
            {
                //enemy set to walking speed again
                agent.navMeshAgent.speed = enemy_property.walkSpeed;
                //Debug.Log($"Chasing Speed: {agent.navMeshAgent.speed}");
                this.isOnChase = false;
            }
            //refresh the pickup identity
            pickup = null;
        }

        if(!isOnChase)
        {
            //patrolling
            //constantly updating the remaining distance
            agent.agentAnimFunc.remainingDistance(ref agent);
            if (this.readyToChange)
            {
                SetDestination(agent);
            }
            else
            {
                checkIfArrived(agent);
            }
        }

        /*
        //If enemy chases player; checks the distance between the goal and enemy
        if (isOnChase)
        {
            if (this.checkIfCaptured(agent))
            {
                //Put here end condition
                Debug.LogWarning("Captured!");

                GameObject.Find("InGameHUD").GetComponent<HUD_Controller>().On_Lose();
                //GameObject.FindObjectOfType<HUD_Controller>().On_Lose();
                //Loader.loadinstance.LoadLevel(0);
            }
        }
        */
    }

    private bool onTransition = false;
    private bool readyToChange = true;
    private int currentIndex = -1;
    //function to set the target transform of the AI
    private void SetDestination(AIAgent agent)
    {
        //randomnly move to another destination
        int index = Random.Range(0, destinations.Count);
        //reroll if the random pick is still the previous / current destination
        while (index == currentIndex)
        {
            index = Random.Range(0, destinations.Count);
        }
        currentIndex = index;

        if (this.destinations[index] != null)
        {
            Vector3 targetVector = this.destinations[index].transform.position;
            //checks the validity of the set goal
            if(agent.navMeshAgent.SetDestination(targetVector))
            {
                onTransition = true;
                this.readyToChange = false;
            }
        }

        //Debug.LogError($"Current index: {currentIndex}");
    }

    private float distance_offset = 1.0f;
    [SerializeField]private float max_travel_Time = 10.0f;
    private float travel_tick = 0.0f;
    private void checkIfArrived(AIAgent agent)
    {
        this.travel_tick += Time.deltaTime;

        if(agent.navMeshAgent.remainingDistance <= distance_offset)
        {
            this.isWalk = false;
            this.isRun = false;
            //turns the character to idle animation
            agent.agentAnimFunc.walkingAnim(ref agent);
            agent.agentAnimFunc.runningAnim(ref agent);
            onTransition = false;
            /*
             * You can set a code statements here while the character waits 
             * before changing to the next destination
             */
            //character waits
            characterWaiting();
        }
        //fix rat being stuck; rat will move to another goal if it finishes the max travel duration
        else if (travel_tick >= this.max_travel_Time && (this.onTransition || !this.readyToChange))
        {
            this.travel_tick = 0.0f;
            this.readyToChange = true;
            return;
        }
        else
        {
            this.isWalk = true;
            this.isRun = true;
            agent.agentAnimFunc.walkingAnim(ref agent);
            //enemyAnim.runningAnim(true);
        }
    }

    //Checker for capturing player
    private bool checkIfCaptured(AIAgent agent)
    {
        //Debug.Log($"Checking capture!");
        //if enemy touches the player while chasing her
        if (agent.navMeshAgent.remainingDistance <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [SerializeField] private float OnSetWaitTime = 1.5f;
    private float OnSetWaitTime_ticks = 0.0f;
    private void characterWaiting()
    {
        //wait for a bit before the character can transition to another destination
        OnSetWaitTime_ticks += Time.deltaTime;
        if (OnSetWaitTime_ticks >= OnSetWaitTime)
        {
            this.readyToChange = true;
            OnSetWaitTime_ticks = 0.0f;
        }
    }

    private GameObject pickup;
    private float distanceSense = 5.0f;
    private GameObject FindPickup(AIAgent agent)
    {
        if(agent.aiSensor.Objects.Count > 0)
        {
            //speeds up the movement if he sees the player
            agent.navMeshAgent.speed = enemy_property.runSpeed;
            return agent.aiSensor.Objects[0];
        }
        //sense distance of the enemy
        if (Vector3.Distance(agent.agentTrans.position, agent.mainPlayerSc.transform.position) < distanceSense)
        {
            //if the player is near the enemy while out of sight and sneaking,
            //then the enemy will not notice the player
            if(agent.mainPlayerSc.playerMovementSc._playerProperty.isSneak || (agent.mainPlayerSc.playerMovementSc.MovementX == 0 && 
                                                               agent.mainPlayerSc.playerMovementSc.MovementY == 0))
            {
                //Debug.LogError("Found but sneaking!");
                return null;
            }
                //Debug.LogError("Found not sneaking!");
            return agent.mainPlayerSc.transform.gameObject;
        }
        return null;
    }

    private void CollectPickUp(AIAgent agent)
    {
        //change the destination and chases the player
        //Debug.LogError($"Rat Found You: {this.transform.name}");
        agent.navMeshAgent.SetDestination(this.pickup.transform.position);
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
