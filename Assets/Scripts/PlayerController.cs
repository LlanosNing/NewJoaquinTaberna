using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables públicas
    public float walkSpeed = 2.0f; // Velocidad al caminar
    public float runSpeed = 5.0f;  // Velocidad al correr
    public float rotationSpeed = 10f; // Velocidad de rotación
    public float jumpForce = 8f; // Fuerza del salto
    public float gravity = -9.81f; // Fuerza de la gravedad

    // Variables privadas
    private float currentSpeed; // Velocidad actual (caminar o correr)
    private Vector3 velocity; // Velocidad en el espacio 3D
    private bool isGrounded; // Verifica si está en el suelo
    private CharacterController characterController; // Referencia al CharacterController

    // Animación (opcional, si usas Animator)
    private Animator animator; // Referencia al Animator

    // Variables para la cámara
    public Transform cameraTransform; // Transform de la cámara
    public float cameraDistance = 5.0f; // Distancia de la cámara desde el personaje
    public float cameraHeight = 2.0f; // Altura de la cámara respecto al personaje
    public float cameraRotationSpeed = 5.0f; // Velocidad de rotación de la cámara
    public float cameraCollisionOffset = 0.3f; // Para evitar que la cámara pase a través de objetos

    // Start se llama antes del primer frame
    void Start()
    {
        // Obtener el CharacterController y Animator
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Asegúrate de que tu personaje tiene un Animator
    }

    // Update se llama una vez por frame
    void Update()
    {
        // Movimiento y gravedad
        isGrounded = characterController.isGrounded; // Verifica si estamos tocando el suelo

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Hace que el personaje se quede pegado al suelo
        }

        // Mover al jugador
        MovePlayer();

        // Aplicar la gravedad
        velocity.y += gravity * Time.deltaTime;

        // Mover al personaje en el espacio 3D
        characterController.Move(velocity * Time.deltaTime);

        // Controlar la cámara
        HandleCamera();
    }

    // Función para mover al jugador
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f) // Si hay movimiento
        {
            // Determinar la velocidad actual según si el jugador corre o camina
            if (Input.GetKey(KeyCode.LeftShift)) // Si mantiene Shift, corre
                currentSpeed = runSpeed;
            else
                currentSpeed = walkSpeed;

            // Calcular la dirección de movimiento (como en tercera persona)
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref velocity.y, rotationSpeed);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Mover el personaje en la dirección de la cámara
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            velocity = moveDirection * currentSpeed;

            // Animaciones (si estás usando un Animator)
            if (animator != null)
            {
                animator.SetFloat("Speed", direction.magnitude); // Cambiar la velocidad de animación
                animator.SetBool("IsRunning", Input.GetKey(KeyCode.LeftShift)); // Animación de correr
            }
        }
        else
        {
            // Si no hay movimiento, rotar hacia la cámara
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            directionToCamera.y = 0; // Mantener la dirección en el plano horizontal
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToCamera), Time.deltaTime * rotationSpeed);

            // Detener las animaciones si no hay movimiento
            if (animator != null)
            {
                animator.SetFloat("Speed", 0);
            }
        }

        // Salto (si el personaje está en el suelo y presiona la tecla de salto)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce; // Aplicar fuerza de salto
        }
    }

    // Función para manejar la cámara en tercera persona
    private void HandleCamera()
    {
        // Obtener los movimientos del mouse o del joystick (para rotar la cámara)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotar la cámara alrededor del personaje (en el eje Y)
        cameraTransform.Rotate(0, mouseX * cameraRotationSpeed, 0);

        // Limitar la inclinación vertical de la cámara (para evitar que mire demasiado arriba o abajo)
        Vector3 currentRotation = cameraTransform.rotation.eulerAngles;
        currentRotation.x -= mouseY * cameraRotationSpeed;
        currentRotation.x = Mathf.Clamp(currentRotation.x, 10f, 80f); // Limitar la inclinación
        cameraTransform.rotation = Quaternion.Euler(currentRotation);

        // Colocar la cámara detrás del personaje
        Vector3 desiredPosition = transform.position - cameraTransform.forward * cameraDistance + Vector3.up * cameraHeight;

        // Verificar colisiones entre la cámara y el entorno
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -cameraTransform.forward, out hit, cameraDistance))
        {
            desiredPosition = hit.point + hit.normal * cameraCollisionOffset; // Desplazar la cámara para evitar colisiones
        }

        cameraTransform.position = desiredPosition;
    }
}
