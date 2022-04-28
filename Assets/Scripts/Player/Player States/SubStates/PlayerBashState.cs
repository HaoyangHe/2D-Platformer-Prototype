using UnityEngine;

public class PlayerBashState : PlayerAbilityState
{
    // Player Inputs
    private int xInput;
    private bool bashInputStop;
    private Vector2 bashDirection;

    private bool isButtonHolding;

    public PlayerBashState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Time.timeScale = playerData.bashTimeScale;

        player.InputHandler.UseBashInput();
        player.InputHandler.BashDirectionIndicator.position = collisionSenser.BashAbleObj.Transform.position;
        player.InputHandler.BashDirectionIndicator.gameObject.SetActive(true);

        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();

        isButtonHolding = true;

        collisionSenser.BashAbleObj.BeforeBash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isButtonHolding)
            {
                xInput = player.InputHandler.NormInputX;
                bashInputStop = player.InputHandler.BashInputStop;
                bashDirection = player.InputHandler.RawBashDirecionInput;
                bashDirection.Normalize();

                player.InputHandler.BashDirectionIndicator.rotation = Quaternion.AngleAxis(Mathf.Atan2(bashDirection.y, bashDirection.x) * Mathf.Rad2Deg, Vector3.forward);

                if (bashInputStop)
                {
                    isButtonHolding = false;

                    Time.timeScale = 1.0f;
                    startTime = Time.time;

                    player.InputHandler.BashDirectionIndicator.gameObject.SetActive(false);
                    player.transform.position = collisionSenser.BashAbleObj.Transform.position;

                    collisionSenser.BashAbleObj.SetImpulse(-bashDirection * playerData.bashImpulse);
                    collisionSenser.BashAbleObj.AfterBash();
                }
            }
            else 
            {
                isAbilityDone = true;
            }

            player.Anim.SetFloat("xVelocity", Mathf.Abs(movementAPI.CurrentVelocity.x));
            player.Anim.SetFloat("yVelocity", movementAPI.CurrentVelocity.y);
        }
    }

    public override void PhysicsUpdate()
    {
       base.PhysicsUpdate();
    }
}
