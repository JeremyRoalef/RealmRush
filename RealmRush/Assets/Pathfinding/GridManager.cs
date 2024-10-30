using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize = 10;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, TileNode> grid = new Dictionary<Vector2Int, TileNode>();
    public Dictionary<Vector2Int, TileNode> Grid { get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new TileNode(coordinates, true));
            }
        }
    }

    public TileNode getTileNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        else
        {
            return null;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize); //position is multiplicative of 10, but can use the grid snapp setting
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize); //the 2d layout in the game uses x,z coordinates, so the z coordinate will be stored in the second vector
        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = unityGridSize * coordinates.x;
        position.y = unityGridSize * coordinates.y;

        return position;
    }
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, TileNode> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
}
