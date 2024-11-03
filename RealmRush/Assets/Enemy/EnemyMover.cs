using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will be attached to all enemies
 * 
 * This script will be responsible for moving enemies along their designated path
 */

//This script requires the enemy script
[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    //TODO: enemies will inherit different pathfinding behaviors based on what I say they should do. This means this script will need to account for many types of pathfinding.

    //Serialized fields
    [SerializeField][Range(0.1f, 10f)] float fltMoveSpeed = 1f;

    //Cashe references
    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;
    List<TileNode> path = new List<TileNode>();

    //Event Systems
    private void Awake()
    {
        //get components
        enemy = GetComponent<Enemy>();

        //get objects in scene
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }
    void OnEnable()
    {
        ReturnToStart();

        //Stop any coroutine
        StopAllCoroutines();

        //clear whatever may be in the path before generating new path
        path.Clear();

        switch (pathfinder.Pathfinding)
        {
            case Pathfinder.PathfindingMethod.FollowTrail:
                FollowTrail();
                break;
            case Pathfinder.PathfindingMethod.FindOwnPath:
                RecalculatePath(true);
                break;
            case Pathfinder.PathfindingMethod.DirectPathing:
                PathDirectly();
                break;
            default:
                Debug.Log("No pathfinding method determined. Please select a pathfind method");
                break;
        }
    }

    //Public Methods

    //Private Methods
    void ReturnToStart()
    {
        //set position to the start coordinates in the currentGrid manager
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        //If the path is being reset to the beginning, set the coordinates to the start.
        //otherwise, set the coordinates to the current object's position.
        if (resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        //determine new path based on current coordinates
        path = pathfinder.FindOwnPath(coordinates);

        StartCoroutine(FollowPath());
    }

    void FollowTrail()
    {
        path = pathfinder.FollowTrail();
        StartCoroutine(FollowPath());
    }

    void PathDirectly()
    {
        path = pathfinder.PathDirectly();
        StartCoroutine(FollowPath());
    }
    void FinishPath()
    {
        //Enemy reached the castle
        gameObject.SetActive(false);
        enemy.RemoveGold();
    }

    IEnumerator FollowPath()
    {
        //for each waypoint in the path, Lerp between the current position and the end position while the percentage of travel is less than 1
        for(int i = 1; i < path.Count; i++)
        {
            //store current and end position
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            //percentage of the way to end position
            float fltTravelPercent = 0f;

            //Look at where youre going
            transform.LookAt(endPos);

            //While not at the end position, move to end position
            while (fltTravelPercent < 1f)
            {
                fltTravelPercent += Time.deltaTime * fltMoveSpeed;

                //Use LERP (Linear Interpolation) to move object smoothly between two positions
                transform.position = Vector3.Lerp(startPos, endPos, fltTravelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        //The path has been reached
        FinishPath();
    }
}
