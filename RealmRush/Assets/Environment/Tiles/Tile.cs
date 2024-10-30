using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable = true;
    //Create a property to get and set the isPlaceable boolean. Same as getter & setter methods

    GridManager gridManager;
    Pathfinder pathfinder;

    Vector2Int coordinates = new Vector2Int();
    public bool IsPlaceable
    {
        get { return isPlaceable; }
        set { isPlaceable = value; }
    }

    [SerializeField] Tower tower;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        if (gridManager.getTileNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            bool isPlaced = tower.CreateTower(tower, transform.position);
            IsPlaceable = !isPlaced;
            gridManager.BlockNode(coordinates);
        }
    }
}
