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
    public bool redAgent = false;

    public GameObject bulletPrefab;
    bool shootCoroutineActive = false;
    public int enemyAttackRadius;
    public List<MinionNPC> minionsList;

    public LeaderState currentState;

    void Start()
    {
        fieldOfView = GetComponent<EnemyFieldOfView>();
        fsm = new FiniteStateMachine(this);
        fsm.AddState(LeaderState.Chase, new LeaderChase());

        fsm.AddState(LeaderState.BackToPatrol, new EnemyBackToPatrol());
        fsm.AddState(LeaderState.Follow, new EnemyFollow());
        fsm.AddState(LeaderState.Idle, new LeaderIdleState());
        fsm.AddState(LeaderState.Attack, new LeaderAttack());
        fsm.ChangeState(LeaderState.Idle, transform.position);
        EnemyFollow.onFoundPlayer += SetFollowState;
    }

    public void SetFollowState(Vector3 target)
    {
        if (!(fsm.GetCurrentState() is EnemyFollow))
        {
            fsm.ChangeState(LeaderState.Chase, target);
        }
    }

    void Update()
    {
        fsm.Update();

        if (Input.GetMouseButtonDown(keyCode))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            click = new Vector3(click.x, click.y, 0);

            fsm.ChangeState(LeaderState.Chase, click);
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

    public void shoot(Vector3 attackTarget)
    {
        if (!shootCoroutineActive)
        {
            StartCoroutine(shootCoroutine(attackTarget));
        }
    }

    IEnumerator shootCoroutine(Vector3 attackTarget)
    {
        shootCoroutineActive = true;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Initialize(this, attackTarget);
        yield return new WaitForSeconds(1.5f);
        shootCoroutineActive = false;
    }

    public bool InSight(Vector3 A, Vector3 B)
    {
        return !Physics.Raycast(A, B - A, Vector3.Distance(A, B), GameManager.Instance.wallMask);
    }
}
