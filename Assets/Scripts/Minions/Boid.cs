using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : Agent
{
    [SerializeField] protected int fleeSpeed;

    void Start()
    {

        float x = Random.Range(-1f, 1);
        float z = Random.Range(-1f, 1f);
        Vector3 dir = new Vector3(x, 0, z);

        velocity = dir.normalized * _maxSpeed;
        //GameManager.Instance.agents.Add(this);
    }

    void Update()
    {


        if (!HasToUseObstacleAvoidance())
        {
            Vector3 force = Vector3.zero;

            if(GameManager.Instance.leader != null)
            {
                force += LeaderFollowing(GameManager.Instance.leader);
            }
            /*
            AddForce(Flee(GameManager.Instance.playerAgent) * fleeSpeed);
            foreach (FoodScript item in GameManager.Instance.food)
            {
                if (Vector3.Distance(transform.position, item.transform.position) > viewRadius) continue;
                AddForce(Arrive(GameManager.Instance.food) * 7);
            }

            AddForce(Alignment(GameManager.Instance.agents));
            AddForce(Cohesion(GameManager.Instance.agents));
            AddForce(Separation(GameManager.Instance.agents) * 3);
            */
        }
        Move();
    }
}
