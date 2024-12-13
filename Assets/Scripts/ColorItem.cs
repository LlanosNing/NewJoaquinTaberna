using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorItem : MonoBehaviour
{
    public ColorType colorType;
    private Rigidbody rb;
    private Collider col;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Pick(Transform _player)
    {
        rb.isKinematic = true;
        transform.SetParent(_player);
        //modificamos la posicion local del objeto (relatica al objeto padre)
        //para que se situe justo delante del jugador
        transform.localPosition = Vector3.forward;
        //desactivar el collider para que no choque con el jugador
        //(cuando choca es cuando se va volando)
        col.enabled = false;
    }

    public void Drop()
    {
        rb.isKinematic = false;
        //desemparentar del jugador
        transform.SetParent(null);
        col.enabled = true;
    }

    //la fuerza y direccion de lanzamiento la pasa el script de PickItem
    //la fuerza no es float, son vectores
    public void Throw(Vector3 _force)
    {
        //primero se suelta para poder aplicarle fisicas
        Drop();
        rb.AddForce(_force, ForceMode.VelocityChange);
    }
}
