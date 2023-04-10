using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float gridSize = 5f;
    [SerializeField] private float gridDiameter = 1f;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Debugging")]
    [SerializeField] private bool visualizeGridSpawn = false;
    [SerializeField] private float spawnDelay = 0.1f;
    private void Start()
    {
        if (visualizeGridSpawn)
        {
            StartCoroutine(SpawnGridWithDelay());
        }
        else
        {
            SpawnGrid();
        }
    }

    private void SpawnGrid()
    {
        // Calculate the number of rows and columns needed to fill the grid
        int gridRows = Mathf.RoundToInt(gridSize / gridDiameter);
        int gridCols = Mathf.RoundToInt(gridSize / gridDiameter);

        // Calculate the overall size of the grid perimeter
        float gridWidth = gridCols * gridDiameter;
        float gridHeight = gridRows * gridDiameter;

        // Position the grid parent object to center the grid in the scene
        Vector3 startPosition = transform.position - new Vector3(gridSize / 2f, gridSize / 2f, 0f) + new Vector3(gridDiameter / 2f, gridDiameter / 2f, 0f);

        // Create the individual grid cells
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                // Instantiate a new grid cell and position it based on its row and column index
                GameObject newGridCell = Instantiate(gridPrefab, gridParent);
                newGridCell.transform.position = new Vector3(col * gridDiameter, row * gridDiameter, 0f) + startPosition;

                // Resize the grid cell to match the specified diameter
                newGridCell.transform.localScale = new Vector3(gridDiameter, gridDiameter, 1f);
            }
        }
    }

    private IEnumerator SpawnGridWithDelay()
    {
        // Calculate the number of rows and columns needed to fill the grid
        int gridRows = Mathf.RoundToInt(gridSize / gridDiameter);
        int gridCols = Mathf.RoundToInt(gridSize / gridDiameter);

        // Calculate the overall size of the grid perimeter
        float gridWidth = gridCols * gridDiameter;
        float gridHeight = gridRows * gridDiameter;

        // Position the grid parent object to center the grid in the scene
        Vector3 startPosition = transform.position - new Vector3(gridSize / 2f, gridSize / 2f, 0f) + new Vector3(gridDiameter / 2f, gridDiameter / 2f, 0f);

        // Create the individual grid cells with a delay in between each instantiation
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                // Instantiate a new grid cell and position it based on its row and column index
                GameObject newGridCell = Instantiate(gridPrefab, gridParent);
                newGridCell.transform.position = new Vector3(col * gridDiameter, row * gridDiameter, 0f) + startPosition;

                // Resize the grid cell to match the specified diameter
                newGridCell.transform.localScale = new Vector3(gridDiameter, gridDiameter, 1f);

                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the individual grid cells
        Gizmos.color = Color.green;
        if (gridDiameter <= 0.2f) gridDiameter = 0.2f;
        Vector3 startPosition = transform.position - new Vector3(gridSize / 2f, gridSize / 2f, 0f) + new Vector3(gridDiameter / 2f, gridDiameter / 2f, 0f);
        for (float row = 0; row < gridSize; row += gridDiameter)
        {
            for (float col = 0; col < gridSize; col += gridDiameter)
            {
                Gizmos.DrawWireCube(startPosition + new Vector3(col, row, 0f), new Vector3(gridDiameter, gridDiameter, 0f));
            }
        }

        // Draw the outline of the grid perimeter
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize, gridSize, 0f));
    }
}