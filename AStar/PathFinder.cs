using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    Grid GridReference;
    private Transform StartPosition;
    private Transform TargetPosition;

    private int room = 0;
    private int nrooms;


    void Start()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        StartPosition = enemy.transform;
        nrooms = BoardManager.nroomsPublic;
        GridReference = GetComponent<Grid>();
    }

    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        TargetPosition = target.transform;
        FindPath(StartPosition.position, TargetPosition.position);
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos, room);
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos, room);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode) GetFinalPath(StartNode, TargetNode);

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode, room))
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode)) continue;

                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode)) OpenList.Add(NeighborNode);
                }
            }
        }
    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<List <Node>> FinalPath = new List<List<Node>>();
        Node CurrentNode = a_EndNode;

        for (int r = 0; r < nrooms; r++)
        {
            while (CurrentNode != a_StartingNode)
            {
                FinalPath[r].Add(CurrentNode);
                CurrentNode = CurrentNode.ParentNode;
            }
        }

        FinalPath.Reverse();

        GridReference.FinalPath = FinalPath;

        /*for (int i = 0; i < FinalPath.Count; i++)
        {
            Debug.Log("FinalPath[" + i + "] su posicion es " + FinalPath[i].vPosition);
        }*/
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);

        return ix + iy;
    }
}
