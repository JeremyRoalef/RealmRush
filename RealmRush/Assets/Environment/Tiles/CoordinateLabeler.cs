using TMPro;
using UnityEngine;

//THIS IS SO COOL

[ExecuteAlways] //execute this script in edit mode and in play mode
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f,.5f,0f);

    TextMeshPro textCoordinate;
    Vector2Int position = new Vector2Int();
    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        textCoordinate = GetComponent<TextMeshPro>();
        DisplayCurrentCoordinates();
        UpdateGameObjectName();
        textCoordinate.enabled = false;

    }

    void Update()
    {
        if (!Application.isPlaying) //ONLY RUN THIS IF THE APPLICATION IS NOT IN PLAY MODE
        {
            DisplayCurrentCoordinates();
            UpdateGameObjectName();
        }

        SetTextColor();
        ToggleLabels();
    }
    void DisplayCurrentCoordinates()
    {
        if (gridManager == null) { return; }
        position.x = Mathf.RoundToInt(transform.parent.position.x/ gridManager.UnityGridSize); //position is multiplicative of 10, but can use the grid snapp setting
        position.y = Mathf.RoundToInt(transform.parent.position.z/ gridManager.UnityGridSize); //the 2d layout in the game uses x,z coordinates, so the z coordinate will be stored in the second vector

        textCoordinate.text = $"{position.x},{position.y}";
    }

    void UpdateGameObjectName()
    {
        transform.parent.name = $"Tile {position.ToString()}";
    }

    void SetTextColor()
    {
        if (gridManager == null) { return; }

        TileNode tileNode = gridManager.getTileNode(position);

        if (tileNode == null) { return; }

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            textCoordinate.enabled = !textCoordinate.IsActive(); //when pressing the c button, turn on/off the text coordinates
        }
    }
}
