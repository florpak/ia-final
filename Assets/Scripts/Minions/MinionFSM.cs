using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFSM : MonoBehaviour
{
    Dictionary<NPCStates, MinionStates> allStates = new Dictionary<NPCStates, MinionStates>();
    MinionStates _currentState;
    MinionNPC minion;


    private void Start()
    {
        LeaderFollow.onFoundPlayer += SetFollowState;
    }
    public MinionFSM(MinionNPC minion)
    {
        this.minion = minion;
    }
    public void SetFollowState(Vector3 target)
    {
        if (GetCurrentState() is not MinionStates)
        {
            ChangeState(NPCStates.Chase, target);
        }
    }

    public void AddState(NPCStates minionState, MinionStates state)
    {

        if (!allStates.ContainsKey(minionState))
        {
            allStates.Add(minionState, state);
            state.fsm = this;
            state.minion = this.minion;
        }
        else
        {
            allStates[minionState] = state;
        }
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(NPCStates state, Vector3 target)
    {
        _currentState?.OnExit();
        if (allStates.ContainsKey(state)) _currentState = allStates[state];
        _currentState?.OnEnter(target);
    }
    public MinionStates GetCurrentState()
    {
        return _currentState;
    }
}

public enum NPCStates
{
    Idle, Attack, Follow, BackToBase, Chase
}
