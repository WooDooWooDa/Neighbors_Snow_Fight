using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensititvity = 500f;
    [SerializeField] private float originY = 90f;
    [SerializeField] public Transform playerBody;
    [SerializeField] public Transform headParent;
    [SerializeField] public Transform headModel;

    private float xRotation;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetDirection()
    {
        return transform.rotation;
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
        headModel.localRotation = Quaternion.Euler(-xRotation, -originY, 0f);

        headParent.Rotate(Vector3.up * mouseX);
        if (headParent.transform.localRotation.y > 0.3 || headParent.transform.localRotation.y < -0.3) {
            playerBody.Rotate(Vector3.up * mouseX);
            headParent.Rotate(Vector3.up * -mouseX);
        } else {
            headParent.Rotate(Vector3.up * mouseX);
        }
    }
}
