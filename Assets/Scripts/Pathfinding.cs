using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    float HeuristicDistance(Vector3 a, Vector3 b)
    {
        Vector3 distance = b - a;
        return distance.magnitude;
    }

    public List<Node> AStar(Node start, Node goal)
    {
        if (start == null || goal == null) return null;
        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Get();
            //GameManager.Instance.PaintGameObject(current.gameObject, Color.blue);
            if (current == goal)
            {

                Debug.Log("Llegu� a la meta");

                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }
                path.Reverse();
                return path;
            }

            foreach (var next in current.GetNeighbors())
            {
                if (next.isBlocked) continue;
                float distance = HeuristicDistance(next.transform.position, goal.transform.position);
                float newCost = costSoFar[current] + next.cost;
                float priority = distance + newCost;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Put(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else
                {
                    if (newCost < costSoFar[next])
                    {
                        frontier.Put(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }
                }
            }
        }
        return null;
    }
}