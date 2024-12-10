using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private FiniteStateMachine fsm;
    [SerializeField] protected List<Node> wayPoints;
    [SerializeField] protected float velocity;
    [SerializeField] protected int waypointNumber = 0;
    [SerializeField] protected EnemyFieldOfView fieldOfView;


    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<EnemyFieldOfView>();
        fsm = new FiniteStateMachine(this);
        fsm.AddState(EnemyState.Chase, new EnemyChase());

        fsm.AddState(EnemyState.BackToPatrol, new EnemyBackToPatrol());
        fsm.AddState(EnemyState.Follow, new EnemyFollow());
        fsm.AddState(EnemyState.Patrol, new EnemyPatrolState());
        fsm.ChangeState(EnemyState.Patrol, transform.position);
        EnemyFollow.onFoundPlayer += SetFollowState;

    }
    public void SetFollowState(Vector3 target)
    {
        if (!(fsm.GetCurrentState() is EnemyFollow))
        {
            fsm.ChangeState(EnemyState.Chase, target);
        }
    }


    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }

    
    public List<Node> GetWayPoints()
    {
        return this.wayPoints;
    }

    public void Move(Vector3 dir)
    {
        transform.forward= dir;
        transform.position += dir.normalized * velocity * Time.deltaTime;
    }

    public GameObject GetTargetPlayer()
    {
        return fieldOfView.FieldOfView();
    }
    public int GetWayPointNumber()
    {
        return waypointNumber;
    }
    public void SetWayPointNumber(int number)
    {
        this.waypointNumber = number;
    }
}
