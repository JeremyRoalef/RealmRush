using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    //pass in a bunch of waypoints for the enemy to move to and then loop through those waypoints to move the enemy to the location
    List<TileNode> path = new List<TileNode>(); //Use a list b/c we will mess with the size of the elements
    [SerializeField] [Range(0.1f,10f)] float fltMoveSpeed = 1f;

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;


    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }
    void OnEnable()
    {
        RecalculatePath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void RecalculatePath()
    {
        path.Clear(); //clear whatever may be in the path before generating new path
        path = pathfinder.GetNewPath();
    }

    void FinishPath()
    {
        gameObject.SetActive(false);
        enemy.RemoveGold();
    }

    IEnumerator FollowPath()
    {
        //for each waypoint in the path, Lerp between the current position and the end position while the percentage of travel is less than 1
        for(int i = 0; i < path.Count; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float fltTravelPercent = 0f;
            //Use LERP (Linear Interpolation) to move object smoothly between two positions

            transform.LookAt(endPos);

            while (fltTravelPercent < 1f)
            {
                fltTravelPercent += Time.deltaTime * fltMoveSpeed; //travel percentage will increase based on elapsed time
                transform.position = Vector3.Lerp(startPos, endPos, fltTravelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }
}
