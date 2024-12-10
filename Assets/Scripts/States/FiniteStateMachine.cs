using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    Dictionary<EnemyState, State> allStates = new Dictionary<EnemyState, State>();
    State _currentState;
    Enemy enemy;


    private void Start()
    {
        EnemyFollow.onFoundPlayer += SetFollowState;
    }
    public FiniteStateMachine(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void SetFollowState(Vector3 target)
    {
        if (GetCurrentState() is not EnemyChase)
        {
            ChangeState(EnemyState.Chase, target);
        }
    }

    public void AddState(EnemyState enemyState, State state)
    {

        if (!allStates.ContainsKey(enemyState))
        {
            allStates.Add(enemyState, state);
            state.fsm = this;
            state.enemy = this.enemy;
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

    public void ChangeState(EnemyState state, Vector3 target)
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


public enum EnemyState
{
    Idle, Patrol, Follow, BackToPatrol, Chase
}

