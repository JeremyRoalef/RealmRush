using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class TileNode
{
    public Vector2Int coordinates;

    public bool isWalkable;
    public bool isExplored;
    public bool isPath;

    public TileNode connectedTo;

    public TileNode(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
