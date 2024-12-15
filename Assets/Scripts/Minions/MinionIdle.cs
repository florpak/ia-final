using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionIdle : MinionStates
{
    public override void OnEnter(Vector3 target)
    {
        minion.currentState = NPCStates.Idle;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        minion.SwitchToAttackState();
        if (Vector3.Distance(minion.transform.position, minion.target.transform.position) > minion.separationRadius * 4)
        {
            fsm.ChangeState(NPCStates.Follow, minion.target.transform.position);
        }
    }
}
