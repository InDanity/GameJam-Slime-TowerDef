using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    // Camera Sensitivity
    public float xSens;
    public float ySens;

    // Player Orientation
    public Transform orientation;

    // Camera Rotation
    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Locks the cursor in the middle of the screen, makes cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Getting mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and player orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
