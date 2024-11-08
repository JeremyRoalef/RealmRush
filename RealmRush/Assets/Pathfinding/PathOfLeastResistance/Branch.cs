using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch
{
    TileNode startNode;
    List<TileNode> path;
    List<Branch> childBranches;
    int resistance;

    //Constructor
    public Branch(TileNode startNode)
    {
        path = new List<TileNode>();
        childBranches = new List<Branch>();
        resistance = 0;
        this.startNode = startNode;
    }

    public void Add(TileNode node)
    {
        path.Add(node);
    }

    //Use to calculate the resistance of the trail.
    public int GetResistance()
    {
        //Reset the value of the resistance
        resistance = 0;

        //Calculate the new value of the resistance
        foreach (TileNode node in path)
        {
            resistance += node.resistance;
        }

        return resistance;
    }
    public void CreateChildBranch(Branch childBranch)
    {
        this.childBranches.Add(childBranch);
    }
    public List<TileNode> GetPath()
    {
        return path;
    }

    public List<Branch> GetChildBranches()
    {
        return childBranches;
    }
    public Branch GetLeastResistantBranch()
    {
        if (childBranches.Count == 0)
        {
            return null; //No branches to get
        }
        //Start from index 0
        Branch leastResistantBranch = childBranches[0];

        //Determine the branch of least resistance
        foreach (Branch branch in childBranches)
        {
            if (branch.GetResistance() < leastResistantBranch.GetResistance())
            {
                leastResistantBranch = branch;
            }
        }

        return leastResistantBranch;
    }

    public List<TileNode> GetTrail()
    {
        //Get the trail from this branch
        return GetTrail(this);
    }

    public List<TileNode> GetTrail(Branch currentBranch)
    {
        //The path to return
        List<TileNode> path = new List<TileNode>();

        //build the path from the current branch
        path = currentBranch.GetPath();

        //If there are child branches, get the least resistant branch and get its trail
        if ((currentBranch.GetChildBranches().Count > 0))
        {
            //Get least resistant branch
            Branch leastResistantBranch;

            //Set the least resistant branch to the least resistant branch (amazing commentary!)
            leastResistantBranch = currentBranch.GetLeastResistantBranch();

            //Get the trail (recursion) of the least resistant branch
            List<TileNode> childBranchPath = GetTrail(leastResistantBranch);

            //add the child branch path to the current branch path
            foreach (TileNode tileNode in childBranchPath)
            {
                path.Add(tileNode);
            }
        }

        //Return the path
        return path;
    }

    public TileNode GetStartNode() { return startNode; }

    public TileNode GetEndOfCurrentPath()
    {
        if (path.Count == 0)
        {
            Debug.Log("path does not have anything");
            return null;
        }

        return path[path.Count - 1];
    }
}
