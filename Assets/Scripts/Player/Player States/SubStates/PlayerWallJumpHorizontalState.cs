using UnityEngine;

public class PlayerWallJumpHorizontalState : PlayerAbilityState
{
    // Collision Senses
    private bool isTouchingWall;

    private int wallJumpDirection;
    
    public PlayerWallJumpHorizontalState(Player playerInstance, string animationBoolName)
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

        isAbilityDone = true;
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

