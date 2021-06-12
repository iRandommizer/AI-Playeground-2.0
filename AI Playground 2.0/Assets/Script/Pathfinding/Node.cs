using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IHeapItem<Node>
{
    public bool walkable; // Boolean to know if the node is walkable or not
    public Vector2 worldPosition; // Variable to store the world position of the node
    public int gridX; // Variable to store the x value according to the grid
    public int gridY; // Variable to store the y value according to the grid

    public int movementPenaltyValue;

    public int gCost; // Cost which represents how far away is the current node from the beginning of the beginning node accordning to the existing path
    public int HCost; // Cost which represents how far away is the currnet node from the end node (straight)
    public Node parent; // Reference to the main node at the center of all it's sibling neighbour nodes
    int heapIndex;

    public int fCost { get { return gCost + HCost; } }

    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movementPenaltyValue = _penalty;
    }

    public int HeapIndex { get { return heapIndex; } set {heapIndex = value;} }

    public int CompareTo(Node nodeToCompare)
    {
        // Where O = origin node, C = compared node
        // O.Fcost > C.Fcost then "compare" = 1     | C is a better path
        // O.Fcost < C.Fcost then "compare" = -1    | O is a better path
        // O.Fcost == C.Fcost then "compare" = 0
        int compare = fCost.CompareTo(nodeToCompare.fCost); // if fcost of origin node is greater than the fcost of compared node, compare's value = 1
        if (compare == 0) // Fcosts have the same value
        {
            compare = HCost.CompareTo(nodeToCompare.HCost); // if hcost of origin node is greater than the hcost of compared node, compare's value = 1
        }
        // Reverse the compare value
        return -compare;
        //Hence if compare = -1, C is a higer priority. If compare = 1, then O is a higher priority
    }
}
