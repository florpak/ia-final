using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<MinionNPC> minions = new List<MinionNPC>();
    private Node _startingNode;
    private Node _goalNode;
    public Pathfinding pf;
    [SerializeField] private List<Node> allNodes = new List<Node>();
    public LayerMask wallMask;
    public List<MinionNPC> redNPC;
    public List<MinionNPC> blueNPC;
    public GameObject redBase;
    public GameObject blueBase;


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
}
