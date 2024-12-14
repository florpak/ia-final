using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    protected Vector3 velocity;
    protected float size = 0.5f;
    [SerializeField] protected GameObject target;
    [SerializeField] protected float _maxSpeed;
    [SerializeField] protected float _maxForce;
    [SerializeField] protected float viewRadius, separationRadius;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected float seekForceMultiplier;

    void Start()
    {

    }

    void Update()
    {

    }

    public Vector3 LeaderFollowing(Agent leader)
    {
        Vector3 desiredPosition = leader.transform.position - (leader.transform.forward * (viewRadius * 0.2f));
        Debug.DrawLine(transform.position, desiredPosition, Color.blue);

        Vector3 desiredVelocity = leader.velocity;

        Vector3 seekForce = Seek(desiredPosition) * seekForceMultiplier;

        Debug.DrawLine(transform.position, transform.position + seekForce, Color.green);

        return seekForce;
    }

    public Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, _maxSpeed);
    }

    public Vector3 Seek(Vector3 targetPos, float maxSpeed)
    {
        Debug.DrawLine(transform.position, targetPos, Color.yellow);
        Vector3 vectorDeseado = targetPos - transform.position;

        vectorDeseado.Normalize();
        vectorDeseado *= maxSpeed;

        Vector3 steering = vectorDeseado - velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);


        return steering;

    }
    public virtual void Move()
    {

        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity;
        UpdateBoundPosition();
    }


    public bool HasToUseObstacleAvoidance()
    {
        Vector3 avoidance = ObstacleAvoidance();
        avoidance.y = 0;
        AddForce(avoidance * 2);
        return avoidance != Vector3.zero;
    }

    public Vector3 Arrive(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);

        if (dist > viewRadius)
        {
            return Seek(targetPos, _maxSpeed);
        }
        else
        {
            return Seek(targetPos, _maxSpeed * (dist / viewRadius));
        }
    }

    public Vector3 Alignment(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            desired += item.velocity;
            boidCount++;
        }
        desired /= boidCount;
        return SteeringToAlignment(desired.normalized * _maxSpeed);
    }

    public Vector3 SteeringToAlignment(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - velocity, _maxForce * Time.deltaTime);
    }

    public Vector3 Pursuit(Agent agent)
    {
        Vector3 desired = Vector3.zero;
        desired += agent.transform.position + agent.velocity;

        return Seek(desired * _maxSpeed);
    }
    public Vector3 Evade(List<MinionNPC> agents)
    {
        Vector3 desired = Vector3.zero;
        int enemyCount = 0;

        foreach (MinionNPC item in agents)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            enemyCount++;
            desired += item.transform.position + item.velocity;
        }

        return -Seek(desired / enemyCount, _maxSpeed);
    }

    public Vector3 Flee(List<MinionNPC> agents)
    {
        Vector3 desired = Vector3.zero;
        int enemyCount = 0;

        foreach (MinionNPC item in agents)
        {
            if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
            enemyCount++;
            desired += item.transform.position;
        }

        return -Seek(desired / enemyCount, _maxSpeed);

    }

    public Vector3 ObstacleAvoidance()
    {
        /*Debug.DrawLine(transform.position, transform.position + transform.forward * viewRadius);
        if (Physics.Raycast(transform.position, transform.forward, viewRadius,obstacleLayer))
        {
            return Seek(transform.forward, _maxSpeed);
        }*/

        Debug.DrawLine(transform.position + transform.right * size, transform.position + transform.right * size + transform.forward * viewRadius, Color.green);
        Debug.DrawLine(transform.position - transform.right * size, transform.position - transform.right * size + transform.forward * viewRadius, Color.green);

        if (Physics.Raycast(transform.position + transform.right * size, transform.forward, viewRadius, obstacleLayer))
        {
            return Seek(transform.up - transform.right * 4, _maxSpeed);
        }
        else if (Physics.Raycast(transform.position - transform.right * size, transform.forward, viewRadius, obstacleLayer))
        {
            return Seek(transform.up + transform.right, _maxSpeed);
        }


        return Vector3.zero;
    }

    protected Vector3 Cohesion(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > viewRadius * viewRadius) continue;
            boidCount++;
            desired += item.transform.position;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired /= boidCount;

        return Seek(desired);
    }

    protected Vector3 Separation(List<Agent> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (Agent item in agents)
        {
            if (item == this) continue;
            Vector3 dist = item.transform.position - transform.position;
            if (dist.sqrMagnitude > separationRadius * separationRadius) continue;
            boidCount++;
            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired *= -1;

        return SteeringToAlignment(desired.normalized * _maxSpeed);
    }

    public void AddForce(Vector3 force)
    {
        velocity = Vector3.ClampMagnitude(force + velocity, _maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    public void UpdateBoundPosition()
    {
        //transform.position = GameManager.Instance.AdjustPositionsToBounds(transform.position);
    }
}
