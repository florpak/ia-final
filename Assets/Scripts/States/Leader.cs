using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    private FiniteStateMachine fsm;
    [SerializeField] protected List<Node> wayPoints;
    [SerializeField] protected float velocity;
    [SerializeField] protected int waypointNumber = 0;
    [SerializeField] protected EnemyFieldOfView fieldOfView;
    [SerializeField] public int keyCode;

    void Start()
    {
        fieldOfView = GetComponent<EnemyFieldOfView>();
        fsm = new FiniteStateMachine(this);
        fsm.AddState(EnemyState.Chase, new LeaderChase());

        fsm.AddState(EnemyState.BackToPatrol, new EnemyBackToPatrol());
        fsm.AddState(EnemyState.Follow, new EnemyFollow());
        fsm.AddState(EnemyState.Idle, new LeaderIdleState());
        fsm.ChangeState(EnemyState.Idle, transform.position);
        EnemyFollow.onFoundPlayer += SetFollowState;
    }

    public void SetFollowState(Vector3 target)
    {
        if (!(fsm.GetCurrentState() is EnemyFollow))
        {
            fsm.ChangeState(EnemyState.Chase, target);
        }
    }

    void Update()
    {
        fsm.Update();

        if (Input.GetMouseButtonDown(keyCode))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);

            fsm.ChangeState(EnemyState.Chase, click);
        }
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
