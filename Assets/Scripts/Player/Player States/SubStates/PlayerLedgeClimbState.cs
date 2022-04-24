using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;
    private Vector2 workspace;
    
    private bool isHanging;
    private bool isClimbing;
    private bool jumpInput;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) 
        : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = DetermineCornerPosition();
        startPos.Set(cornerPos.x - core.Movement.FacingDirection * playerData.startOffset.x, cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + core.Movement.FacingDirection * playerData.stopOffset.x, cornerPos.y + playerData.stopOffset.y);
        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

        if (isClimbing)
        {
            player.transform.position = stopPos;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else 
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            player.transform.position = startPos;
            core.Movement.SetVelocityZero();

            if (xInput == core.Movement.FacingDirection && isHanging && !isClimbing)
            {
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if ((yInput == -1 || xInput == -core.Movement.FacingDirection) && isHanging && !isClimbing)
            {
                core.Movement.CheckIfShouldFlip(xInput);
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && !isClimbing)
            {
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public override void AnimationTrigger() 
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void AnimationFinishTrigger() 
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
    }

    public void SetDetectedPosition(Vector2 pos)
    {
        detectedPos = pos;
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(core.CollisionSenses.WallCheck.position, core.Movement.FacingDirection * Vector2.right, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        float tolerance = 0.015f;
        workspace.Set(core.Movement.FacingDirection * (xDist + tolerance), 0.0f);

        RaycastHit2D yHit = Physics2D.Raycast(core.CollisionSenses.LedgeCheck.position + (Vector3)workspace, Vector2.down, core.CollisionSenses.LedgeCheck.position.y + tolerance - core.CollisionSenses.WallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;
        workspace.Set(core.CollisionSenses.WallCheck.position.x + core.Movement.FacingDirection * xDist, core.CollisionSenses.LedgeCheck.position.y - yDist);

        return workspace;
    }
}
