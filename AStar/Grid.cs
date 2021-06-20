using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Transform StartPosition;
    private LayerMask WallMask;

    public static List<Node[,]> NodeArray = new List<Node[,]>();
    public List<List<Node>> FinalPath = new List<List<Node>>();

    int xna = 0;
    bool xnabool = false;
    int yna = 0;
    bool ynabool = false;

    int nrooms;
    /*
    void Start()
    {
        nrooms = BoardManager.nroomsPublic;
        Debug.Log("nrooms = " + nrooms);
        CreateGrid(nrooms);
    }

    // Update is called once per frame
    void Update()
    {
        Enemy.publicFinalPath = FinalPath;
    }
    */
    void CreateGrid(int boardn)
    {
        WallMask = LayerMask.GetMask("Obstacle");

        for(int r = 0; r < boardn; r++)
        {
            NodeArray.Add(new Node[BoardManager.dimension, BoardManager.dimension]);

            Debug.Log("Entra en el for");

            for (int x = (int)BoardManager.usedPositions[r].x; x < (int)BoardManager.usedPositions[r].x + BoardManager.dimension; x++)
            {
                if (xnabool) xna++;
                for (int y = (int)BoardManager.usedPositions[boardn].y; y < (int)BoardManager.usedPositions[boardn].y + BoardManager.dimension; y++)
                {
                    if (ynabool) yna++;
                    Vector3 worldPoint = new Vector3(x, y, 0);
                    bool Wall = true;

                    if (Physics2D.OverlapCircle(worldPoint, 0.49f, WallMask)) Wall = false;

                    NodeArray[boardn][xna, yna] = new Node(Wall, worldPoint, xna, yna);

                    Debug.Log("Nodo, posición lista x = " + xna + ", posición lista y = " + yna + ", posición x = " + x + ", posición y = " + y );

                    ynabool = true;
                }
                xnabool = true;
                yna = 0;
                ynabool = false;
            }
            xna = 0;
            yna = 0;
            xnabool = false;
            ynabool = false;
        }  
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode, int gridN)
    {
        List<Node> NeighborList = new List<Node>();
        int icheckX;
        int icheckY;

        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < BoardManager.dimension)
        {
            if (icheckY >= 0 && icheckY < BoardManager.dimension) NeighborList.Add(NodeArray[gridN][icheckX, icheckY]);
        }

        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < BoardManager.dimension)
        {
            if (icheckY >= 0 && icheckY < BoardManager.dimension) NeighborList.Add(NodeArray[gridN][icheckX, icheckY]);
        }

        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < BoardManager.dimension)
        {
            if (icheckY >= 0 && icheckY < BoardManager.dimension) NeighborList.Add(NodeArray[gridN][icheckX, icheckY]);
        }

        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < BoardManager.dimension)
        {
            if (icheckY >= 0 && icheckY < BoardManager.dimension) NeighborList.Add(NodeArray[gridN][icheckX, icheckY]);
        }

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                icheckX = a_NeighborNode.iGridX + x;
                icheckY = a_NeighborNode.iGridY + y;

                if (icheckX >= 0 && icheckX < BoardManager.dimension && icheckY >= 0 && icheckY < BoardManager.dimension)
                {
                    NeighborList.Add(NodeArray[gridN][icheckX, icheckY]);
                }
            }
        }
        return NeighborList;
    }

    public Node NodeFromWorldPoint(Vector3 a_vWorldPos, int gridN)
    {
        float ixPos = (a_vWorldPos.x + BoardManager.dimension / 2) / BoardManager.dimension;
        float iyPos = (a_vWorldPos.y + BoardManager.dimension / 2) / BoardManager.dimension;

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((BoardManager.dimension - 1) * ixPos);
        int iy = Mathf.RoundToInt((BoardManager.dimension - 1) * iyPos);

        return NodeArray[gridN][ix, iy];
    }

    private void OnDrawGizmos()
    {
        if (NodeArray != null)
        {
            for (int r = 0; r < nrooms - 1; r++)
            {
                foreach (Node node in NodeArray[r])
                {
                    if (node.bIsWall) Gizmos.color = Color.white;

                    else Gizmos.color = Color.yellow;

                    if (FinalPath != null)
                    {
                        if (FinalPath[r].Contains(node)) Gizmos.color = Color.red;
                    }
                    Gizmos.DrawCube(node.vPosition, Vector3.one);
                }
            }
        }
    }
}
