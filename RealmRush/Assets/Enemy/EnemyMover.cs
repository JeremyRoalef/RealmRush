using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    //pass in a bunch of waypoints for the enemy to move to and then loop through those waypoints to move the enemy to the location
    [SerializeField] List<Waypoint> path = new List<Waypoint>(); //Use a list b/c we will mess with the size of the elements
    [SerializeField] [Range(0.1f,10f)] float fltMoveSpeed = 1f;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    void ReturnToStart()
    {
        transform.position = path[0].transform.position;
        //Remove the first object in the path
        path.Remove(path[0]);
    }

    void FindPath()
    {
        path.Clear(); //clear whatever may be in the path before generating new path

        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in parent.transform)
        {
            path.Add(child.GetComponent<Waypoint>());
        }
    }

    IEnumerator FollowPath()
    {
        //for each waypoint in the path, Lerp between the current position and the end position while the percentage of travel is less than 1
        foreach (Waypoint waypoint in path)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = waypoint.transform.position;
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

        gameObject.SetActive(false);
        enemy.RemoveGold();
    }
}
