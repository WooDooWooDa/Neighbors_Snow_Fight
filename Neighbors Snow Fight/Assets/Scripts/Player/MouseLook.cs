using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensititvity = 100f;
    [SerializeField] private float originY = 90f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform headParent;
    [SerializeField] private Transform head;

    private float xRotation;

    public Quaternion GetDirection()
    {
        return transform.rotation.normalized;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        headParent.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensititvity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensititvity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, originY, 0f);
        head.localRotation = Quaternion.Euler(-xRotation, -originY, 0f);

        headParent.Rotate(Vector3.up * mouseX);
        if (headParent.transform.localRotation.y > 0.3 || headParent.transform.localRotation.y < -0.3) {
            playerBody.Rotate(Vector3.up * mouseX);
            headParent.Rotate(Vector3.up * -mouseX);
        } else {
            headParent.Rotate(Vector3.up * mouseX);
        }
    }
}
