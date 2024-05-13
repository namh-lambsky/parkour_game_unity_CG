using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    Camera cam;

    float mouseX;
    float mouseY;
    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        MyInput();
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation  , 0);

    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
