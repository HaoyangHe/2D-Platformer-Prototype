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
    public BashableObject BashAbleObj { get; private set; }
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB2D { get; private set; }
    public Transform BashDirectionIndicator { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;
    
    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;
    #endregion

    #region Other Varialbles
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }

    private Vector2 workspace;
    #endregion

    #region Unity Callback Functions
    private void Awake() 
    {
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

        FacingDirection = 1;

        StateMachine.Initialize(IdleState);
    }

    private void Update() 
    {
        CurrentVelocity = RB2D.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate() 
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        Gizmos.DrawWireSphere(transform.position, playerData.bashCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + FacingDirection * playerData.wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(ledgeCheck.position, new Vector3(ledgeCheck.position.x + FacingDirection * playerData.wallCheckDistance, ledgeCheck.position.y, ledgeCheck.position.z));
        //Gizmos.DrawLine(LedgeClimbState.startPos, LedgeClimbState.stopPos);
        //Gizmos.DrawWireCube(LedgeClimbState.startPos, new Vector3(0.3f, 0.3f, 0.1f));
        //Gizmos.DrawWireCube(LedgeClimbState.stopPos, new Vector3(0.3f, 0.3f, 0.1f));
    }
    #endregion

    #region Set Functions
    public void SetVelocityZero()
    {
        RB2D.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(direction * angle.x * velocity, angle.y * velocity);
        RB2D.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        direction.Normalize();
        workspace = direction * velocity;
        RB2D.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB2D.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB2D.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void AddForce(Vector2 forceToAdd) 
    {
        RB2D.AddForce(forceToAdd);
    }

    public void AddForceImpulse(Vector2 forceToAdd) 
    {
        RB2D.AddForce(forceToAdd, ForceMode2D.Impulse);
    }
    #endregion

    #region Check Functions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, FacingDirection * Vector2.right, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, -FacingDirection * Vector2.right, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, FacingDirection * Vector2.right, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfNearBashAble()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerData.bashCheckRadius, playerData.whatIsInteractable);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "BashAble")
            {
                BashAbleObj = hit.transform.gameObject.GetComponent<BashableObject>();
                BashAbleObj.Lit();
                return true;
            }
        }

        if (BashAbleObj != null)
        {
            BashAbleObj.Unlit();
            BashAbleObj = null;
        }

        return false;
    }

    public bool CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
            StateMachine.CurrentState.DoChecks();
            return true;
        }

        return false;
    }
    #endregion

    #region Other Functions
    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, FacingDirection * Vector2.right, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        float tolerance = 0.015f;
        workspace.Set(FacingDirection * (xDist + tolerance), 0.0f);

        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)workspace, Vector2.down, ledgeCheck.position.y + tolerance - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;
        workspace.Set(wallCheck.position.x + FacingDirection * xDist, ledgeCheck.position.y - yDist);

        return workspace;
    }

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
