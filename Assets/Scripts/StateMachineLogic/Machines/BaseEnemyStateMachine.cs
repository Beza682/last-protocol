using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStateMachine : MonoBehaviour, IStateSwitcher, IStateUpdate
{
    private BaseState _currentState;
    private StatesNPC _defaultState;
    private readonly protected Dictionary<StatesNPC, BaseState> StatesMap;

    public BaseEnemyStateMachine(in StatesNPC defaultState)
    {
        StatesMap = new Dictionary<StatesNPC, BaseState>();
        _defaultState = defaultState;
    }

    private BaseState GetState(StatesNPC state)
    {
        return StatesMap[state];
    }

    private void SetState(StatesNPC state)
    {
        _currentState?.Stop();
        _currentState = GetState(state);
        _currentState.Start();
    }

    private protected void DefaultState()
    {
        SetState(_defaultState);
    }

    public void SwitchState(StatesNPC state)
    {
        SetState(state);
    }

    public void StateUpdate()
    {
        _currentState.Update();
    }
}
