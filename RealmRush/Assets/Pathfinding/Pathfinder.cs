using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is attached to the object pool.
 * 
 * This script is responsible for helping the object pool pathfind to its destination. For that reason,
 * this script will be used to create various pathfinding methods to spice up the game
 */

//TODO: Script only allows for one type of pathfinding. There should be two other types: manual pathing & direct pathing
//TODO: add ways to swap the pathfinding from automatic, manual, and direct
public class Pathfinder : MonoBehaviour
{
    //Serialized fields
    [SerializeField] Vector2Int startCoordinates;
    //Property to get the starting coordinates
    public Vector2Int StartCoordinates {  get { return startCoordinates; } }

    [SerializeField] Vector2Int endCoordinates;
    //Property to get the end coordinates
    public Vector2Int EndCoordinates { get { return endCoordinates; } }

    //Cashe references
    TileNode startNode;
    TileNode endNode;
    TileNode currentSearchNode;
    GridManager gridManager;

    //Attributes
    //left, right, up, and down Vector2Int is preexisting unit Vector2Int
    Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};

    Dictionary<Vector2Int, TileNode> currentGrid = new Dictionary<Vector2Int, TileNode>();
    Dictionary<Vector2Int, TileNode> reached = new Dictionary<Vector2Int, TileNode>();

    Queue<TileNode> frontier = new Queue<TileNode>();
    
    //Event Systems
    void Awake()
    {
        //get grid manager
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager != null)
        {
            //set currentGrid to the Grid in gridManager
            currentGrid = gridManager.Grid;
            startNode = currentGrid[startCoordinates];
            endNode = currentGrid[endCoordinates];
        }
        else
        {
            Debug.Log("Missing grid manager");
        }
    }

    void Start()
    {
        GetNewPath();
    }

    //Public Methods
    public List<TileNode> GetNewPath()
    {
        //Get path form starting coordinates
        return GetNewPath(startCoordinates);
    }
    public List<TileNode> GetNewPath(Vector2Int coordinates)
    {
        //Reset the path
        gridManager.ResetNodes();

        //UseBreadthFirstSearch from the given coordinates
        BreadthFirstSearch(coordinates);

        //Build the path based on the breadth first search
        return BuildPath();
    }
    public void NotifyReceivers()
    {
        //Tell any script attached to this gameObject to recalculate the enemy path.
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
    public bool WillBlockPath(Vector2Int coordinates)
    {
        //Test if whatever action is about to happen will preven the object from ever reaching the path

        if (currentGrid.ContainsKey(coordinates))
        {
            //Save the TileNode's previous state before changing it
            bool previousState = currentGrid[coordinates].isWalkable;

            //Test what happens if the TileNode is no longer walkable
            currentGrid[coordinates].isWalkable = false;

            List<TileNode> newPath = GetNewPath();

            //Reset the TileNode back to its previous state
            currentGrid[coordinates].isWalkable = previousState;

            //If there is no path, this action will block the path
            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }
        //By default, return false
        return false;
    }

    //Private Methods
    void ExploreNeighbors()
    {
        //Get the neighbors of the current node
        List<TileNode> neighbors = new List<TileNode>();

        //For each direction, if the coordinate exists in the grid, store the neighboring TileNode in the list of neighbors
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = currentSearchNode.coordinates + direction; //right, left, up, down

            if (currentGrid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(currentGrid[neighborCoordinates]);
            }
        }

        //For each neighbor TileNode, if the TileNode is not in the reached dictionary & the TileNode is walkable,
        //connect it to the current search node, add it to the reached dictionary, & add it to the queue
        foreach (TileNode neighbor in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int coordinates)
    {
        //Set the start & end node to walkable. This must always be true
        startNode.isWalkable = true;
        endNode.isWalkable = true;

        //Clear whatever may be in queue
        frontier.Clear();

        //Clear whatever node was reached (Need to research)
        reached.Clear();

        //Find the path
        bool isFindingPath = true;

        //Add the TileNode from the given coordinates to the queue
        frontier.Enqueue(currentGrid[coordinates]);
        
        //Current TileNode has been reached
        reached.Add(coordinates, currentGrid[coordinates]);

        //While there are TileNodes in queue & searching for path...
        while (frontier.Count > 0 && isFindingPath == true)
        {
            //currently searching through the TileNodes in queue
            currentSearchNode = frontier.Dequeue();

            //Set the node to explored
            currentSearchNode.isExplored = true;

            //Explore the current node's neighbors
            ExploreNeighbors();

            //If the current search node is the destination coordinate, no longer finding the path
            if (currentSearchNode.coordinates == endCoordinates)
            {
                isFindingPath = false;
            }
        }
    }

    List<TileNode> BuildPath()
    {
        //Generate a new list for the path
        List<TileNode> path = new List<TileNode>();

        TileNode currentNode = endNode;

        //Add the current node to the path
        path.Add(currentNode);

        //The current node is part of the path
        currentNode.isPath = true;

        //While the current node is connected to a TileNode
        while (currentNode.connectedTo != null)
        {
            //Current node is the one its connected to
            currentNode = currentNode.connectedTo;

            //Add the node to the path
            path.Add(currentNode);

            //The current node is the path
            currentNode.isPath = true;
        }

        //Reverse the path to go from start TileNode to end TileNode
        path.Reverse();
        return path;
    }

}
