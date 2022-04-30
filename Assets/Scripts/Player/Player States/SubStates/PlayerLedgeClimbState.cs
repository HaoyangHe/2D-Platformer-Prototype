using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    // Player Inputs
    private int xInput;
    private int yInput;
    private bool jumpInput;

    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 workspace;
    
    private bool isHanging;
    private bool isClimbing;

    public PlayerLedgeClimbState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        cornerPos = FindLedgeCorner();
        startPos.Set(cornerPos.x - movementAPI.FacingDirection * playerData.startOffset.x, cornerPos.y - playerData.startOffset.y);
        endPos.Set(cornerPos.x + movementAPI.FacingDirection * playerData.endOffset.x, cornerPos.y + playerData.endOffset.y);

        isHanging = false;
        isClimbing = false;

        movementAPI.SetVelocityZero();
        player.transform.position = startPos;
    }

    public override void Exit()
    {
        base.Exit();

        if (isClimbing)
        {
            player.transform.position = endPos;
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

            movementAPI.SetVelocityZero();          // Keeps the cinemachine camera focus on the player
            player.transform.position = startPos;           

            if (xInput == movementAPI.FacingDirection && isHanging && !isClimbing)
            {
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);    
            }
            else if ((xInput == -movementAPI.FacingDirection || yInput == -1) && isHanging && !isClimbing)
            {
                movementAPI.CheckIfShouldFlip(xInput);
                stateMachine.ChangeState(player.InAirState);
            }
            else if (jumpInput && xInput != movementAPI.FacingDirection && !isClimbing)
            {
                stateMachine.ChangeState(player.WallJumpHorizontalState);
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

        player.Anim.SetBool("climbLedge", false);   // The trigger for HOLD animation state to CLIMB animation state.  
    }

    public void SetDetectedPosition(Vector2 pos)
    {
        detectedPos = pos;
    }

    public Vector2 FindLedgeCorner()
    {
        player.transform.position = detectedPos;

        RaycastHit2D xHit = Physics2D.Raycast(collisionSenser.WallCheck.position, 
                                              movementAPI.FacingDirection * Vector2.right,
                                              collisionSenser.WallCheckDistance,
                                              collisionSenser.WhatIsGround);
        float xDist = xHit.distance;
        float tolerance = 0.015f;
        workspace.Set(movementAPI.FacingDirection * (xDist + tolerance), 0.0f);

        RaycastHit2D yHit = Physics2D.Raycast(collisionSenser.LedgeCheck.position + (Vector3)workspace,
                                              Vector2.down,
                                              collisionSenser.LedgeCheck.position.y - collisionSenser.WallCheck.position.y + tolerance,
                                              collisionSenser.WhatIsGround);
        float yDist = yHit.distance;
        workspace.Set(collisionSenser.WallCheck.position.x + movementAPI.FacingDirection * xDist, collisionSenser.LedgeCheck.position.y - yDist);

        return workspace;
    }
}
