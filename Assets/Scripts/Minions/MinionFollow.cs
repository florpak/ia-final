using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionFollow : MinionStates
{
    public override void OnEnter(Vector3 target)
    {
        minion.currentState = NPCStates.Follow;
    }

    public void SetFollowState(Vector3 target)
    {
        if (fsm.GetCurrentState() is not MinionChase)
        {
            fsm.ChangeState(NPCStates.Chase, target);
        }
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        minion.SwitchToAttackState();
        if (!InSight(minion.transform.position, minion.target.transform.position))
        {
            minion.currentState = NPCStates.Chase;
            fsm.ChangeState(NPCStates.Chase, minion.target.transform.position);
        }
        else
        {
            if (Vector3.Distance(minion.transform.position, minion.target.transform.position) < minion.separationRadius)
            {
                fsm.ChangeState(NPCStates.Idle, minion.target.transform.position);
            }
        }
        if (!HasToUseObstacleAvoidance())
        {
            Vector3 vec = Arrive(minion.target.transform.position);
            AddForce(vec);
        }
        Separation(minion.redNPC ? GameManager.Instance.redNPC : GameManager.Instance.blueNPC);
        Move();

    }
    public Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, minion._maxSpeed);
    }

    public Vector3 Seek(Vector3 targetPos, float maxSpeed)
    {
        Debug.DrawLine(minion.transform.position, targetPos, Color.yellow);
        Vector3 vectorDeseado = targetPos - minion.transform.position;

        vectorDeseado.Normalize();
        vectorDeseado *= maxSpeed;

        Vector3 steering = vectorDeseado - minion.velocity;
        steering = Vector3.ClampMagnitude(steering, minion._maxForce * Time.deltaTime);


        return steering;

    }
    public virtual void Move()
    {

        minion.transform.position += minion.velocity * minion.speed * Time.deltaTime;
        minion.transform.forward = minion.velocity * minion.speed;
        AddForce(Separation(minion.redNPC ? GameManager.Instance.redNPC : GameManager.Instance.blueNPC));
        UpdateBoundPosition();
    }


    public bool HasToUseObstacleAvoidance()
    {
        Vector3 avoidance = ObstacleAvoidance();
        avoidance.z = 0;
        AddForce(avoidance * 5);
        return avoidance != Vector3.zero;
    }

    public Vector3 Arrive(Vector3 ship)
    {
        Vector3 desired = Vector3.zero;

        if (Vector3.Distance(minion.transform.position, ship) < minion.viewRadius)
            desired += ship;

        float dist = Vector3.Distance(minion.transform.position, desired);
        if (dist < minion.viewRadius)
        {
            return Seek(desired, minion._maxSpeed * (dist / minion.viewRadius));
        }
        else
        {
            return Seek(desired, minion._maxSpeed);
        }
    }
    public Vector3 Alignment(List<MinionNPC> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (MinionNPC item in agents)
        {
            if (Vector3.Distance(minion.transform.position, item.transform.position) > minion.viewRadius) continue;
            desired += item.velocity;
            boidCount++;
        }
        desired /= boidCount;
        return SteeringToAlignment(desired.normalized * minion._maxSpeed);
    }

    public Vector3 SteeringToAlignment(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - minion.velocity, minion._maxForce * Time.deltaTime);
    }

    public Vector3 Pursuit(MinionNPC agent)
    {
        Vector3 desired = Vector3.zero;
        desired += agent.transform.position + agent.velocity;

        return Seek(desired * minion._maxSpeed);
    }

    public Vector3 ObstacleAvoidance()
    {
        Debug.DrawLine(minion.transform.position + minion.transform.right * minion.size, minion.transform.position + minion.transform.right * minion.size + minion.transform.forward * minion.dodgeRadius, Color.green);
        Debug.DrawLine(minion.transform.position - minion.transform.right * minion.size, minion.transform.position - minion.transform.right * minion.size + minion.transform.forward * minion.dodgeRadius, Color.green);

        if (Physics.Raycast(minion.transform.position + minion.transform.up * minion.size, minion.transform.forward, minion.dodgeRadius, minion.obstacleLayer))
        {
            return Seek(minion.transform.right - minion.transform.up * 1, minion._maxSpeed);
        }
        else if (Physics.Raycast(minion.transform.position - minion.transform.up * minion.size, minion.transform.forward, minion.dodgeRadius, minion.obstacleLayer))
        {
            return Seek(minion.transform.right + minion.transform.up, minion._maxSpeed);
        }

        return Vector3.zero;
    }

    protected Vector3 Cohesion(List<MinionNPC> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (MinionNPC item in agents)
        {
            if (item == minion) continue;
            Vector3 dist = item.transform.position - minion.transform.position;
            if (dist.sqrMagnitude > minion.viewRadius * minion.viewRadius) continue;
            boidCount++;
            desired += item.transform.position;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired /= boidCount;

        return Seek(desired);
    }

    protected Vector3 Separation(List<MinionNPC> agents)
    {
        Vector3 desired = Vector3.zero;
        int boidCount = 0;

        foreach (MinionNPC item in agents)
        {
            if (item == minion) continue;
            Vector3 dist = item.transform.position - minion.transform.position;
            if (dist.sqrMagnitude > minion.separationRadius * minion.separationRadius) continue;
            boidCount++;
            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;

        desired *= -1;

        return SteeringToAlignment(desired.normalized * minion._maxSpeed);
    }

    public void AddForce(Vector3 force)
    {
        force.z = 0;
        minion.velocity.z = 0;
        minion.velocity = Vector3.ClampMagnitude(force + minion.velocity, minion._maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(minion.transform.position, minion.viewRadius);
    }

    public void UpdateBoundPosition()
    {
        //transform.position = GameManager.Instance.AdjustPositionsToBounds(transform.position);
    }
    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.wallMask);
    }
}
