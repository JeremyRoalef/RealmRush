using TMPro;
using UnityEngine;

/*
 * This script is attached to the Tile prefab under the Text (TMP) child.
 * This script will be responsible for displaying the tile coordinates in the game. The game will show the various colors of text based on the pathfinding
 * of the enemies in the game. However, due to issues with Unity's input system, toggling the coordinates off and on in the editor version will not be
 * possible. For debugging purposes, the game version will be able to toggle.
 */

//Use "ExecuteAlways" to make this script execute while in the Unity editor and while in the game
[ExecuteAlways]

//Make the object this script is attached to have teh TextMeshPro component. This will prevent potential null reference exception errors.
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    //Gather serialized fields for colors to show state of tile coordinates
    [SerializeField][Tooltip("The default color of the text")] Color defaultColor = Color.white;
    [SerializeField][Tooltip("The blocked color of the text")] Color blockedColor = Color.gray;
    [SerializeField][Tooltip("The explored color of the text")] Color exploredColor = Color.yellow;
    [SerializeField][Tooltip("The color of the path text")] Color pathColor = new Color(1f,.5f,0f); //orange

    //Cashe references
    TextMeshPro textCoordinate;
    GridManager gridManager;

    //Attributes
    Vector2Int position = new Vector2Int();

    //Event Systems
    void Awake()
    {
        //Place the tile in the correct empty game object 

        //Find object in scene that has the currentGrid manager.
        gridManager = FindObjectOfType<GridManager>();

        //get TMPro component
        textCoordinate = GetComponent<TextMeshPro>();


        DisplayCurrentCoordinates();
        UpdateGameObjectName();

        //Default text coordinates to false so players don't see it in the game
        textCoordinate.enabled = false;
    }

    void Update()
    {
        /*
         * If the game is not playing (in Unity inspector), display the coordinates, update the object's name based on the coordinates,
         * toggle the coordinates on, & set the text colors.
         * Otherwise, toggle the labels on/off & set the text colors
         */

        if (!Application.isPlaying)
        {
            DisplayCurrentCoordinates();
            UpdateGameObjectName();
            textCoordinate.enabled = true;
            SetTextColor();
        }
        else
        {
            ToggleLabels();
            SetTextColor();
        }
    }

    //Public Methods

    //Private Methods
    void DisplayCurrentCoordinates()
    {
        //DO NOT RUN IF THERE IS NO GRID MANAGER!!!
        if (!CheckForGridManager()) { return; }

        //Get the x,y coordinate pair of the parent game object.
        position.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize); //position is multiplicative of 10, but can use the currentGrid snapp setting
        position.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize); //the 2d layout in the game uses x,z coordinates, so the z coordinate will be stored in the second vector

        //Set UI text
        textCoordinate.text = $"{position.x},{position.y}";
    }

    void UpdateGameObjectName()
    {
        //Set parent gameObject's name equal to the coordinate of the parent object.
        transform.parent.name = $"Tile {position.ToString()}";
    }

    void SetTextColor()
    {
        //DO NOT RUN IF THERE IS NO GRID MANAGER!!!
        if (!CheckForGridManager() ) { return; }

        //temp tile node
        TileNode tileNode = gridManager.getTileNode(position);

        //DO NOT RUN IF THERE IS NO TILE NODE!!!
        if (tileNode == null) { return; }

        //Set coordinate color
        if (!tileNode.isWalkable)
        {
            textCoordinate.color = blockedColor;
        }
        else if (tileNode.isPath)
        {
            textCoordinate.color = pathColor;
        }
        else if (tileNode.isExplored)
        {
            textCoordinate.color = exploredColor;
        }
        else
        {
            textCoordinate.color = defaultColor;
        }
    }

    void ToggleLabels()
    {
        //Toggle coordinates on/off based on previous state of the coordinates
        if (Input.GetKeyDown(KeyCode.C))
        {
            textCoordinate.enabled = !textCoordinate.enabled;
        }
    }

    bool CheckForGridManager()
    {
        if (gridManager == null)
        {
            //do a double check in case the grid manager was added, but code did not update
            gridManager = FindObjectOfType<GridManager>();

            //If it is still null, return false
            if (gridManager == null)
            {
                Debug.Log("Grid Manager does not eixt. Please create one.");
                //Grid manager not fount
                return false;
            }
            else
            {
                //Grid manager found
                return true;
            }
        }
        //Grid manager found
        return true;
    }
}
