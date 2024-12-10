using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : State
{

    public delegate void FoundPlayer(Vector3 position);
    public static event FoundPlayer onFoundPlayer;

    public override void OnEnter(Vector3 target)
    {
        onFoundPlayer.Invoke(enemy.GetTargetPlayer().transform.position);
    }

    public void SetFollowState(Vector3 target)
    {
        if (fsm.GetCurrentState() is not EnemyChase)
        {
            fsm.ChangeState(EnemyState.Chase, target);
        }
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if (enemy.GetTargetPlayer() != null)
        {
            if(Vector3.Distance(enemy.GetTargetPlayer().transform.position, enemy.transform.position) > 0.1)
            {
                enemy.Move(enemy.GetTargetPlayer().transform.position - enemy.transform.position);
            }
            
        }
        else
        {
            fsm.ChangeState(EnemyState.BackToPatrol, enemy.GetWayPoints()[enemy.GetWayPointNumber()].transform.position);
        }
    }
}