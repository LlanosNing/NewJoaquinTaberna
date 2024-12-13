using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    //el objeto que tengamos en rango para poder recoger
    public GameObject tempItem;
    //el objeto que hayamos recogido
    public ColorItem currentItem;
    public bool hasItem;
    public float throwForce = 15;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //solo deja recoger el objeto si esta asignado a la variable tempItem
        //cuando tempItem es null, no hay ningun objeto cerca
        if (Input.GetKeyDown(KeyCode.E) && tempItem != null)
        {
            //asignamos al objeto recogido el script ColorItem que lleve el objeto
            currentItem = tempItem.GetComponent<ColorItem>();
            //ejecutar la funcion pick del propio objeto
            currentItem.Pick(this.transform);
            //desaignar el objeto temporal una vez ya lo ha recogido
            tempItem = null;
            //en cuanto encuentra la palabra return, la funcion no sigue ejecutandose
            //Y asi empieza de nuevo
            //asi evitamos que recoja y cuelte el objeto a la vez
            return;
        }

        //comprueba si hay algun valor asignado a la variable currentItem
        //para saber si ha recogido algun objeto
        if (Input.GetKeyDown(KeyCode.E) && currentItem != null)
        {
            currentItem.Drop();
            //desasignar el objeto que lleva recogido
            currentItem = null;
        }

        //lanzar el objeto que lleva recogido
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //calculamos un rayo que va hacia el centro de la pantalla (mirilla)
            Ray _ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
            if (Physics.Raycast(_ray, out RaycastHit _hit, 1000f))
            {
                //calculamos la direccion en la que hay que lanzar el objeto para que vaya
                //hacia lo que haya en el centro de la pantalla
                Vector3 _dir = _hit.point - currentItem.transform.position;
                //lanzamos el objeto en esa direccion
                currentItem.Throw(_dir.normalized * throwForce);
            }
            else
            {
                //lanza el objeto hacia la direccoin forwars (adelante) del jugador
                //si no le metes la parafernalia del parentesis del throw se queja, porque la funcion pide un Vector3
                //el camera main es para tirar la pelota en la direccion de la camara (usa el tag MainCamera)
                //si hay mas de una camara asegurarse que la del jugador sea la unica con el tag MainCamera
                currentItem.Throw(Camera.main.transform.forward * throwForce);

            }
            currentItem = null;
            Debug.DrawRay(_ray.origin, _ray.direction, Color.red, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item") == true)
        {
            //el truco del dinero es como poner el + (other.gameObject) en el string
            //Debug.Log($"Me he encontrado el objeto {other.gameObject}");
            //asignamos el objeto temporal a recoger
            tempItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item") == true)
        {
            //Debug.Log($"Me he alejado el objeto {other.gameObject}");
            //desasignar el objeto temporal
            tempItem = null;
        }
    }
}
