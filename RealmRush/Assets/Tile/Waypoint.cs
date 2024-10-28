using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] bool isPlaceable = true;
    //Create a property to get and set the isPlaceable boolean. Same as getter & setter methods
    public bool IsPlaceable
    {
        get { return isPlaceable; }
        set { isPlaceable = value; }
    }

    [SerializeField] GameObject tower;



    private void OnMouseDown()
    {
        if (isPlaceable)
        {
            Instantiate(tower, transform.position, Quaternion.identity);
            IsPlaceable = false;
        }
    }
}
