using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int amountOfJumpsLeft { get; private set; }

    public PlayerJumpState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();

        if (amountOfJumpsLeft == playerData.amountOfJumps)
        {
            movementAPI.SetVelocityY(playerData.jumpVelocityNo1);
            isAbilityDone = true;
        }
        else
        {
            stateMachine.ChangeState(player.JumpStateDouble);
        }

        DecreaseAmountOfJumpsLeft();
    }

    public bool CanJump()
    {
        return amountOfJumpsLeft > 0;
    }

    public void ResetAmountOfJumpsLeft()
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public void DecreaseAmountOfJumpsLeft()
    {
        amountOfJumpsLeft--;
    }
}
