using UnityEngine;

public class PlayerBashState : PlayerAbilityState
{
    public bool CanBash { get; private set; }
    
    private Vector2 bashDirction;
    private Vector2 bashDirctionInput;
    
    private int xInput;
    private bool isHolding;
    private bool bashInputStop;

    public PlayerBashState(Player playerInstance, string animationBoolName)
        : base(playerInstance, animationBoolName)
    {
        CanBash = true;
    }

    public override void Enter()
    {
        base.Enter();
        
        startTime = Time.unscaledTime;
        Time.timeScale = playerData.holdTimeScale;

        player.InputHandler.UseBashInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.JumpState.DecreaseAmountOfJumpsLeft();
        player.BashDirectionIndicator.gameObject.SetActive(true);

        collisionSenser.BashAbleObj.BeforeBash();
        player.BashDirectionIndicator.position = collisionSenser.BashAbleObj.Transform.position;

        bashDirction = movementAPI.FacingDirection * Vector2.one;
        isHolding = true;
        CanBash = false;
    }

    public override void Exit()
    {
        base.Exit();

        if (movementAPI.CurrentVelocity.y > playerData.endBashMaxYVelocity)
        {
            movementAPI.SetVelocityY(movementAPI.CurrentVelocity.y * playerData.endBashYUpMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isHolding)
            {
                bashDirctionInput = player.InputHandler.RawBashDirecionInput;
                bashInputStop = player.InputHandler.BashInputStop;
                player.BashDirectionIndicator.position = collisionSenser.BashAbleObj.Transform.position;
                xInput = player.InputHandler.NormInputX;
                
                if (bashDirctionInput != Vector2.zero)
                {
                    bashDirction = bashDirctionInput;
                    bashDirction.Normalize();
                }

                player.BashDirectionIndicator.rotation = Quaternion.AngleAxis(Mathf.Atan2(bashDirction.y, bashDirction.x) * Mathf.Rad2Deg, Vector3.forward);
                
                if (bashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                {
                    isHolding = false;
                    Time.timeScale = 1.0f;
                    startTime = Time.time;

                    if (xInput * bashDirction.x >= 0)
                    {
                        movementAPI.CheckIfShouldFlip(bashDirction.x > 0 ? 1 : -1);
                    }
                    else
                    {
                        movementAPI.CheckIfShouldFlip(xInput > 0 ? 1 : -1);
                    }

                    player.transform.position = collisionSenser.BashAbleObj.Transform.position;
                    movementAPI.SetVelocityZero();
                    // movementAPI.RB2D.drag = playerData.bashDrag;

                    player.BashDirectionIndicator.gameObject.SetActive(false);

                    collisionSenser.BashAbleObj.SetImpulse(-bashDirction * playerData.bashImpulse);
                    collisionSenser.BashAbleObj.AfterBash();
                }
            }
            else
            {
                movementAPI.SetVelocity(playerData.bashVelocity, bashDirction);

                if (Time.time >= startTime + playerData.bashTime)
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

    public override void DoChecks()
    {
        base.DoChecks();
    }
}
