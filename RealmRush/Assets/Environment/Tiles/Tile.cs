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
    [SerializeField] Tower tower;

    //Cashe regerences
    GridManager gridManager;
    Pathfinder pathfinder;

    //Attributes
    Vector2Int coordinates = new Vector2Int();


    private void Awake()
    {
        //Find references in game scene
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        //On the start of object, get the coordinates of this tile in the currentGrid manager & block this tile if isPlaceable is set to false
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
        else
        {
            Debug.Log("You forgot the grid manager object");
        }
    }


    //TODO: Create a new script that allows the behavior of tower building in the game. Tile script should not be responsible for this action!
    private void OnMouseDown()
    {
        //Run only if placing this tower will not break the pathfinding
        if (gridManager.getTileNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            bool isSuccessful = tower.CreateTower(tower, transform.position);
            if (isSuccessful)
            {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
            }
        }
    }
}
