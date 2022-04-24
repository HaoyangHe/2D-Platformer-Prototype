using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    private bool grabInput;
    private bool bashInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool isNearBashAble;
    
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        grabInput = player.InputHandler.GrabInput;
        bashInput = player.InputHandler.BashInput;

        DoChecks();

        if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        } 
        else if (isTouchingWall && isTouchingLedge && grabInput)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingWall && core.Movement.CurrentVelocity.y < -0.01f)
        {
            player.WallSlideState.StartCoyoteTime();
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (isNearBashAble && bashInput)
        {
            stateMachine.ChangeState(player.BashState);
        }
        else if (isAbilityDone)
        {
            stateMachine.ChangeState(player.InAirState);
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
        isNearBashAble = core.CollisionSenses.CheckIfNearBashAble();

        if (isTouchingWall && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }
}
