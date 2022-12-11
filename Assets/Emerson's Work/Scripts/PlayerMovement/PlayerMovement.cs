using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Build.Content;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    //Player properties:
    public PlayerProperty _playerProperty;
    //character movement coordinates
    private float movementX = 0.0f;
    private float movementY = 0.0f;
    // timer attribute for dead ticks 
    private float dead_ticks = 0.0f;
    public float MovementX
    {
        get { return this.movementX; }
    }
    public float MovementY
    {
        get { return this.movementY; }
    }
    // Gravity value
    private const float gravity = -9.81f;
    //player's condition
    private Vector3 velocity;
    // for Ground Check
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private float groundCheckRad = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    // KeybindReceiver object
    private KeybindReceiver local_keybind;

    [SerializeField]private Transform modelTransform;
    private Dictionary<Movement_Angle, List<KeyCode>> inputOrder;
    [HideInInspector] public Movement_Angle movement_angle;

    private MainPlayerSc MPsc;
    private void Awake()
    {

    }
    /*
    // Checking Foot Checker Radius
    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(groundCheckLeft.position, Vector3.forward, groundCheckRad);
        Handles.DrawWireDisc(groundCheckRight.position, Vector3.forward, groundCheckRad);
    }
    */

    private void Start()
    {
        MPsc = FindObjectOfType<MainPlayerSc>();

        _playerProperty.speed = _playerProperty.maxSpeed;
        if (FindObjectOfType<KeybindReceiver>() != null)
        {
            local_keybind = FindObjectOfType<KeybindReceiver>();
        }

        InitializeMovementOrder();
        movement_angle = Movement_Angle.DEG_270;
        InitializeDelegates();
    }

    private void InitializeMovementOrder()
    {
        /*
         * 
        CustomizedGetAxis(local_keybind.left, local_keybind.right, ref movementX);
        CustomizedGetAxis(local_keybind.back, local_keybind.fwd, ref movementY);
         */
        // Movement Axis [-X, +X, -Z, +Z]
        inputOrder = new Dictionary<Movement_Angle, List<KeyCode>>();
        inputOrder.Add(Movement_Angle.DEG_270, 
            new List<KeyCode>(){local_keybind.left, local_keybind.right, local_keybind.back, local_keybind.fwd});
        inputOrder.Add(Movement_Angle.DEG_180, 
            new List<KeyCode>(){local_keybind.back, local_keybind.fwd, local_keybind.right, local_keybind.left});
        inputOrder.Add(Movement_Angle.DEG_90, 
            new List<KeyCode>(){local_keybind.right, local_keybind.left, local_keybind.fwd, local_keybind.back});
        inputOrder.Add(Movement_Angle.DEG_0, 
            new List<KeyCode>(){local_keybind.fwd, local_keybind.back, local_keybind.left, local_keybind.right});
    }

    private void InitializeDelegates()
    {
        Gameplay_DelegateHandler.D_OnDeath += DeathEvent;

    }

    public void OnDestroy()
    {
        Gameplay_DelegateHandler.D_OnDeath -= DeathEvent;
    }

    private void DeathEvent(Gameplay_DelegateHandler.C_OnDeath c_onDeath)
    {
        // Play sound
        PlayerSFX_Manager.Instance.findSFXSourceByLabel("DeathSound").
            PlayOneShot(PlayerSFX_Manager.Instance.findSFXSourceByLabel("DeathSound").clip); 

        // if the player is captured
        MPsc.characterControllerDetection.isPlayerCaptured = c_onDeath.isPlayerCaptured;
        // spawns back the player to the last saved point
        MainCharacterStructs.Instance.playerSavedAttrib.IsDead = true;
        _playerProperty.isDead = true;
        MainPlayerSc temp = FindObjectOfType<MainPlayerSc>();
        FindObjectOfType<MainPlayerSc>().playerAngelaAnim.IH_DeathAnim(ref temp);
        // increment death count
        CareerStatsHandler.instance._careerProperty.total_deaths += 1;

        // reset a specific scene: chase rat scene
        if (MPsc.timelineLevelSc.lastPlayedSceneType == CutSceneTypes.Level4RatCage)
        {
            MPsc.timelineLevelSc.ResetCutscene(CutSceneTypes.Level4RatCage);
        }

        //Debug.LogError($"Dead");

        // play death vignette effect
        FindObjectOfType<PPVolumeSc>().IsDeathEffect = true;

        Invoke("BackToRespawnPoint", 5.0f);

        /* // replays the cutscene **Don't Delete**
        int start = mainPlayer.timelineLevelSc.triggerObjectList.IndexOf(MainCharacterStructs.Instance.playerSavedAttrib.recentTrigger);
        for(int i = start; i < mainPlayer.timelineLevelSc.triggerObjectList.Count; i++)
        {
            mainPlayer.timelineLevelSc.triggerObjectList[i].SetActive(true);
        }
        */
    }

    void BackToRespawnPoint()
    {
        MainPlayerSc temp = FindObjectOfType<MainPlayerSc>();
        temp.playerCharController.enabled = false;
        // re-position the player transform to its latest re-spawn point
        FindObjectOfType<MainPlayerSc>().transform.position = DataPersistenceManager.instance.GetGameData().respawnPoint;
        temp.playerCharController.enabled = true;
    }
    
    // 'update()' is called once per frame
    // Input getkey downs are only computed accurately in Update or LateUpdate
    public void update(MainPlayerSc mainPlayer)
    {
        //temporarily disables the main character during re-spawn
        if(MainCharacterStructs.Instance.playerSavedAttrib.IsDead)
        {
            this.dead_ticks += Time.deltaTime;
            if(this.dead_ticks > 5.0f)
            {
                MainCharacterStructs.Instance.playerSavedAttrib.IsDead = false;
                _playerProperty.isDead = false;
                this.dead_ticks = 0.0f;
            }
            return;
        }
        // NOTE: Currently, this is called in the frame. This should be placed on the collider trigger
        // so it's memory call will not be wasted
        // Checks if the player touched an enemy; if was touched, player gets killed
        // checks if the player has touched an enemy
        /*
        if (mainPlayer.playerDetectCollision.isPlayerCaptured)
        {
            // resets the 'isPlayerCaptured' boolean
            Gameplay_DelegateHandler.D_OnDeath(new Gameplay_DelegateHandler.C_OnDeath(isPlayerCaptured:false));
        }
        */
        // checks if the player is on the ground; checks both feet(left and right)
        _playerProperty.isGround = 
            Physics.CheckSphere(groundCheckRight.position,
         groundCheckRad, groundLayer) || Physics.CheckSphere(groundCheckLeft.position,
         groundCheckRad, groundLayer) ? true : false;
        // configures the idle animation for the mainPlayer
        mainPlayer.playerAngelaAnim.IH_IsGroundAnim(ref mainPlayer); 
        // IDLE CHECKER
        if(this.MovementX != 0 || this.MovementY != 0)
        {
            TextControl.Instance.idleReset();
        }
        // sets the velocity to a constant value when player is on the ground
        if (_playerProperty.isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // gets the delta x and y movement and check the orientation of the player
        if(!MainCharacterStructs.Instance.playerSavedAttrib.IsDead)
        {
            // calls the method that handles the 'fall' behavior
            CheckFalling(ref mainPlayer);
            CalculateHorizontalMovement(ref mainPlayer);
            FindObjectOfType<MainPlayerSc>().playerAngelaAnim.IH_DeathAnim(ref mainPlayer);
        }
        // the conditions below are for movements that shouldn't be combine
        if (!_playerProperty.isSneak)
        {
            PlayerSprintMovement(ref mainPlayer);
        }
        if (_playerProperty.canWalk)
        {
            PlayerSneakMovement(ref mainPlayer);
        }
        // calls the method that handles the 'jump' behavior
        PlayerJumpMovement(ref mainPlayer);
        // integrate gravity simulation to the player; manual gravity implementation since we
        // create our mainPlayer from the basis of 'playerController' and not on 'rigidBody'
        velocity.y += 2.0f * gravity * Time.deltaTime;
        if(!MainCharacterStructs.Instance.playerSavedAttrib.IsDead)
            mainPlayer.playerCharController.Move(velocity * Time.deltaTime);
    }
    // Checks if the player starts to fall 
    private void CheckFalling(ref MainPlayerSc mainPlayer)
    {
        // Checks if the player is falling based from the last and current frame position
        _playerProperty.air_Time1 = mainPlayer.transform.position.y;
        _playerProperty.isFalling = 
            (_playerProperty.air_Time2 > _playerProperty.air_Time1 + 0.01f) ? true : false;
        _playerProperty.air_Time2 = mainPlayer.transform.position.y;
        // check if the last frame of the player position is still ascending
        // and if the current frame is when the player starts descending
        // detects the highest point in y of the fall
        if (!_playerProperty.was_Falling && _playerProperty.isFalling)
        {
            _playerProperty.startOfFallPoint = mainPlayer.playerCharController.transform.position.y;
        }
        // assigns the current frame to the last frame variable
        _playerProperty.was_Falling = _playerProperty.isFalling;
        // End of Fall Functionality
        // Checks if the last frame is coming from a fall
        // and if the current frame is when the player lands on the ground
        if (!_playerProperty.was_Grounded && _playerProperty.isGround)
        {
            // Check if there's a damage
            CheckFallDistance(ref mainPlayer);
        }
        _playerProperty.was_Grounded = _playerProperty.isGround;
    }
    // check if the player fell from a high platform / if fell time is long
    private void CheckFallDistance(ref MainPlayerSc mainPlayer)
    {
        // measures the distance from the 'startOfFallPoint' up to the 'landingPoint'
        _playerProperty.fall_Distance = 
            _playerProperty.startOfFallPoint - mainPlayer.playerCharController.transform.position.y;
        //if the player reach the fell distance threshold, then it will result into an instant death 
        if (_playerProperty.fall_Distance >= _playerProperty.distanceForDeath)
        {
            Gameplay_DelegateHandler.D_OnDeath(new Gameplay_DelegateHandler.C_OnDeath(isPlayerCaptured:false));
        }
        //Debug.LogError($"Player Fell Distance: {this.fall_Distance}");
    }
    // method that handles the sprint conditions and calculation of the mainPlayer
    private void PlayerSprintMovement(ref MainPlayerSc mainPlayer)
    {
        // Sprint Mechanic
        // Is the player moving?
        if (Mathf.Abs(MovementX) > 0.01f || Mathf.Abs(MovementY) > 0.01f)
        {
            // brings back the player to its normalSpeed ; maxSpeed configured by the designer
            _playerProperty.speed = _playerProperty.maxSpeed;
            // checks if the player is on ground layer or sprinting while on air
            if(_playerProperty.isGround || _playerProperty.isJump)
            {
                // Sprint Mechanic KeyEvents
                // When the player is holding the sprint button; increases the speed of the player
                if(Input.GetKey(local_keybind.run))
                {
                    _playerProperty.speed = _playerProperty.maxSpeed * 2.0f;
                    _playerProperty.isRun = true;
                    mainPlayer.playerAngelaAnim.IH_RunAnim(ref mainPlayer);
                }
                if(!Input.GetKey(local_keybind.run))
                {
                    _playerProperty.isRun = false;
                    mainPlayer.playerAngelaAnim.IH_RunAnim(ref mainPlayer);
                }
            }
        }
        else
        {
            _playerProperty.isRun = false;
            mainPlayer.playerAngelaAnim.IH_RunAnim(ref mainPlayer);
        }
    }
    // method that handles the jump conditions and calculation of the mainPlayer
    private void PlayerSneakMovement(ref MainPlayerSc mainPlayer)
    {
        // checks if a timeline is being played.
        // If its true then we lock the player's movement by terminating this method early
        if (mainPlayer.timelineLevelSc.isTimelinePlayed) return;
        // This is for the CUSTOMIZED/BIND KEY FOR 'SNEAK'
        //KeyCode sneakCode = KeybindManager.Instance.getKeyByAction("SNEAK");
        // Sneak Mechanic KeyEvents
        if (Input.GetKey(local_keybind.sneak) && !_playerProperty.isJump)
        {
            mainPlayer.playerCharController.height = 3.0f;
            mainPlayer.playerCharController.center = new Vector3(0, -1.5f, 0);
            _playerProperty.isSneak = true;
            _playerProperty.speed = _playerProperty.maxSpeed * 0.75f;
            mainPlayer.playerAngelaAnim.IH_SneakAnim(ref mainPlayer);
        }
        else if (!Input.GetKey(local_keybind.sneak) || _playerProperty.isJump)
        {
            mainPlayer.playerCharController.height = 4.0f;
            mainPlayer.playerCharController.center = new Vector3(0, -1, 0);
            _playerProperty.isSneak = false;
            mainPlayer.playerAngelaAnim.IH_SneakAnim(ref mainPlayer);
        }
    }
    // method that handles the jump conditions and calculation of the mainPlayer
    public void PlayerJumpMovement(ref MainPlayerSc mainPlayer)
    {
        //checks if a timeline is being played, if its true then we lock the player's movement
        if (mainPlayer.timelineLevelSc.isTimelinePlayed)
        {
            // still walking
            _playerProperty.canWalk = true;
            // for edge jump 
            _playerProperty.onGroundTicks = _playerProperty.onGroundTimer;
            // player lands on the ground
            _playerProperty.isJump = false;
            // Configure the animation for jump mechanic
            mainPlayer.playerAngelaAnim.IH_JumpAnim(ref mainPlayer);
            // timer for the player to jump after the player touches the ground again
            _playerProperty.jumpTicks += Time.deltaTime;
            return;
        }
        // Jumping Mechanic
        // This is for the CUSTOMIZED/BIND KEY FOR 'JUMP'
        //KeyCode jumpCode = KeybindManager.Instance.getKeyByAction("JUMP");
        // if you don't want to make the jump slightly early, then remove this variable
        // and set a value for delayJump(jumpTimer)
        _playerProperty.earlyJumpTicks -= Time.deltaTime;
        //for the smooth edge jump
        _playerProperty.onGroundTicks -= Time.deltaTime;
        // Sneak Mechanic KeyEvents
        if (Input.GetKeyDown(local_keybind.jump))
        {
            _playerProperty.earlyJumpTicks = _playerProperty.earlyJumpTime; //for early jump
        }
        // this is for the case when the player is repeatedly mashing the space / jump button
        if ((_playerProperty.earlyJumpTicks > 0) && (_playerProperty.onGroundTicks > 0) &&
            _playerProperty.isGround)
        {
            //restart the ticks
            _playerProperty.earlyJumpTicks = 0;
            _playerProperty.onGroundTicks = 0;
            // apply downward velocity
            velocity.y = Mathf.Sqrt(_playerProperty.jumpHeight * -2.0f * gravity * _playerProperty.playerWeight);
            // for jumpDelay
            //_playerProperty.canJump = false;
            // end of jump delay
            _playerProperty.isJump = true;
            mainPlayer.playerAngelaAnim.IH_JumpAnim(ref mainPlayer);
        }
        // player is above the ground; free falling
        else if (!_playerProperty.isGround) //(!_playerProperty.isGround  && !_playerProperty.canJump) -- this is for jump delay
        {
            // Reset properties relating to jump mechanic
            _playerProperty.canWalk = false;
            //Debug.LogError($"On Ground: {isGround}");
            _playerProperty.isJump = true;
            _playerProperty.isSneak = false;
            mainPlayer.playerAngelaAnim.IH_JumpAnim(ref mainPlayer);
        }
        // if the player touches the ground
        if (_playerProperty.isGround)
        {
            // still walking
            _playerProperty.canWalk = true;
            // for edge jump 
            _playerProperty.onGroundTicks = _playerProperty.onGroundTimer;
            // player lands on the ground
            _playerProperty.isJump = false;
            // Configure the jumpAnimation
            mainPlayer.playerAngelaAnim.IH_JumpAnim(ref mainPlayer);
            // timer for the player to jump after the player touches the ground again
            _playerProperty.jumpTicks += Time.deltaTime;

             // this is for jump delay
             /*
            if (_playerProperty.jumpTicks >= _playerProperty.jumpTimer)
            {
                _playerProperty.jumpTicks = 0.0f;
                _playerProperty.canJump = true;
            }
            */
            // end of jump delay
            
        }
    }
    
    // Method that handles the translation movement of the MainCharacter
    private void CalculateHorizontalMovement(ref MainPlayerSc mainPlayer)
    {
        // checks if a timeline is being played, if its true then we lock the player's movement
        if (mainPlayer.timelineLevelSc.isTimelinePlayed)
        {
            this.movementX = 0;
            this.movementY = 0;
            mainPlayer.playerAngelaAnim.IH_MoveAnim(ref mainPlayer);
            return;
        }

        /* // old code (no-bind)
        //horizontal and forward coordinates
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");

        Debug.Log($"MOVE X: {movementX}");
        */

        List<KeyCode> order = inputOrder[movement_angle];
        // Evaluates the translation value from the customized method
        CustomizedGetAxis(order[0], order[1], ref movementX);
        CustomizedGetAxis(order[2], order[3], ref movementY);

        // translates the player
        Vector3 move = transform.right * movementX + transform.forward * movementY;
        FlipCharacter(ref mainPlayer);
        mainPlayer.playerCharController.Move(move * _playerProperty.speed * Time.deltaTime);
        /*
        Debug.LogError($"Angle: {modelTransform.eulerAngles.y} : {-angle}");
        // required rotation to move
        if (Math.Abs(modelTransform.eulerAngles.y - (-angle)) <= _playerProperty.rotationToMove)
        {
            mainPlayer.playerCharController.Move(move * _playerProperty.speed * Time.deltaTime);
        }
        */
        // check if the total value for x and y movement is not equal to 0; means its moving
        mainPlayer.playerAngelaAnim.IH_MoveAnim(ref mainPlayer);
    }

    // NOTE: THIS IS NOT EFFICIENT, CUSTOM METHODS LIKE THIS SHOULD BE PLACED IN A SEPARATE CLASS 
    // THAT CONSIST OF DIFFERENT SELECTIONS/BEHAVIORS
    // Customized method for player movement
    private void CustomizedGetAxis(KeyCode first, KeyCode second, ref float axisValue)
    {
        if (Input.GetKey(first))
        {
            axisValue -= Time.deltaTime * _playerProperty.SPEED_MULTIPLIER;
            axisValue = Mathf.Clamp(axisValue, -1.0f, 1.0f);
            //Debug.Log($"MOVE LEFT: {axisValue}");
        }
        else if (Input.GetKey(second))
        {
            axisValue += Time.deltaTime * _playerProperty.SPEED_MULTIPLIER;
            axisValue = Mathf.Clamp(axisValue, -1.0f, 1.0f);
            //Debug.Log($"MOVE RIGHT: {axisValue}");
        }
        else
        {
            axisValue = 0.0f;
        }
    }
    // General method used to clamp the face position of the player to the object
    public void ClampToObject(ref MainPlayerSc mainPlayer, GameObject directedObj)
    {
        Vector3 normalize = 
            (mainPlayer.playerCharController.transform.position - directedObj.transform.position).normalized;
        float angle = Mathf.Atan2(normalize.z, normalize.x) * Mathf.Rad2Deg;
        // Add smooth rotation 
        modelTransform.rotation = Quaternion.Slerp
        (modelTransform.rotation, Quaternion.Euler(0.0f, -angle, 0.0f), rotate_interval * Time.deltaTime);
        //modelTransform.rotation = Quaternion.Euler(0.0f, -angle, 0.0f);
    }

    // current angle of the character in Y-axis; reference position starts from the right
    float angle = 0.0f;
    // how smooth the rotation of the player should be
    public float rotate_interval = 0.1f;
    private void FlipCharacter(ref MainPlayerSc mainPlayer)
    {
        // change orientation based on the direction
        if(movementX != 0.0f || movementY != 0.0f)
        {
            angle = -(Mathf.Atan2(movementY, movementX) * Mathf.Rad2Deg) + 180;
            // Add smooth rotation 
            modelTransform.rotation = Quaternion.Slerp
                (modelTransform.rotation, Quaternion.Euler(0.0f, angle, 0.0f), rotate_interval * Time.deltaTime);
        }
    }

}
