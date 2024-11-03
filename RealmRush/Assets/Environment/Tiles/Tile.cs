using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is attached to the Tile prefab
 * 
 * This script will work with the tiles in the world. Currently, the tile script is doing more than it should be, like instantiating towers,
 * and this script will have to be broken down to allow for more flexibility in the future. There are many tile types in the game, including
 * grass, shore, and water tiles that should eventually behave differently from each other.
 * 
 * This script talks to the GridManager and PathFinder script.
 */
public class Tile : MonoBehaviour
{
    //Serialized Fields
    [SerializeField] bool isPlaceable = true;

    //Property for isPlaceable attribute
    public bool IsPlaceable
    {
        get { return isPlaceable; }
        set { isPlaceable = value; }
    }
    //enum to store the type of tile this script is attached to
    enum TileType
    {
        Land,
        Shore,
        Water
    };
    [SerializeField] TileType tileType;
    [SerializeField] Tower tower;

    //Cashe regerences
    GridManager gridManager;
    Pathfinder[] pathfinders;

    //Attributes
    Vector2Int coordinates = new Vector2Int();

    //Event Systems
    void Awake()
    {
        //Find references in game scene
        gridManager = FindObjectOfType<GridManager>();
        pathfinders = FindObjectsOfType<Pathfinder>();
    }

    void Start()
    {
        //On the start of object, get the coordinates of this tile in the currentGrid manager & block this tile if isPlaceable is set to false
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable && !(gameObject.tag == "Trail"))
            {
                gridManager.BlockNode(coordinates);
            }
            //If the object's tag is trail, tell the grid manager the Tile Node at given coordinates is a trail node
            if (gameObject.tag == "Trail")
            {
                gridManager.SetTrail(coordinates, true);
            }
        }
        else
        {
            Debug.Log("You forgot the grid manager object");
        }
    }


    //TODO: Create a new script that allows the behavior of tower building in the game. Tile script should not be responsible for this action!
    void OnMouseDown()
    {
        //Run for each pathfinders game object
        foreach (Pathfinder pathfinder in pathfinders)
        {
            //If placing this tower will block this path, do not place the tower
            if (pathfinder.WillBlockPath(coordinates))
            {
                return;
            }
        }
        if (gridManager.getTileNode(coordinates).isWalkable && isPlaceable)
        {
            bool isSuccessful = tower.CreateTower(tower, transform.position);
            //If tower was successfully placed, tell each pathfinder to recalculate the path
            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                foreach (Pathfinder pathfinder in pathfinders)
                {
                    pathfinder.NotifyReceivers();
                }
            }
        }
    }

    //Public Methods

    //Private Methods
}
