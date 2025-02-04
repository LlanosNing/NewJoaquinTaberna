using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    //la rotacion de la camara en el eje X (parriba pabajo)
    public float camXRot = 0f;
    public float jumpForce = 700f;
    public bool isGrounded = true;
    //el pivote de la camara que tiene que rotar en el eje X
    public Transform cameraPivot;

    [Header("GROUND CHECKER")]
    public Transform groundCheckCenter;
    public Vector3 groundCheckSize = Vector3.one; //el .one es lo mismo que poner new Vector3 (1, 1, 1)
    //para guardar los colliders que detecta el ground checkers
    public Collider[] detectedColliders;
    //asi no detectara al player ni a otros objetos que estorban
    public LayerMask groundLayer;

    //para guardar el input en update y usarlo en fixedupdate
    private Vector3 input;
    public Rigidbody rb;
    private Camera cam;

    //cosas pa la animacion
    public Animator miAnim;

    void Start()
    {
        //buscar y asignar el rigidbody automaticamente
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");

        //cosas para la animacion xd
        float HVMagnitud = new Vector2(_horizontal, _vertical).magnitude;
        miAnim.SetFloat("HV_Magnitud", HVMagnitud);

        //guardamos el input para usarlo en fixedupdate
        input = new Vector3(_horizontal, 0f, _vertical);
        //para que se mueva en la direccion correcta respecto hacia donde mira,
        //hay que transformar el input para que sea en espacio local y no en espacio global
        input = transform.TransformDirection(input);

        float _rotMouseX = Input.GetAxisRaw("Mouse X"); //Es el X y no el Y porque se cuenta el eje donde esta el mouse en fisico (y se mueve en plano)
        //Aqui el _rotMouseX va en la Y porque este va referido al juego, y el eje de rotacion entonces es la Y
        transform.Rotate(0, _rotMouseX * rotationSpeed * Time.deltaTime, 0);
        //el deltaTime es para que no vaya pasudpoasyjdasm

        //hay que ir acumulando el valor de la rotacion en X de la camara
        //para que aumente o disminuya conforme movemos el raton arriba y abajo
        camXRot -= Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;
        //mathf clamp acota 2 valores para que no baje ni suba de esos valores
        camXRot = Mathf.Clamp(camXRot, -60, 60); //el camXRot se pone porque mathf.clamp devuelve un valor, asi que para usar ese valor hay que poner que ese valor sea la rotacion 
        //asignamos la rotacion en X a los angulos del pivote de la camara. localEulerAngles es para el eje loca, el global es el eulerAngles
        cameraPivot.localEulerAngles = new Vector3(camXRot, 0, 0);


        //SALTO
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

        GroundCheck();
    }


    private void FixedUpdate() //el fixedupdate para fisicas que se ejecutan muchas veces
    {   //.normalized es para que no se mueva mas rapido en diagonal
        Vector3 _velocity = input.normalized * moveSpeed;

        //no podemos modificar la velocity en el eje y del rigidbody o caera muy despacio
        _velocity.y = rb.velocity.y;
        rb.velocity = _velocity;
    }

    void GroundCheck()
    {
        //guardamos en la variable todos los colliders que encuentre el overlap
        //añadimos como ultimo parametro la groundlayer para que solo detecte los objetos que estan en esa capa
        detectedColliders = Physics.OverlapBox(groundCheckCenter.position, groundCheckSize * 0.5f, Quaternion.Euler(0, 0, 0), groundLayer);

        //cuando el checker detecte al menos un objeto suelo, podemos saltar
        if (detectedColliders.Length > 0)
        {
            isGrounded = true;
        }
        else //cuando no haya ningun objeto detectado no podemos saltar
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheckCenter.position, groundCheckSize); //el .position es porque es un transform
    }
}
