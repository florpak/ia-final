using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionNPC : Agent
{
    private FiniteStateMachine fsm;
    //[SerializeField] protected float size = 1f;
    List<Agent> boidsList;
    List<Agent> boidsInRange;
    [SerializeField]
    protected Agent targetAgent;
    [SerializeField]
    protected float stamina;
    [SerializeField]
    protected List<GameObject> wayPoints;
    [SerializeField] protected int waypointNumber = 0;

    [SerializeField] protected Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        //slider = GetComponentInChildren<Slider>();
        slider.value = this.stamina;
        size = 0.5f;
        //GameManager.Instance.enemyAgent.Add(this);
        //fsm = new FiniteStateMachine(this);
        //fsm.AddState(PlayerState.Idle, new IdleState());
        //fsm.AddState(PlayerState.Patrol, new PatrolState());
        //fsm.AddState(PlayerState.Chase, new ChaseState());
        //fsm.ChangeState(PlayerState.Idle);
        //boidsList = GameManager.Instance.agents;
        //GameManager.Instance.playerAgent.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        fsm.Update();

    }

    private void GetTarget()
    {
        boidsInRange = new List<Agent>();

        foreach (Agent item in boidsList)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            boidsInRange.Add(item);
        }
        if (boidsInRange.Count == 0)
        {
            targetAgent = null;
        }
        else
        {
            //targetAgent = boidsList.OrderBy(X => Vector3.Distance(X.transform.position, transform.position)).FirstOrDefault();
        }


    }

    private void UpdateToChase()
    {
        foreach (Boid item in boidsList)
        {
            /*if (Vector3.Distance(transform.position,item.transform.position< viewRadius){
                fsm.ChangeState(PlayerState.Attack,);
            })*/
        }

    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public float GetSize()
    {
        return this.size;
    }
    public float GetMaxSpeed()
    {
        return this._maxSpeed;
    }
    public float GetMaxForce()
    {
        return this._maxForce;
    }
    public float GetViewRadius()
    {
        return this.viewRadius;
    }
    public Vector3 GetVelocity()
    {
        return this.velocity;
    }
    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }
    public LayerMask GetObstacleLayer()
    {
        return this.obstacleLayer;
    }
    public float GetStamina()
    {
        return this.stamina;
    }
    public float AddStamina(float amount)
    {
        this.stamina += amount;
        slider.value = this.stamina;
        return this.stamina;
    }
    public float SubstractStamina(float amount)
    {
        this.stamina -= amount * 3;
        slider.value = this.stamina;
        return this.stamina;
    }
    public List<GameObject> GetWayPoints()
    {
        return this.wayPoints;
    }
    public int GetWayPointNumber()
    {
        return this.waypointNumber;
    }

    public void SetWayPointNumber(int number)
    {
        this.waypointNumber = number;
    }
    public Agent GetTargetAgent()
    {
        return targetAgent;
    }
}
