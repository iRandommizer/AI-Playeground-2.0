using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;

    Grid grid; // Reference to the grid

    // Make a temp variable
    List<Vector2> tempWayPoints = new List<Vector2>();

    IEnumerator instance = null; // Holder for coroutines to be easily started ot stopped

    void Awake()
    {
        #region Caching
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>(); 
        #endregion
    }

    // Find the path
    IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
    {
        #region Stop Watch Start
        // To help track how long the code takes to run
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        #endregion

        // New vector2 array for the waypoints
        Vector2[] waypoints = new Vector2[0];

        // Set the pathsuccess to false first since it's just the beginning
        bool pathSucess = false;

        // Find the grid of the start point and target point
        Node startNode = grid.NodeFromWolrdPoint(startPos);
        Node targetNode = grid.NodeFromWolrdPoint(targetPos);

        // Make sure that the start and end node are possible to reach
        if(startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Set of nodes to be evaluated
            HashSet<Node> closedSet = new HashSet<Node>(); // Set of nodes already evaluated

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // Current node is the first once in the openset
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // Path is already found
                if (currentNode == targetNode)
                {
                    #region Stop Watch Stop
                    sw.Stop();
                    //!!print("Path found: " + sw.ElapsedMilliseconds + " ms"); 
                    #endregion
                    pathSucess = true;
                    break;
                }

                // This segment is the part where we update the fcosts of the nodes that we get back to 
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    // If neighbour is not walkable or is in the closedSet, then skip the node from being checked
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                    // Recalculate the neighbour node which may seem like a good path
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenaltyValue;

                    // If the neighbour node has potential to be a better path
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        // Set costs and parent node
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour); // Brand new node
                        else openSet.UpdateItem(neighbour); // Open node
                    }
                }

            }
        }
        
        yield return new WaitForEndOfFrame();

        // If path was found
        if (pathSucess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSucess = waypoints.Length > 0;
            //print("Path Found");
        }
        else
        {
            // Need to update the path again, which means I need to some how retrive the new position of the client
            print("Path Not Found");
            FindPath(startPos, targetPos);
        }
        // Just informs the queue handler that the current path has been processed
        requestManager.FinishProcessingPath(waypoints, pathSucess);
    }

    Vector2[] RetracePath(Node startnode, Node endNode)
    {
        List<Node> path = new List<Node>(); // Make new list of nodes to add the path nodes in the list
        Node currentNode = endNode; // Make the current node the last node and work back from their 

        // Loops through from the end node till the first node
        while(currentNode != startnode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent; // Since the parent nodes refers to the previos node inside the path, there will be a chain of nodes making an entire path
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    // Goes through all the points found in the original findpath function and eliminates any redudant points
    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>(); // New list which will hold the new cleaned up version of the points
        Vector2 directionOld = Vector2.zero; // Just to keep track of the direction of the previous point to the current point 

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            // Only if there has been a change in path and then add it inside the new list
            if (directionNew != directionOld) // Path has changed direction
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        StartCoroutine(displaySimplificationOfPath(waypoints));
        return waypoints.ToArray(); // Turn List into array
    }

    // LMao this function was useless, gotta remove it
    IEnumerator displaySimplificationOfPath(List<Vector2> wayPoints)
    {
        //!!Temp        
        tempWayPoints = new List<Vector2>();

        foreach (Vector2 point in wayPoints)
        {
            tempWayPoints.Add(point);
        }
        yield return new WaitForSeconds(0.5f);
    }

    // This funtion basically gets the distance between 2 positions as pernormal but it can 
    //only do by going 45degrees diagonally and then vertical or horizontal, until it gets to the target position
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }

    public void StartFindPath(Vector2 startPos, Vector2 targetPos) 
    {
        instance = FindPath(startPos, targetPos);
        StartCoroutine(instance);
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //foreach (Vector2 p in tempWayPoints)
        //{
        //    Gizmos.DrawCube(p, Vector3.one * 0.5f);
        //}
    }
}
