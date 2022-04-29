using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    // Player Inputs
    protected int xInput;
    protected int yInput;
    protected bool jumpInput;
    protected bool grabInput;

    // Collision Senses
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;

    private bool coyoteTime;        // Player can still leave the wall after touching it
                                    // within a very short period of time. Instead of changing
                                    // into WallSlide state and stick to the wall immediately.

    public PlayerTouchingWallState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();

        if (stateMachine.LastState is PlayerInAirState && stateMachine.CurrentState is PlayerWallSlideState)
        {
            coyoteTime = true;
        }
    }

    public override void Exit()
    {
        base.Exit();

        coyoteTime = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        grabInput = player.InputHandler.GrabInput;

        CheckCoyoteTime();

        if (jumpInput && xInput != 0)
        { 
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall)
        { 
            stateMachine.ChangeState(player.InAirState);
        }
        else if (xInput * movementAPI.FacingDirection < 0)
        {
            if (coyoteTime || IsXInputTimeEnough())
            {
                movementAPI.CheckIfShouldFlip(xInput);
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = collisionSenser.IsGrounded;
        isTouchingWall = collisionSenser.IsTouchingWallFront;
        isTouchingLedge = collisionSenser.IsTouchingLedge;

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    private bool IsXInputTimeEnough()
    {
        return Time.time >= player.InputHandler.XInputStartTime + playerData.getOfWallTime;
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTimeSlide)
        {
            coyoteTime = false;
        }
    }
}
