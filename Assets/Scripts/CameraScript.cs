using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerBody;

    public float mouseSensitivity = 50f;

    /*Quaternion rotationMin = Quaternion.Euler(new Vector3(-80, 0f, 0f));
    Quaternion rotationMax = Quaternion.Euler(new Vector3(80, 0f, 0f));*/
    float xRotation = 0f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * inputX);

        /*Vector3 inputVectorX = new Vector3(0, inputX, 0);
        Vector3 inputVectorY = new Vector3(-inputY, 0, 0);

        Quaternion newRotationX = Quaternion.Euler(playerBody.eulerAngles += inputVectorX);
        Quaternion newRotationY = Quaternion.Euler(transform.eulerAngles += inputVectorY);
        
        if (newRotationY.x < rotationMax.x && newRotationY.x > rotationMin.x)
        {
            transform.rotation = newRotationY;
        }
        playerBody.rotation = newRotationX;*/
    }
}
