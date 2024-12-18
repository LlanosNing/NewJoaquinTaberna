using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLOUCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float rotationSpeed = 5f;

    private void LateUpdate()
    {
        RotateCamera();
        FollowPlayer();
    }

    private void RotateCamera()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        transform.RotateAround(player.position, Vector3.up, horizontalInput * rotationSpeed);
        transform.RotateAround(player.position, transform.right, -verticalInput * rotationSpeed);
    }

    private void FollowPlayer()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);
    }
}

