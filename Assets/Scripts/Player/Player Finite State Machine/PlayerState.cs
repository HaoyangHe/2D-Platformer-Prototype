using UnityEngine;

public class PlayerState
{
    // Core References
    protected CoreComponentsManager core;
    protected CollisionSenses collisionSenser;
    protected Movement movementAPI;
    
    // References
    protected Player player;
    protected PlayerData playerData;
    protected PlayerStateMachine stateMachine;

    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected float startTime;

    private string animBoolName;

    public PlayerState(Player playerInstance, string animationBoolName)
    {
        player = playerInstance;
        playerData = player.PlayerData;
        stateMachine = player.StateMachine;

        core = player.Core;
        collisionSenser = core.CollisionSenses;          // NOTE: CoreComponent references are set after the Awake() funcion.
        movementAPI = core.Movement;

        animBoolName = animationBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        DoChecks();     // NOTE: The reason of putting Dochecks() in LogicUpdate() instead of PhysicsUpdate() listed as follow:
                        // 1. Physics System independent: NOTHING will applies to the rigidbody here.
                        // 2. Update Loop related: collision detection is related to FacingDirection
                        //                      which will be update during the normal Update() Loop.
    }

    public virtual void PhysicsUpdate()
    {
        // Put DoChecks() in LogicUpdate() for a more accurate collision detection & lower performance cost.
    }

    public virtual void DoChecks()
    {
    }

    public virtual void AnimationTrigger() 
    {
    }

    public virtual void AnimationFinishTrigger() 
    {
        isAnimationFinished = true;
    }
}
