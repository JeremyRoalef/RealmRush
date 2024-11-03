using System.Collections.Generic;
using UnityEngine;

/*
 * This script is attached to the currentGrid manager gameObject
 *
 * This script will be responsible for holding the conceptual model of the tile currentGrid layout in code. 
 */

public class GridManager : MonoBehaviour
{
    //Serialized fields
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize = 10;
    //Property for unity currentGrid size
    public int UnityGridSize { get { return unityGridSize; } }

    //Attributes
    Dictionary<Vector2Int, TileNode> grid = new Dictionary<Vector2Int, TileNode>();
    //Property for currentGrid dictionary
    public Dictionary<Vector2Int, TileNode> Grid { get { return grid; } }

    //Event Systems
    private void Awake()
    {
        CreateGrid();
    }

    //Public Methods
    public void ResetNodes()
    {
        //for every key-value pair in teh currentGrid, reset the TileNodes
        foreach (KeyValuePair<Vector2Int, TileNode> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }
    public void BlockNode(Vector2Int coordinates)
    {
        //Block the TileNode at the given coordinates if it exists in the dictionary
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        //Convert the Vector2Int coordinates to a Vector3
        Vector3 position = new Vector3();

        //Rearrange equation in GetCoordinatesFromPosition to solve for position values
        position.x = unityGridSize * coordinates.x;
        position.z = unityGridSize * coordinates.y;

        return position;
    }
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        //Convert the Vector3 position to coordinates
        Vector2Int coordinates = new Vector2Int();

        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize); //position is multiplicative of the unity currentGrid size
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize); //the 2d layout in the game uses x,z coordinates, so the z coordinate will be stored in the second vector
        return coordinates;
    }
    public TileNode getTileNode(Vector2Int coordinates)
    {
        //If the currentGrid stores the coordinates, return teh TileNode at the given coordinates
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        else
        {
            return null;
        }
    }

    //Private Methods
    void CreateGrid()
    {
        //Create currentGrid of coordinates
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new TileNode(coordinates, true));
            }
        }
    }
}
