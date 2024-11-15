using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * This script will be attached to the main camera.
 * 
 * This script will be responsible for the movement of the camera
 */

public class CameraMover : MonoBehaviour
{
    [SerializeField] InputAction moveCamera;
    [SerializeField][Range(0,1)] float cameraMoveSpeed = 0.5f; //The lazy way of solving my problems :)

    Vector3 oldMousePosition;
    Vector3 newMousePosition;
    Vector3 oldCameraPosition;
    bool cameraIsMoving = false;


    private void OnEnable()
    {
        moveCamera.Enable();
    }

    private void OnDisable()
    {
        moveCamera.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCamera.ReadValue<float>() > 0)
        {
            //Set old mouse position if camera is not moving
            if (!cameraIsMoving)
            {
                SetOldMousePosition();
                //Store the camera position
                oldCameraPosition = transform.position;
            }

            //Camera is moving
            cameraIsMoving = true;

            SetNewMousePosition();

            //Get difference between the old & new mouse vectors
            Vector3 deltaPos = newMousePosition - oldMousePosition;

            //Using only x and y values, & multiply by the camera move speed
            deltaPos = new Vector3 (deltaPos.x, 0, deltaPos.y) * cameraMoveSpeed;

            //New camera's position is equal to original position + inverted change in mouse position.
            Vector3 newCameraPosition = oldCameraPosition - deltaPos;

            //Set camera pos to new camera pos
            transform.position = newCameraPosition;
        }
        else
        {
            //Camera is not moving
            cameraIsMoving = false;
        }
    }

    void SetOldMousePosition()
    {
        oldMousePosition = GetMousePosition();
    }

    Vector3 GetMousePosition()
    {
        // Get the mouse's position on the screen
        Vector3 mouseScreenPosition = Input.mousePosition;
        //Debug.Log(mouseScreenPosition);
        return mouseScreenPosition;
    }

    void SetNewMousePosition()
    {
        newMousePosition = GetMousePosition();
    }
}
