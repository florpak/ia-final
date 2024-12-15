using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderAttack : State
{
    Vector3 attackTarget;
    public override void OnEnter(Vector3 target)
    {
        attackTarget = target;
        leader.currentState = LeaderState.Attack;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        leader.shoot(attackTarget);

        if (Input.GetMouseButtonDown(leader.keyCode))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);
            fsm.ChangeState(LeaderState.Chase, click);
        }
    }

}
