using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour
{
    public enum RotationAxes { MouseX, MouseY }
    public RotationAxes axes = RotationAxes.MouseY;
 
    private float currentSensitivity_X = 1.5f;
    private float currentSensitivity_Y = 1.5f;

    private float sensitivity_X = 1.5f;
    private float sensitivity_Y = 1.5f;

    private float rotation_X, rotation_Y;

    private float minimum_X = -360f;
    private float maximum_X = 360f;

    private float minimum_Y = -360f;
    private float maximum_Y = 360f;

    private Quaternion originalRotation;
    private float mouseSensitivity = 1.7f;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        HandleRotation();
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    void HandleRotation()
    {
        if (currentSensitivity_X != mouseSensitivity || currentSensitivity_Y != mouseSensitivity)
        {
            currentSensitivity_X = currentSensitivity_Y = mouseSensitivity;
        }

        sensitivity_X = currentSensitivity_X;
        sensitivity_Y = currentSensitivity_Y;

        // Mouse X is the horizontal movement of the mouse
        if (axes == RotationAxes.MouseX)
        {
            rotation_X += Input.GetAxis("Mouse X") * sensitivity_X;
            rotation_X = ClampAngle(rotation_X, minimum_X, maximum_X); // degree rotation
            Quaternion xQuaternion = Quaternion.AngleAxis(rotation_X, Vector3.up); // move by X degree rotation around Y Axis

            transform.localRotation = originalRotation * xQuaternion;
        }

        // Mouse Y is the vertical movement of the mouse
        if (axes == RotationAxes.MouseY)
        {
            rotation_Y += Input.GetAxis("Mouse Y") * sensitivity_Y;
            rotation_Y = ClampAngle(rotation_Y, minimum_Y, maximum_Y); // degree rotation
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotation_Y, Vector3.right); // move by Y degree rotation around X Axis

            transform.localRotation = originalRotation * yQuaternion;
        }      
    }
}
