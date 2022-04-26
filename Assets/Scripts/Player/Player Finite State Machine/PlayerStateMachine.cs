using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public PlayerState LastState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        LastState = startingState;
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        LastState = CurrentState;
        CurrentState = newState;
        CurrentState.Enter();
    }
}
