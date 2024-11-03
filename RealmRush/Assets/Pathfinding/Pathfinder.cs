using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is attached to the object pool.
 * 
 * This script is responsible for helping the object pool pathfind to its destination. For that reason,
 * this script will be used to create various pathfinding methods to spice up the game
 * 
 * this script highlights the importance of knowing what you want to add before adding it. The videos were throwing solutions
 * to the problem without defining the problem from the beginning, and this code is spagghettified for it.
 */

//TODO: Script only allows for one type of pathfinding. There should be two other types: manual pathing & direct pathing
//TODO: add ways to swap the pathfinding from automatic, manual, and direct
public class Pathfinder : MonoBehaviour
{
    //Enum
    public enum PathfindingMethod
    {
        FindOwnPath,
        FollowTrail,
        DirectPathing
    }

    //Serialized fields
    [Tooltip("How will the object pool pathfind to its location? Will the pool find its own path? An existing path? or directly head to the location?")]
    [SerializeField] PathfindingMethod pathfinding;
    //Property to get the pathfinding method
    public PathfindingMethod Pathfinding
    {
        get { return pathfinding; }
    }
    [SerializeField] Vector2Int startCoordinates;
    //Property to get the starting startingCoordinates
    public Vector2Int StartCoordinates {  get { return startCoordinates; } }

    [SerializeField] Vector2Int endCoordinates;
    //Property to get the end startingCoordinates
    public Vector2Int EndCoordinates { get { return endCoordinates; } }

    //Cashe references
    TileNode startNode;
    TileNode endNode;
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
        switch (pathfinding)
        {
            case PathfindingMethod.FollowTrail:
                FollowTrail();
                break;
            case PathfindingMethod.FindOwnPath:
                FindOwnPath();
                break;
            default:
                break;
        }

    }

    //Public Methods
    public List<TileNode> FindOwnPath()
    {
        //Get path form starting startingCoordinates
        return FindOwnPath(startCoordinates);
    }

    public List<TileNode> FindOwnPath(Vector2Int coordinates)
    {
        //Reset the path
        gridManager.ResetNodes();

        //UseBreadthFirstSearch from the given startingCoordinates
        BreadthFirstSearch(coordinates);

        //Build the path based on the breadth first search
        return BuildPath();
    }
    public List<TileNode> FollowTrail()
    {
        startNode.isWalkable = true;
        endNode.isWalkable = true;

        //Reset the path
        gridManager.ResetNodes();

        //Clear anything that may be in queue
        reached.Clear();


        frontier.Enqueue(startNode);

        reached.Add(startCoordinates, currentGrid[startCoordinates]);

        bool isFindingPath = true;

        TileNode currentSearchNode;
        while (isFindingPath && frontier.Count > 0)
        {
            currentSearchNode = frontier.Dequeue();

            //Check neighbors for trail nodes
            ExploreNeighbors(currentSearchNode);

            //If the current search node is the destination coordinate, no longer finding the path
            if (currentSearchNode.coordinates == endCoordinates)
            {
                isFindingPath = false; //found the path
            }
        }

        //build the path
        return BuildPath();
    }
    public void NotifyReceivers()
    {
        //Tell any script attached to this gameObject to recalculate the enemy path.
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
    public bool WillBlockPath(Vector2Int coordinates)
    {
        //Test if whatever action is about to happen will prevent the object from ever reaching the path

        if (currentGrid.ContainsKey(coordinates))
        {
            //Save the TileNode's previous state before changing it
            bool previousState = currentGrid[coordinates].isWalkable;

            //Test what happens if the TileNode is no longer walkable
            currentGrid[coordinates].isWalkable = false;

            List<TileNode> newPath = FindOwnPath();

            //Reset the TileNode back to its previous state
            currentGrid[coordinates].isWalkable = previousState;

            //If there is no path, this action will block the path
            if (newPath.Count <= 1)
            {
                FindOwnPath();
                return true;
            }
        }
        //By default, return false
        return false;
    }

    //Private Methods
    void BreadthFirstSearch(Vector2Int startingCoordinates)
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

        //Add the starting TileNode from the given startingCoordinates to the queue
        frontier.Enqueue(currentGrid[startingCoordinates]);
        
        //Starting TileNode has been reached. Add to dictionary of reached nodes
        reached.Add(startingCoordinates, currentGrid[startingCoordinates]);

        TileNode currentSearchNode;

        //While there are TileNodes in queue & searching for path...
        while (frontier.Count > 0 && isFindingPath == true)
        {
            //Take out the next TileNode in queue
            currentSearchNode = frontier.Dequeue();

            //Set the node to explored
            currentSearchNode.isExplored = true;

            //Explore the current node's neighbors (This will connect neighboring nodes to the current node)
            ExploreNeighbors(currentSearchNode);

            //If the current search node is the destination coordinate, no longer finding the path
            if (currentSearchNode.coordinates == endCoordinates)
            {
                isFindingPath = false; //found the path
            }
        }
    }
    void ExploreNeighbors(TileNode parentNode)
    {
        //Get the neighbors of the tile node
        List<TileNode> neighbors = new List<TileNode>();

        //For each direction, if the coordinate exists in the grid, store the neighboring TileNode in the list of neighbors
        foreach (Vector2Int direction in directions) //(1,0), (-1,0), (0,1), (0,-1)
        {
            Vector2Int neighborCoordinates = parentNode.coordinates + direction; //right, left, up, down

            //If node exists in the grid, add it to the list of neighboring tile nodes. (otherwise you'll be adding a null object)
            if (currentGrid.ContainsKey(neighborCoordinates))
            {
                neighbors.Add(currentGrid[neighborCoordinates]);
            }
        }
        
        foreach (TileNode neighbor in neighbors)
        {
            //switch logic based on pathfinding method
            switch (pathfinding)
            {
                case PathfindingMethod.FollowTrail:
                    //Add each tile node to the reached dictionary and to the queue if the node hasn't yet been reached and it is part of the trail
                    if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isTrail)
                    {
                        neighbor.parentNode = parentNode;
                        reached.Add(neighbor.coordinates, neighbor);
                        //Add to queue to search through its neighboring tile nodes (if it has any)
                        frontier.Enqueue(neighbor);
                    }
                    break;

                case PathfindingMethod.FindOwnPath:
                    //Add each tile node to the reached dictionary and to the queue if the node hasn't yet been reached and it can be walked on
                    if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
                    {
                        neighbor.parentNode = parentNode;
                        reached.Add(neighbor.coordinates, neighbor);
                        //Add to queue to search through its neighboring tile nodes (if it has any)
                        frontier.Enqueue(neighbor);
                    }
                    break;

                default:
                    Debug.Log("Pathfinding method does not need to explore neighbors to pathfind");
                    break;
            }
        }
    }

    List<TileNode> BuildPath()
    {
        //Generate a new list for the path
        List<TileNode> path = new List<TileNode>();

        TileNode currentNode = endNode; //start from end, since nodes are connected from end to start

        //Add the current node to the path
        path.Add(currentNode);

        //The current node is part of the path
        currentNode.isPath = true;

        //While the current node is connected to a TileNode
        while (currentNode.parentNode != null)
        {
            //Current node is the one its connected to
            currentNode = currentNode.parentNode;

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
