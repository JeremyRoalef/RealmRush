using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    //pass in a bunch of waypoints for the enemy to move to and then loop through those waypoints to move the enemy to the location
    [SerializeField] List<Waypoint> path = new List<Waypoint>(); //Use a list b/c we will mess with the size of the elements


    // Start is called before the first frame update
    void Start()
    {
        PrintWaypointName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintWaypointName()
    {
        foreach (Waypoint waypoint in path)
        {
            Debug.Log(waypoint);
        }
    }
}
