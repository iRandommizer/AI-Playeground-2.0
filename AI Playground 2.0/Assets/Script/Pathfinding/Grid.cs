using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos; // Boolean to easily control if we want the gizmos to be displayed or not
    public LayerMask unwalkableMask; // Layermask for the unwalkable layer
    public Vector2 gridWorldSize; // Width and Length of the whole grid in Vector2 form
    public float nodeRadius; // The radius of the grid

    public TerrainType[] walkableRegions; // For defining the different walkable regions agents can walk on
    public int obstacleProximityPenalty = 10; // Penalty for walking near obstacles
    LayerMask walkableMask; // The layermask holding all the different layers that can be walked on
    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>(); // Have a key pair relationship with the layer and the penality given to that layer

    Node[,] grid; // It's a 2 dimensional array which means it has multiple arrays in this grid array

    float nodeDiameter; // Holds the value of the diameter for easier acess
    int gridSizeX, gridSizeY; // Holds the value of the x and y value of the grid size

    int penaltyMin = int.MaxValue; // This is useful when trying to replace something using greater than to compare 
    int penaltyMax = int.MinValue; // This is useful when trying to replace something using lesser than to compare

    public int MaxSize { get {return gridSizeX * gridSizeY; } } // Helper property to get the size of the grid in nodes

    private void Awake()
    {
        // Caching, set the values of the various variables
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Updated the layermask to know layers are walkable and how "walkable" they are
        foreach(TerrainType region in walkableRegions)
        {
            walkableMask.value |= region.terrainMask.value; // Add the current region layer value to the layermask's value (adding based on it's bits)
            walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty); // To populate the dictionaries values appropriately
        }

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY]; // 2 dimensional array of Nodes to create each grid
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2; // find the bottomleft of the world

        // Populate the grid itself and the information about it's nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Set the positions of each node
                Vector2 worldPoint = worldBottomLeft 
                    + Vector2.right * (x * nodeDiameter + nodeRadius) 
                    + Vector2.up * (y * nodeDiameter + nodeRadius);

                // Check if there are any colliders within the node itself that is layerd on the unwalkable layer
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius * 0.95f,unwalkableMask));

                int movementPenalty = 0; // Setting the movement penalty

                // Shoot a ray from the "top"(z axis top) of the map towards the map to detect any obects that has different layers
                RaycastHit2D hit = Physics2D.Raycast((Vector3)worldPoint - Vector3.forward * 2, Vector3.forward, 5, walkableMask);

                #if UNITY_EDITOR
                //Debug.DrawRay((Vector3)worldPoint - Vector3.forward * 2, Vector3.forward * 5, Color.black, 5f);
                #endif

                // If a walkabe layer was hit, try to get the value of the movement penality of that node according to it's layer
                if (hit)
                {
                    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                }

                // if not walk able, increase the movement penality
                if (!walkable)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                // Set the node's value on the respective Node Array's indices
                grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }

        // Blur the map
        BlurPenaltyMap(4);
    }

    // Find desired node based on world pos
    public Node NodeFromWolrdPoint(Vector2 worldPosition)
    {
        //Eg. (2 + 4) / 8 = 0.75 
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        // We just want to make sure that we don't have a weird value that is below 0 or above 1 for the percentage
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
 
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void BlurPenaltyMap(int blurSize)
    {
        // Defition of Kernel https://en.wikipedia.org/wiki/Kernel, look for image processing
        int kernelSize = blurSize * 2 + 1; // The kernel is a temporary smaller grid from our larger grid, which is used to compare values to evaluate the value of the centre grid
        int kernelExtents = (kernelSize - 1) / 2; // The extra grid boxes outside the larger grid

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenaltyValue;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0 , gridSizeX -1);

                penaltiesHorizontalPass[x, y] 
                    = penaltiesHorizontalPass[x - 1, y]
                    - grid[removeIndex, y].movementPenaltyValue
                    + grid[addIndex, y].movementPenaltyValue;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].movementPenaltyValue = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y]
                    = penaltiesVerticalPass[x, y-1]
                    - penaltiesHorizontalPass[x, removeIndex]
                    + penaltiesHorizontalPass[x , addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].movementPenaltyValue = blurredPenalty;

                if(blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if(blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    // Helper function to get the neighouring nodes
    public List<Node> GetNeighbours(Node mainNode)
    {
        List<Node> neighbours = new List<Node>();

        // Loop through x-1, x and x + 1 to get horizontal neighbours
        for (int x = -1; x <= 1; x++)
        {
            // Loop through y-1, y and y + 1 to get vertical neighbours
            for (int y = -1; y <= 1; y++)
            {
                // If it's currently at the mainnode, skip it
                if (x == 0 && y == 0) continue;

                // Add the offset values based on the loop occurance to the main node x and y values
                int checkX = mainNode.gridX + x;
                int checkY = mainNode.gridY + y;

                // Just to make sure that we aren't getting any node's past the edges of the grid
                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenaltyValue));

                Gizmos.color = (n.walkable) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector2.one * nodeDiameter);
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
