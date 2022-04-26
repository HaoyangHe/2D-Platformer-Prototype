using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    // Collision Senses
    private bool isTouchingWall;

    private int wallJumpDirection;
    
    public PlayerWallJumpState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        
        DetermineWallJumpDirection(isTouchingWall);
        movementAPI.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        movementAPI.CheckIfShouldFlip(wallJumpDirection);

        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("xVelocity", Mathf.Abs(movementAPI.CurrentVelocity.x));
        player.Anim.SetFloat("yVelocity", movementAPI.CurrentVelocity.y);
        
        if (Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingWall = collisionSenser.IsTouchingWallFront;
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -movementAPI.FacingDirection;
        }
        else 
        {
            wallJumpDirection = movementAPI.FacingDirection;
        }
    }
}

