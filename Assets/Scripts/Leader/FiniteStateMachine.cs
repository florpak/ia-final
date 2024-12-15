using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    Dictionary<LeaderState, State> allStates = new Dictionary<LeaderState, State>();
    State _currentState;
    Leader enemy;


    private void Start()
    {
        EnemyFollow.onFoundPlayer += SetFollowState;
    }
    public FiniteStateMachine(Leader enemy)
    {
        this.enemy = enemy;
    }
    public void SetFollowState(Vector3 target)
    {
        if (GetCurrentState() is not LeaderChase)
        {
            ChangeState(LeaderState.Chase, target);
        }
    }

    public void AddState(LeaderState enemyState, State state)
    {

        if (!allStates.ContainsKey(enemyState))
        {
            allStates.Add(enemyState, state);
            state.fsm = this;
            state.leader = this.enemy;
        }
        else
        {
            allStates[enemyState] = state;
        }
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(LeaderState state, Vector3 target)
    {
        _currentState?.OnExit();
        if (allStates.ContainsKey(state)) _currentState = allStates[state];
        _currentState?.OnEnter(target);
    }
    public State GetCurrentState()
    {
        return _currentState;
    }
}


public enum LeaderState
{
    Idle, Patrol, Follow, BackToPatrol, Chase, Attack
}

