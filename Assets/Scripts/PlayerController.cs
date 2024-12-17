using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f; // Ajustado para trabajar mejor con la gravedad.
    public Transform cameraPivot;
    public Transform groundCheckCenter;
    public Vector3 groundCheckSize = Vector3.one;
    public LayerMask groundLayer;

    private float camXRot = 0f;
    private Vector3 inputDirection;
    private Rigidbody rb;
    private Camera cam;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evitar que el Rigidbody rote por colisiones.
        cam = Camera.main;
    }

    void Update()
    {
        HandleMovementInput();
        HandleCameraRotation();
        GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        inputDirection = (forward * vertical + right * horizontal).normalized;
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);

        camXRot -= mouseY * rotationSpeed * Time.deltaTime;
        camXRot = Mathf.Clamp(camXRot, -45f, 45f);

        cameraPivot.localEulerAngles = new Vector3(camXRot, 0, 0);
    }

    void MoveCharacter()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        if (inputDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 velocity = new Vector3(inputDirection.x * currentSpeed, rb.velocity.y, inputDirection.z * currentSpeed);
        rb.velocity = velocity;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapBox(groundCheckCenter.position, groundCheckSize * 0.5f, Quaternion.identity, groundLayer);
        isGrounded = colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheckCenter.position, groundCheckSize);
    }
}

