using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionNPC : MonoBehaviour
{
    private MinionFSM fsm;
    [SerializeField] public Vector3 velocity;

    [SerializeField] public float speed;
    public float size = 0.5f;
    [SerializeField] public GameObject target;
    [SerializeField] public float _maxSpeed;
    [SerializeField] public float _maxForce;
    [SerializeField] public float dodgeRadius, viewRadius, separationRadius, enemyAttackRadius;
    [SerializeField] public LayerMask obstacleLayer;
    public NPCStates currentState;
    public bool redNPC;
    List<MinionNPC> minions;
    public GameObject bulletPrefab;
    bool shootCoroutineActive = false;
    bool recoverLifeActive = false;
    public int life = 100;

    void Start()
    {
        if (redNPC)
        {
            GameManager.Instance.redNPC.Add(this);
        }
        else
        {
            GameManager.Instance.blueNPC.Add(this);
        }
        fsm = new MinionFSM(this);
        fsm.AddState(NPCStates.Chase, new MinionChase());
        fsm.AddState(NPCStates.Follow, new MinionFollow());
        fsm.AddState(NPCStates.Idle, new MinionIdle());

        fsm.AddState(NPCStates.BackToBase, new MinionBackSafeArea());

        fsm.AddState(NPCStates.Attack, new MinionAttack());
        fsm.ChangeState(NPCStates.Follow, Vector3.zero);
        currentState = NPCStates.Follow;
    }

    // Update is called once per frame
    void Update()
    {


        fsm.Update();

    }
    public void Move(Vector3 dir)
    {
        transform.forward = dir;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }

    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.wallMask);
    }


    public void SwitchToAttackState()
    {
        if (redNPC)
        {
            minions = GameManager.Instance.blueNPC;
        }
        else
        {
            minions = GameManager.Instance.redNPC;
        }
        foreach (MinionNPC agent in minions)
        {
            if (Vector3.Distance(transform.position, agent.transform.position) < enemyAttackRadius)
            {
                if (InSight(transform.position, agent.transform.position))
                    fsm.ChangeState(NPCStates.Attack, agent.transform.position);
            }
        }
    }

    public void OutOfAttackState()
    {
        int count = 0;
        foreach (MinionNPC agent in minions)
        {
            if (Vector3.Distance(transform.position, agent.transform.position) < enemyAttackRadius)
            {
                count++;
            }
        }
        if (count == 0)
        {
            fsm.ChangeState(NPCStates.Follow, target.transform.position);
        }
    }


    public void Hurt()
    {
        Debug.Log("Hurt");
        life -= 10;
        Debug.Log("life is : " + life);
    }

    public void shoot(Vector3 attackTarget)
    {
        if (!shootCoroutineActive)
        {
            StartCoroutine(shootCoroutine(attackTarget));
        }
    }

    public void recoverLifeCoroutine()
    {
        if (recoverLifeActive == false && life < 100)
        {
            StartCoroutine(recoverLife());
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

    IEnumerator recoverLife()
    {
        recoverLifeActive = true;
        life += 10;
        yield return new WaitForSeconds(0.5f);
        if (life < 100)
        {
            StartCoroutine(recoverLife());
        }
        recoverLifeActive = false;
        if (life >= 100)
        {
            fsm.ChangeState(NPCStates.Chase, target.transform.position);
        }
    }
}
