using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    public void DoFov(float fov)
    {
        // Assuming you have a reference to the camera component in your Movement class
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.fieldOfView = fov;
        }
        else
        {
            Debug.LogWarning("Main camera not found. Unable to adjust field of view.");
        }
    }

    // Update is called once per frame
   private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation 
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        
    }
}
