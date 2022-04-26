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
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerBashState BashState { get; private set; }
    #endregion

    [SerializeField] private PlayerData playerData;
    
    public Animator Anim { get; private set; }
    public PlayerData PlayerData { get => playerData; private set => PlayerData = value; }
    public PlayerInputHandler InputHandler { get; private set; }
    public CoreComponentsManager Core { get; private set; }
    
    public Transform BashDirectionIndicator { get; private set; }

    private void Awake() 
    {
        Anim = GetComponent<Animator>();
        Core = GetComponentInChildren<CoreComponentsManager>();
        InputHandler = GetComponent<PlayerInputHandler>();
        BashDirectionIndicator = transform.Find("BashDirectionIndicator");
    }

    private void Start() 
    {
        StateMachine = new PlayerStateMachine();

        // Make sure to instantiate PlayerStates in AFTER the Awake() function.
        IdleState = new PlayerIdleState(this, "idle");
        MoveState = new PlayerMoveState(this, "move");
        JumpState = new PlayerJumpState(this, "inAir");
        InAirState = new PlayerInAirState(this, "inAir");
        LandState = new PlayerLandState(this, "land");
        WallSlideState = new PlayerWallSlideState(this, "wallSlide");
        WallGrabState = new PlayerWallGrabState(this, "wallGrab");
        WallClimbState = new PlayerWallClimbState(this, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, "ledgeClimbState");
        BashState = new PlayerBashState(this, "inAir");

        StateMachine.Initialize(IdleState);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void Update() 
    {
        Core.LogicUpdate();                         // Make sure Core Update first.
        StateMachine.CurrentState.LogicUpdate();
    }

    // Amiation Triggers
    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}
