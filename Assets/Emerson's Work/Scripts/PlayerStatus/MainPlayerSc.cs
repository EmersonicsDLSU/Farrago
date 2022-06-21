using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainPlayerSc : MonoBehaviour
{
    [HideInInspector] public Transform playerTrans;
    [HideInInspector] public CharacterController playerCharController;
    //[HideInInspector] public Rigidbody agentRigid;
    //[HideInInspector] public MeshRenderer agentMesh;
    [HideInInspector] public SkinnedMeshRenderer playerSkinMesh;
    public Animator playerAnim;
    [HideInInspector] public PlayerAngelaAnimations playerAngelaAnim;

    //other player scripts
    [HideInInspector] public PlayerMovement playerMovementSc;
    [HideInInspector] public Inventory playerInventory;
    [HideInInspector] public PuzzleInventory player_puzzleInventory;
    [HideInInspector] public PlayerCollisionDetection playerDetectCollision = null;
    [HideInInspector] public PotionAbsorption PotionAbsorptionSC = null;
    [HideInInspector] public FixedJoystick joystick = null;

    //external scripts
    public TimelineLevel timelineLevelSc = null;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogError($"Test");
        playerTrans = this.GetComponent<Transform>(); ;
        playerCharController = this.GetComponentInChildren<CharacterController>();
        playerSkinMesh = this.GetComponentInChildren<SkinnedMeshRenderer>();
        if(playerAnim == null)
        {
            playerAnim = this.GetComponentInChildren<Animator>();
        }
        playerAngelaAnim = this.GetComponentInChildren<PlayerAngelaAnimations>();
        playerMovementSc = this.GetComponentInChildren<PlayerMovement>();
        playerInventory = this.GetComponentInChildren<Inventory>();
        player_puzzleInventory = this.GetComponentInChildren<PuzzleInventory>();
        if (timelineLevelSc == null)
        {
            if (FindObjectOfType<TimelineLevel>() != null) timelineLevelSc = FindObjectOfType<TimelineLevel>();
            else Debug.LogError($"Missing \"TimelineLevel script\" in {this.gameObject.name}");
        }
        playerDetectCollision = this.GetComponentInChildren<PlayerCollisionDetection>();
        PotionAbsorptionSC = this.GetComponentInChildren<PotionAbsorption>();

        joystick = FindObjectOfType<FixedJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementSc.update(this);
        playerInventory.update(this);
        PotionAbsorptionSC.update(this);
    }
}
