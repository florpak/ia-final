using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GameObject[,] _grid;
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] GameObject _nodePrefab;
    [SerializeField] float _offset;
    [SerializeField] LayerMask wallLayer;
    
    void Start()
    {
        List<Node> listNodes = new List<Node>();

        _grid = new GameObject[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject newNode = Instantiate(_nodePrefab, transform);
                newNode.GetComponent<Node>().Initialize(x, y, new Vector3(x + x * _offset, y + y * _offset, 0), this, wallLayer);
                _grid[x, y] = newNode;
                listNodes.Add(newNode.GetComponent<Node>());
            }
        }

        GameManager.Instance.SetNode(listNodes);
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return null;
        return _grid[x, y].GetComponent<Node>();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
