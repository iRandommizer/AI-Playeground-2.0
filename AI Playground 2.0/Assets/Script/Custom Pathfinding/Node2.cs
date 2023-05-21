using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows me to change HeuristicType of experiemntational purposes
public enum HeuristicType { Manhattan, Euclidean, Diagonal };

public class Node2 : MonoBehaviour
{
    public int gridX; // X value of the node on the grid
    public int gridY; // Y value of the node on the grid
    public Vector3 nodePosition; // Position of the node

    public bool isWalkable; // See if it's walkable

    public int gCost; // To keep track of the gValue of the node
    public int hCost; // To keep track of the gValue of the node
    public int fCost { get { return gCost + hCost; } }

    public Node2 parent; // Keep track of where it came from

    [SerializeField]
    private HeuristicType heuristicType;

    public Node2(int x, int y,Vector3 nodePosition, bool isWalkable, HeuristicType heuristicType = HeuristicType.Manhattan)
    {
        this.gridX = x;
        this.gridY = y;
        this.nodePosition = nodePosition;
        this.isWalkable = isWalkable;
        this.heuristicType = heuristicType;
    }

    private int CalculateDistance(Node2 nodeA, Node2 nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (heuristicType == HeuristicType.Euclidean)
        {
            return (int)Mathf.Sqrt(dstX * dstX + dstY * dstY);
        }
        else if (heuristicType == HeuristicType.Diagonal)
        {
            int diagonalCost = Mathf.Min(dstX, dstY);
            int straightCost = Mathf.Abs(dstX - dstY);
            return 14 * diagonalCost + 10 * straightCost;
        }
        else
        {
            return 10 * (dstX + dstY);
        }
    }

    public int CalculateCost(Node2 node)
    {
        int distance = CalculateDistance(this, node);
        int cost = distance + node.gCost;
        return cost;
    }

    public void ResetCosts()
    {
        gCost = 0;
        hCost = 0;
    }
}
