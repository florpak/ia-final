using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionNPC : MonoBehaviour
{
    [SerializeField] protected float _maxSpeed = 2;
    [SerializeField] float maxForce;
    [SerializeField] protected float viewRadius, separationRadius;
    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] Transform leader;
    [SerializeField] float followRadius;
    [SerializeField] float moveSpeed;

    public Vector3 velocity;

    private void Update()
    {
        LeaderFollowing(leader);
        
    }

    public Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = targetPos - transform.position; // forma de calcular direcciones

        //Debug.DrawLine(transform.position, targetPos);

        desired.Normalize();
        desired *= speed;

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce * Time.deltaTime); //Hace que el movimiento no sea brusco

        return steering;
    }

    public void LeaderFollowing(Transform targetPos)
    {
        Vector3 leader = targetPos.position - transform.position;
        if (leader.magnitude > followRadius)
        {
            Vector3 arriveForce = leader.normalized * moveSpeed;
            //Separation(GameManager.Instance.minions, arriveForce);
            transform.position += arriveForce * Time.deltaTime;
        }
    }

    public Vector3 Separation(List<MinionNPC> minions, Vector3 arriveForce)
    {
        Vector3 desired = arriveForce;

        foreach (MinionNPC item in minions)
        {
            if (item == this) continue;// Me salteo

            Vector3 dist = item.transform.position - transform.position;

            if (dist.sqrMagnitude > separationRadius * separationRadius) continue;//Salteo a los que estan lejos

            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;//Si no hay agentes devulvo 0

        desired *= -1;//Invierto el vector

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 CalculateSteering(Vector3 desired) // Calcula el steering con un desire
    {
        return Vector3.ClampMagnitude(desired - velocity, maxForce * Time.deltaTime);
    }

    protected Vector3 ObstacleAvoidance()
    {
        //Hago seek hacia la direccion opuesta al obstaculo
        if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.right, viewRadius, obstacleLayer))
            return Seek(transform.position - transform.up, _maxSpeed);
        else if (Physics.Raycast(transform.position - transform.up * 0.5f, transform.right, viewRadius, obstacleLayer))
            return Seek(transform.position + transform.up, _maxSpeed);

        return Vector3.zero;
    }
}
