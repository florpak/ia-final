using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPatrolState : State
{

    public override void OnEnter(Vector3 target)
    {
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if(enemy.GetTargetPlayer() != null)
        {
            fsm.ChangeState(EnemyState.Follow, enemy.GetTargetPlayer().transform.position);
        }
        if (Vector3.Distance(enemy.GetWayPoints()[enemy.GetWayPointNumber()].transform.position, enemy.transform.position) > 0.1f)
        {
            enemy.Move(enemy.GetWayPoints()[enemy.GetWayPointNumber()].transform.position- enemy.transform.position);
        }
        else
        {
            if(enemy.GetWayPoints().Count -1 > enemy.GetWayPointNumber())
            {
                enemy.SetWayPointNumber(enemy.GetWayPointNumber() + 1);
            }
            else
            {
                enemy.SetWayPointNumber(0);
            }
        }
    }

}
