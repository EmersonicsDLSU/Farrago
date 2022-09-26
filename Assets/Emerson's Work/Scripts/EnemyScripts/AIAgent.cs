using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public Transform agentTrans = null;
    [HideInInspector] public Rigidbody agentRigid = null;
    [HideInInspector] public MeshRenderer agentMesh = null;
    [HideInInspector] public NavMeshAgent navMeshAgent = null;
    [HideInInspector] public EnemyPatrolling enemyPatrol = null;
    [HideInInspector] public ChaseRatBehavior enemyChaseSc = null;
    [HideInInspector] public RatSpawner ratSpawnerSc = null;
    [HideInInspector] public RatChaseSpawner ratChaseSpawnerSc = null;
    [HideInInspector] public IEnemyAnimations<AIAgent> agentAnimFunc = null;
    [HideInInspector] public AISensor aiSensor = null;
    [HideInInspector] public SkinnedMeshRenderer agentSkinMesh = null;
    [HideInInspector] public Animator agentAnim = null;
    [HideInInspector] public MainPlayerSc mainPlayerSc = null;
    // Start is called before the first frame update
    void Start()
    {
        agentTrans = this.GetComponent<Transform>(); ;
        agentRigid = this.GetComponent<Rigidbody>();
        agentMesh = this.GetComponent<MeshRenderer>();
        if(this.GetComponent<NavMeshAgent>() != null)
            navMeshAgent = this.GetComponent<NavMeshAgent>();
        if(this.GetComponentInChildren<EnemyPatrolling>() != null)
            enemyPatrol = this.GetComponentInChildren<EnemyPatrolling>();
        if(this.GetComponentInChildren<ChaseRatBehavior>() != null)
            enemyChaseSc = this.GetComponentInChildren<ChaseRatBehavior>();
        agentAnimFunc = this.GetComponentInChildren<IEnemyAnimations<AIAgent>>();
        aiSensor = this.GetComponentInChildren<AISensor>();
        agentSkinMesh = this.GetComponentInChildren<SkinnedMeshRenderer>();
        agentAnim = this.GetComponentInChildren<Animator>();
        if( FindObjectOfType<MainPlayerSc>() != null)
        {
            mainPlayerSc = FindObjectOfType<MainPlayerSc>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyPatrol != null)
            enemyPatrol.update(this);
        if(enemyChaseSc != null)
            enemyChaseSc.update(this);
        if(aiSensor != null)
            aiSensor.update(this);
    }
}
