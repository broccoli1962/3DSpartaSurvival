using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiniteStateMachine
{
    protected Dictionary<EState, IState> _states;
    protected IState _currentState;

    public FiniteStateMachine(Dictionary<EState, IState> states)
    {
        _states = states;
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void HandleUpdate()
    {
        _currentState?.HandleUpdate();
    }

    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }

    public void ChangeTo(EState state)
    {
        _currentState?.End();
        _currentState = _states[state];
        _currentState?.Start();
    }

    public void HandleAnimation()
    {
        _currentState?.OnAnimationEvent();
    }
}
