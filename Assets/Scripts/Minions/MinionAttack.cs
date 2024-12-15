using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : MinionStates
{
    Vector3 attackTarget;
    public override void OnEnter(Vector3 target)
    {
        attackTarget = target;
        minion.currentState = NPCStates.Attack;
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        if (minion.life < 20)
        {
            if (minion.redNPC)
            {
                fsm.ChangeState(NPCStates.BackToBase, GameManager.Instance.redBase.transform.position);
            }
            else
            {
                fsm.ChangeState(NPCStates.BackToBase, GameManager.Instance.blueBase.transform.position);
            }
        }
        minion.shoot(attackTarget);
        minion.OutOfAttackState();
    }
}
