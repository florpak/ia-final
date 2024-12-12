using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    int _x;
    int _y;
    Grid _grid;
    List<Node> _neighbors = new List<Node>();
    public float cost = 1;
    public bool isBlocked = false;
    [SerializeField] LayerMask wallLayer;


    public List<Node> GetNeighbors()
    {
        if (_neighbors.Count > 0)
        {
            return _neighbors;
        }
        Node neighbor;
        neighbor = _grid.GetNode(_x + 1, _y);
        if (neighbor != null && CheckNeighborNode(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x - 1, _y);
        if (neighbor != null && CheckNeighborNode(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y + 1);
        if (neighbor != null && CheckNeighborNode(neighbor)) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y - 1);
        if (neighbor != null && CheckNeighborNode(neighbor)) _neighbors.Add(neighbor);

        return _neighbors;
    }

    public bool CheckNeighborNode(Node neighbor)
    {
        if(!Physics.Raycast(transform.position, neighbor.transform.position - transform.position, (int)(transform.position - neighbor.transform.position).magnitude, wallLayer))
        {
            return true;
        }
        return false;
    }

    public void Initialize(int x, int y, Vector3 pos, Grid grid, LayerMask wallLayer)
    {
        this.wallLayer = wallLayer;
        this._x = x;
        this._y = y;
        transform.position = pos;
        this._grid = grid;
        gameObject.name = "Node: " + _x + "," + _y;
        SetCost(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        foreach (Node node in GetNeighbors())
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }

    public void SetCost(float newCost)
    {
        cost = Mathf.Clamp(newCost, 1, 99);
    }
}
