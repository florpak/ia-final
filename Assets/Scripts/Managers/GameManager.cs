using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float width, height;
    public Agent leader;
    public List<Agent> agents;

    private Node _startingNode;
    private Node _goalNode;
    public Pathfinding pf;
    [SerializeField] private List<Node> allNodes = new List<Node>();
    public LayerMask wallMask;
    

    public static GameManager Instance;

    public void SetStartingNode(Node node)
    {
        if (_startingNode != null) PaintGameObject(_startingNode.gameObject, Color.white);
        _startingNode = node;
        PaintGameObject(_startingNode.gameObject, Color.green);
    }

    public void SetGoalNode(Node node)
    {
        if (_goalNode != null) PaintGameObject(_goalNode.gameObject, Color.white);
        _goalNode = node;
        PaintGameObject(_goalNode.gameObject, Color.red);
    }

    public void SetNode(List<Node> nodes)
    {
        allNodes = nodes;
    }

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Update()
    {
    }

    public void PaintGameObject(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }

    public List<Node> GetNodes()
    {
        return allNodes;
    }


    //Agents
    public Vector3 AdjustPositionsToBounds(Vector3 pos)
    {
        float boundWidth = width / 2;
        float boundHeight = height / 2;

        if (pos.x > boundWidth) pos.x = -boundWidth;
        if (pos.x < -boundWidth) pos.x = boundWidth;
        if (pos.z > boundHeight) pos.z = -boundHeight;
        if (pos.z < -boundHeight) pos.z = boundHeight;
        return pos;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 0, height));
    }
}
