using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLOUController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1f;
    public float jumpForce = 5f;
    public float rotationSpeed = 10f;

    private float currentSpeed;
    private bool isCrouching = false;
    private bool isGrounded;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Animator animator;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveVelocity = moveDirection * currentSpeed;
            rb.MovePosition(transform.position + moveVelocity * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                capsuleCollider.height = 0.9f; // Reducir altura para agacharse
                currentSpeed = crouchSpeed;
            }
            else
            {
                capsuleCollider.height = 2f; // Restaurar altura
                currentSpeed = walkSpeed;
            }
        }
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsGrounded", isGrounded);
    }
}

