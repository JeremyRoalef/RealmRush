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

    [SerializeField] Tower tower;



    private void OnMouseDown()
    {
        if (isPlaceable)
        {
            bool isPlaced = tower.CreateTower(tower, transform.position);
            IsPlaceable = !isPlaced;
        }
    }
}
