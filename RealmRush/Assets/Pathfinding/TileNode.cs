using System;
using UnityEngine;
/*
 * This script is used in the GridManager and PathFinder scripts
 * This Script is serializeable, meaning it can be used in MonoBehavior scripts
 * 
 * This script will store the coordinate reporesentation of tiles in the game.
 */

[Serializable]
public class TileNode
{
    //public attributes
    public Vector2Int coordinates;
    public bool isWalkable; //can enemies walk on this tile?
    public bool isExplored; //has this TileNode been eplored while searching for a path?
    public bool isPath; //Is this TileNode part of the path?

    //TileNodes attached to this TileNode
    public TileNode connectedTo;

    //TileNode constructor
    public TileNode(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
