using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeaderIdleState : State
{
    public override void OnEnter(Vector3 target)
    {

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        SwitchToAttackState();
        if (Input.GetMouseButtonDown(leader.keyCode))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);

            fsm.ChangeState(LeaderState.Chase, click);
        }
    }

    public void SwitchToAttackState()
    {
        if (leader.redAgent)
        {
            leader.minionsList = GameManager.Instance.blueNPC;
        }
        else
        {
            leader.minionsList = GameManager.Instance.redNPC;
        }
        foreach (MinionNPC npc in leader.minionsList)
        {
            if (Vector3.Distance(leader.transform.position, npc.transform.position) < leader.enemyAttackRadius)
            {
                if (leader.InSight(leader.transform.position, npc.transform.position))
                    fsm.ChangeState(LeaderState.Attack, npc.transform.position);
            }
        }
    }

}
