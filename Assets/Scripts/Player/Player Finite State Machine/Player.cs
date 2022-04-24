using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
//  public PlayerWallHopState WallHopState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerBashState BashState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Object References
    #endregion

    #region Components
    public CoreComponentsManager Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB2D { get; private set; }
    public Transform BashDirectionIndicator { get; private set; }
    #endregion

    #region Unity Callback Functions
    private void Awake() 
    {
        Core = GetComponentInChildren<CoreComponentsManager>();
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");    
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");    
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");    
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");    
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");    
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");    
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");    
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");  
//      WallHopState = new PlayerWallHopState(this, StateMachine, playerData, "inAir"); 
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState"); 
        BashState = new PlayerBashState(this, StateMachine, playerData, "inAir");
    }

    private void Start() 
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB2D = GetComponent<Rigidbody2D>();
        BashDirectionIndicator = transform.Find("BashDirectionIndicator");

        StateMachine.Initialize(IdleState);
    }

    private void Update() 
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() 
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(Core.CollisionSenses.GroundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position, playerData.bashCheckRadius);
        Gizmos.DrawLine(Core.CollisionSenses.WallCheck.position, new Vector3(Core.CollisionSenses.WallCheck.position.x + Core.Movement.FacingDirection * playerData.wallCheckDistance, Core.CollisionSenses.WallCheck.position.y, Core.CollisionSenses.WallCheck.position.z));
        Gizmos.DrawLine(Core.CollisionSenses.LedgeCheck.position, new Vector3(Core.CollisionSenses.LedgeCheck.position.x + Core.Movement.FacingDirection * playerData.wallCheckDistance, Core.CollisionSenses.LedgeCheck.position.y, Core.CollisionSenses.LedgeCheck.position.z));
        //Gizmos.DrawLine(LedgeClimbState.startPos, LedgeClimbState.stopPos);
        //Gizmos.DrawWireCube(LedgeClimbState.startPos, new Vector3(0.3f, 0.3f, 0.1f));
        //Gizmos.DrawWireCube(LedgeClimbState.stopPos, new Vector3(0.3f, 0.3f, 0.1f));
    }
    #endregion

    #region Check Functions
   

    
    #endregion

    #region Other Functions

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    
    #endregion
}
