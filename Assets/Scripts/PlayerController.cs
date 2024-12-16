using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables de movimiento
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2f;
    public float rotationSpeed = 700f;
    private float currentSpeed;
    private float gravity = -9.81f;
    private Vector3 velocity;

    // Variables de salto
    public float jumpHeight = 2f;
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    // Componentes
    private CharacterController characterController;
   // private Animator animator;

    // Movimiento de la cámara
    public Transform cameraTransform;

    private bool isCrouching = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundMask);

        // Movimiento horizontal y vertical
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotación hacia la dirección de movimiento
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref velocity.y, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Movimiento
            Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(move * currentSpeed * Time.deltaTime);

            //// Animación de caminar/correr
            //if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
            //{
            //    currentSpeed = sprintSpeed;
            //    animator.SetFloat("Speed", 1.5f); // Ajusta según tus animaciones
            //}
            //else if (isCrouching)
            //{
            //    currentSpeed = crouchSpeed;
            //    animator.SetFloat("Speed", 0.5f); // Ajusta según tus animaciones
            //}
            //else
            //{
            //    currentSpeed = moveSpeed;
            //    animator.SetFloat("Speed", 1f); // Ajusta según tus animaciones
            //}
        }
        else
        {
           // animator.SetFloat("Speed", 0f); // Detiene la animación si no hay movimiento
        }

        // Salto
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //animator.SetTrigger("Jump"); // Llama la animación de salto
        }

        // Aplicar gravedad
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            if (velocity.y < 0)
                velocity.y = -2f; // Evita que el personaje siga cayendo al estar en el suelo
        }

        characterController.Move(velocity * Time.deltaTime);

        // Cambio de postura (agacharse)
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
           // animator.SetBool("IsCrouching", isCrouching);
            characterController.height = isCrouching ? 0.5f : 2f;
            characterController.center = isCrouching ? new Vector3(0, 0.25f, 0) : new Vector3(0, 1, 0);
        }
    }
}
