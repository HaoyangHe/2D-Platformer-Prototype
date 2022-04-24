using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingLedge;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;

    private bool coyoteTime;
    
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput && xInput != core.Movement.FacingDirection)
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (!isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!isTouchingWall || xInput * core.Movement.FacingDirection < 0)
        {
            if (CheckGetOfWall() || coyoteTime)
            {
                core.Movement.CheckIfShouldFlip(xInput);            
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = core.CollisionSenses.IsGrounded;
        isTouchingWall = core.CollisionSenses.IsTouchingWallFront;
        isTouchingLedge = core.CollisionSenses.IsTouchingLedge;

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    private bool CheckGetOfWall()
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

    public void StartCoyoteTime() 
    {
        coyoteTime = true;
    }
}
