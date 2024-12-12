using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBackToPatrol : State
{
    public float speed;
    List<Node> _path;

    public override void OnEnter(Vector3 target)
    {
        _path = GameManager.Instance.pf.AStar(GetNearestNode(), GetNearestNodeToTarget(target));
    }

    public Node GetNearestNode()
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToNode = Vector3.Distance(node.transform.position, leader.transform.position);
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
        if (leader.GetTargetPlayer() != null)
        {
            fsm.ChangeState(EnemyState.Follow, leader.GetTargetPlayer().transform.position);
        }
        if (_path == null || _path.Count <= 0) fsm.ChangeState(EnemyState.Patrol, new Vector3(0, 0, 0));
        if (_path != null && _path.Count != 0)
        {
            Vector3 dir = _path[0].transform.position - leader.transform.position;
            dir.y = 0;
            if (leader.GetWayPoints().Contains(_path[0]) && dir.magnitude <= 0.01)
            {
                
                leader.SetWayPointNumber(leader.GetWayPoints().IndexOf(_path[0]));
                fsm.ChangeState(EnemyState.Patrol, new Vector3(0, 0, 0));
            } 
            if (dir.magnitude <= 0.01)
            {
                _path.RemoveAt(0);

            }
            else
            {
                leader.Move(_path[0].transform.position - leader.transform.position);
            }

        }
    }

    public void SetPath(List<Node> path)
    {
        _path = path;
        _path?.Reverse();
    }
}
