using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    // La rotación de la cámara en el eje X (arriba/abajo)
    public float camXRot = 0f;
    public float jumpForce = 700f;
    public bool isGrounded = true;
    // El pivote de la cámara, pero ahora es solo para rotación vertical de la cámara
    public Transform cameraPivot;

    [Header("GROUND CHECKER")]
    public Transform groundCheckCenter;
    public Vector3 groundCheckSize = Vector3.one;
    public Collider[] detectedColliders;
    public LayerMask groundLayer;

    private Vector3 input;
    public Rigidbody rb;
    private Camera cam;

    // Variables para la posición de la cámara
    public Vector3 cameraOffset = new Vector3(0, 5, -10);  // Posición en relación al jugador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main; // Usamos la cámara principal de la escena
    }

    void Update()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleJump();
        GroundCheck();
    }

    private void HandleMovementInput()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");
        input = new Vector3(_horizontal, 0f, _vertical);
        input = transform.TransformDirection(input);
    }

    private void HandleCameraInput()
    {
        // Rotación horizontal del jugador
        float _rotMouseX = Input.GetAxisRaw("Mouse X");
        transform.Rotate(0, _rotMouseX * rotationSpeed * Time.deltaTime, 0);

        // Rotación vertical de la cámara
        camXRot -= Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;
        camXRot = Mathf.Clamp(camXRot, -60, 60);
        cameraPivot.localEulerAngles = new Vector3(camXRot, 0, 0);

        // Ajuste de la cámara en el espacio 3D
        Vector3 desiredCameraPos = transform.position + cameraOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, desiredCameraPos, Time.deltaTime * 5f);
        cam.transform.LookAt(transform.position + Vector3.up); // Mantén la cámara mirando hacia el jugador
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    private void FixedUpdate()
    {
        Vector3 _velocity = input.normalized * moveSpeed;
        _velocity.y = rb.velocity.y;
        rb.velocity = _velocity;
    }

    void GroundCheck()
    {
        detectedColliders = Physics.OverlapBox(groundCheckCenter.position, groundCheckSize * 0.5f, Quaternion.Euler(0, 0, 0), groundLayer);
        isGrounded = detectedColliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheckCenter.position, groundCheckSize);
    }
}



