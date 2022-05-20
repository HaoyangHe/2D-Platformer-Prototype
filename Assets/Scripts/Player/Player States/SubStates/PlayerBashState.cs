using UnityEngine;

public class PlayerBashState : PlayerAbilityState
{
    // Player Inputs
    private int xInput;
    private bool jumpInput;
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

        movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.bashExitXMultiplier);

        if (movementAPI.CurrentVelocity.y > 0)
        {
            movementAPI.SetVelocityY(movementAPI.CurrentVelocity.y * playerData.bashExitYMultiplier);
        }
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

                    if (xInput * bashDirection.x >= 0)
                    {
                        movementAPI.CheckIfShouldFlip(bashDirection.x > 0 ? 1 : -1);
                    }
                    else
                    {
                        movementAPI.CheckIfShouldFlip(xInput);
                    }

                    collisionSenser.BashAbleObj.SetImpulse(-bashDirection * playerData.bashImpulse);
                    collisionSenser.BashAbleObj.AfterBash();
                }
            }
            else 
            {
                jumpInput = player.InputHandler.JumpInput;

                movementAPI.SetVelocity(playerData.bashVelocity, bashDirection);

                if (jumpInput)
                { 
                    movementAPI.SetVelocityX(movementAPI.CurrentVelocity.x * playerData.bashjumpInterruptMultiplier);
                }
                
                if (Time.time > startTime + playerData.bashTime || jumpInput)
                { 
                    isAbilityDone = true;
                }
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
