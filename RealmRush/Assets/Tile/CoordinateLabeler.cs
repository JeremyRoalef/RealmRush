using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Reflection.Emit;

//THIS IS SO COOL

[ExecuteAlways] //execute this script in edit mode and in play mode
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;

    TextMeshPro textCoordinate;
    Vector2Int position = new Vector2Int();
    Waypoint waypoint;

    private void Awake()
    {
        waypoint = GetComponentInParent<Waypoint>(); //get the parent component of type waypoint
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

        ColorCoordinates();
        ToggleLabels();
    }
    void DisplayCurrentCoordinates()
    {
        position.x = Mathf.RoundToInt(transform.parent.position.x/ UnityEditor.EditorSnapSettings.move.x); //position is multiplicative of 10, but can use the grid snapp setting
        position.y = Mathf.RoundToInt(transform.parent.position.z/ UnityEditor.EditorSnapSettings.move.z); //the 2d layout in the game uses x,z coordinates, so the z coordinate will be stored in the second vector

        textCoordinate.text = $"{position.x},{position.y}";
    }

    void UpdateGameObjectName()
    {
        transform.parent.name = $"Tile {position.ToString()}";
    }

    void ColorCoordinates()
    {
        if (waypoint.IsPlaceable)
        {
            textCoordinate.color = defaultColor;
        }
        else
        {
            textCoordinate.color = blockedColor;
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
