using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyChase : State
{
    public float speed;
    List<Node> _path;
    Vector3 _target;

    public override void OnEnter(Vector3 target)
    {
        _target = target;
        _path = GameManager.Instance.pf.AStar(GetNearestNode(), GetNearestNodeToTarget(target));
    }

    public Node GetNearestNode()
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToNode = Vector3.Distance(node.transform.position, enemy.transform.position);
            if (nodeDistanceToNode < nearestDistance)
            {
                nearestDistance = nodeDistanceToNode;
                nearestNodeToTarget = node;
            }
        }
        return nearestNodeToTarget;
    }

    public Node GetNearestNodeToTarget(Vector3 target)
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToPlayer = Vector3.Distance(node.transform.position, target);
            if (nodeDistanceToPlayer < nearestDistance)
            {
                nearestDistance = nodeDistanceToPlayer;
                nearestNodeToTarget = node;
            }
        }
        return nearestNodeToTarget;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if(enemy.GetTargetPlayer()!= null)
        {
            fsm.ChangeState(EnemyState.Follow,enemy.GetTargetPlayer().transform.position);
        }
        if (_path != null && _path.Count > 0)
        {
            Vector3 dir = _path[0].transform.position - enemy.transform.position;
            dir.y = 0;
            if (dir.magnitude <= 0.01)
            {
                _path.RemoveAt(0);

            }
            else
            {
                enemy.Move(dir);
            }

        }
        if (_path == null || _path.Count <= 0)
        {
            Vector3 dir = _target-enemy.transform.position;
            dir.y = 0;
            if (dir.magnitude <= 0.01)
            {
                fsm.ChangeState(EnemyState.BackToPatrol, enemy.GetWayPoints()[enemy.GetWayPointNumber()].transform.position);
            }
            else
            {
                enemy.Move(dir);
            }
        }
    }

    public void SetPath(List<Node> path)
    {
        _path = path;
        _path?.Reverse();
    }
}
