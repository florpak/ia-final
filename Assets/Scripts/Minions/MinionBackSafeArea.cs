using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBackSafeArea : MinionStates
{
    public float speed;
    List<Node> _path;
    Vector3 _target;

    public override void OnEnter(Vector3 target)
    {
        _target = target;
        minion.currentState = NPCStates.BackToBase;
        _path = GameManager.Instance.pf.CalculateMove(GetNearestNode(), GetNearestNodeToTarget(_target));
    }

    public Node GetNearestNode()
    {
        float nearestDistance = Mathf.Infinity;
        Node nearestNodeToTarget = null;
        foreach (Node node in GameManager.Instance.GetNodes())
        {
            float nodeDistanceToNode = Vector3.Distance(node.transform.position, minion.transform.position);
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
        if (_path != null && _path.Count > 0)
        {
            Vector3 dir = _path[0].transform.position - minion.transform.position;
            dir.z = 0;

            if (dir.magnitude <= 0.5)
            {

                _path.RemoveAt(0);

            }
            else
            {

                minion.Move(dir);
            }

        }
        if (_path == null || _path.Count <= 0)
        {
            Vector3 dir = _target - minion.transform.position;
            dir.z = 0;
            if (dir.magnitude <= 0.1)
            {
                fsm.ChangeState(NPCStates.Follow, minion.target.transform.position);
            }
            else
            {
                minion.Move(dir);
            }
        }
    }

    public void SetPath(List<Node> path)
    {
        _path = path;
        _path?.Reverse();
    }
    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.wallMask);
    }
}
