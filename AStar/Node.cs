using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int iGridX;
    public int iGridY;

    public bool bIsWall;
    public Vector3 vPosition;

    public Node ParentNode;

    public int igCost;
    public int ihCost;

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool a_bIsWall, Vector3 a_vPos, int a_igridX, int a_igridY)
    {
        bIsWall = a_bIsWall;
        vPosition = a_vPos;
        iGridX = a_igridX;
        iGridY = a_igridY;
    }
}
