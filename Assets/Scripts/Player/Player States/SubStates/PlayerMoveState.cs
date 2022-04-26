using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName) 
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

        movementAPI.CheckIfShouldFlip(xInput);
        movementAPI.SetVelocityX(xInput * playerData.movementVelocity);

        if (xInput == 0 && !isExitingState)    
        {
            stateMachine.ChangeState(player.IdleState);
        } 
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }
}
