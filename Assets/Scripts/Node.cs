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
    [SerializeField] TextMeshProUGUI _textCost;


    public List<Node> GetNeighbors()
    {
        if (_neighbors.Count > 0)
        {
            return _neighbors;
        }
        Node neighbor;
        neighbor = _grid.GetNode(_x + 1, _y);
        if (neighbor != null) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x - 1, _y);
        if (neighbor != null) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y + 1);
        if (neighbor != null) _neighbors.Add(neighbor);
        neighbor = _grid.GetNode(_x, _y - 1);
        if (neighbor != null) _neighbors.Add(neighbor);

        return _neighbors;
    }
    public void Initialize(int x, int y, Vector3 pos, Grid grid)
    {
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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            
        }
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            isBlocked = true;

            //GameManager.Instance.PaintGameObject(gameObject, isBlocked ? Color.grey : Color.white);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SetCost(cost + 1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SetCost(cost - 1);
        }
    }

    public void SetCost(float newCost)
    {
        cost = Mathf.Clamp(newCost, 1, 99);
        //_textCost.text = cost.ToString();
        //_textCost.enabled = cost == 1 ? false : true;
    }
}
