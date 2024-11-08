using System.Collections.Generic;
using UnityEngine;

/*
 * This script will be used in the pathfinder class.
 * 
 * this script will be responsible for finding the path of least resistance from the start and end
 * tile nodes provided. This script will need access to the game manager to search for neighboring
 * tile nodes.
 */

public class PathOfLeastResistance
{
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    //The starting Tile Node (Where the enemies spawn from)
    //The ending Tile Node (Where the enemies want to go)
    //The grid of TileNodes to look through
    TileNode startNode;
    TileNode endNode;
    Dictionary<Vector2Int, TileNode> currentGrid;

    //Keep track of the branches that have been reached
    Dictionary<Vector2Int, TileNode> reached;

    //Need to keep track of branches' ending nodes for later logic
    Dictionary<Vector2Int, TileNode> branchEndNodes;

    //A dictionary to keep track of the branches in the trail. key is the branch's starting tile node
    Dictionary<TileNode, Branch> branches;

    /*
     * A queue of branches to create starting from the TileNode in queue.
     * The queue will be populated each time there is a split in path of the branch
     */
    Queue<Branch> nextBranch;

    public PathOfLeastResistance(TileNode startNode, TileNode endNode, Dictionary<Vector2Int, TileNode> currentGrid)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.currentGrid = currentGrid;

        //Initialize data structures
        branchEndNodes = new Dictionary<Vector2Int, TileNode>();
        reached = new Dictionary<Vector2Int, TileNode>();
        nextBranch = new Queue<Branch>();
        branches = new Dictionary<TileNode, Branch>();

        //When this script is made, initialize all the branches and everything
        InitializeTrail();
    }

    public List<TileNode> GetPathOfLeastResistance()
    {
        //Create the path to return
        List<TileNode> path = new List<TileNode>();

        //Return the path
        return path;
    }

    void InitializeTrail()
    {

    }
}
