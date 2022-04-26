using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            movementAPI.SetVelocityY(playerData.wallClimbVelocity);
            
            if (grabInput && yInput == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (!grabInput || yInput < 0)
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
    }
}
