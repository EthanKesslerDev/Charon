using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sens = 50f;
    public Transform body;
    float xRot = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;


        xRot -= mY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * mX);
    }
}
