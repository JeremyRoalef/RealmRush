using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] TileNode tileNode;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tileNode.coordinates);
        Debug.Log(tileNode.isWalkable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
