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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);

            fsm.ChangeState(EnemyState.Chase, click);
        }
    }
}
