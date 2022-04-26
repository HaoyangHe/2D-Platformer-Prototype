using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;   // PlayerAbilityStates can only change to other states if isAbilityDone is set to true.

    // Collision Senses
    private bool isGrounded;
    
    public PlayerAbilityState(Player playerInstance, string animationBoolName) 
        : base(playerInstance, animationBoolName)
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

        if (isAbilityDone)
        {
            if (isGrounded && movementAPI.CurrentVelocity.y < 0.01f)    // Player falls down and lands on the ground.
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
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

        isGrounded = collisionSenser.IsGrounded;
    }
}
