using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    //pass in a bunch of waypoints for the enemy to move to and then loop through those waypoints to move the enemy to the location
    [SerializeField] List<Waypoint> path = new List<Waypoint>(); //Use a list b/c we will mess with the size of the elements
    [SerializeField] float fltMoveWaitTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("start");
        StartCoroutine(PrintWaypointName());
        //Debug.Log("finish");
    }

    IEnumerator PrintWaypointName()
    {
        //Delay each waypoint by 1 second (or variable time)
        foreach (Waypoint waypoint in path)
        {
            transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(fltMoveWaitTime);
            //Debug.Log(waypoint);
        }
    }
}
