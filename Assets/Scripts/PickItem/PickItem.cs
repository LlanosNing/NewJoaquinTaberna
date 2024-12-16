using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    public Transform holdPosition; // Posici�n donde se sostendr�n los objetos
    public float interactionRange = 5f; // Ampliamos el rango de interacci�n
    public LayerMask interactableLayer; // Capa para los objetos interactuables
    public LayerMask dropPointLayer; // Capa para los puntos de entrega
    public GameObject previewObjectPrefab; // Prefab de previsualizaci�n del objeto
    public Transform raycastOrigin; // Punto de origen del raycast (por ejemplo, la mano del personaje)

    private GameObject heldObject; // Objeto actualmente sostenido por el jugador
    private GameObject previewObject; // Instancia del objeto de previsualizaci�n
    private Transform currentDropPoint; // Punto de entrega m�s cercano
    private Rigidbody heldObjectRb; // Rigidbody del objeto sostenido

    void Update()
    {
        // Detectar interacci�n con objetos al presionar el bot�n
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                TryDropObject();
            }
        }

        // Actualizar la previsualizaci�n
        UpdatePreview();
    }

    void TryPickupObject()
    {
        if (raycastOrigin == null)
        {
            Debug.LogError("raycastOrigin no est� configurado.");
            return;
        }

        // Detectar objetos cercanos usando un rayo esf�rico
        Collider[] hitColliders = Physics.OverlapSphere(raycastOrigin.position, interactionRange, interactableLayer);
        if (hitColliders.Length > 0)
        {
            // Seleccionar el primer objeto v�lido
            GameObject objectToPickup = hitColliders[0].gameObject;

            if (objectToPickup != null)
            {
                PickupObject(objectToPickup);
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        heldObject = obj;

        // Desactivar f�sica del objeto mientras se sostiene
        heldObjectRb = heldObject.GetComponent<Rigidbody>();
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = true;
        }

        // Posicionar el objeto en la posici�n de "hold"
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
        heldObject.transform.parent = holdPosition;

        // Crear la previsualizaci�n
        if (previewObjectPrefab != null)
        {
            previewObject = Instantiate(previewObjectPrefab);
            previewObject.SetActive(false);
        }
    }

    void TryDropObject()
    {
        if (currentDropPoint != null)
        {
            DropObject(currentDropPoint);
        }
        else
        {
            Debug.Log("No puedes soltar el objeto aqu�. Debes estar cerca de un punto de entrega.");
        }
    }

    void DropObject(Transform dropPoint)
    {
        if (heldObject != null)
        {
            DropPoint dropPointScript = dropPoint.GetComponent<DropPoint>();
            if (dropPointScript != null && dropPointScript.isOccupied)
            {
                Debug.Log("Ya hay un objeto colocado en este punto.");
                if (previewObject != null)
                {
                    previewObject.SetActive(false);
                }
                return;
            }

            // Reactivar la f�sica del objeto al soltarlo
            if (heldObjectRb != null)
            {
                heldObjectRb.isKinematic = false;
            }

            // Obtener el centro del Collider del punto de entrega
            Collider dropPointCollider = dropPoint.GetComponent<Collider>();
            Vector3 dropPosition = dropPointCollider != null ? dropPointCollider.bounds.center : dropPoint.position;

            // Posicionar el objeto en el punto de entrega
            heldObject.transform.position = dropPosition;
            heldObject.transform.parent = dropPoint; // Hacer que el objeto sea hijo del dropPoint

            // Congelar la posici�n y rotaci�n del objeto para evitar que se mueva
            Rigidbody dropRb = heldObject.GetComponent<Rigidbody>();
            if (dropRb != null)
            {
                dropRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }

            // Marcar el punto como ocupado
            if (dropPointScript != null)
            {
                dropPointScript.isOccupied = true;
            }

            // Soltar el objeto
            heldObject = null;

            // Destruir la previsualizaci�n
            if (previewObject != null)
            {
                Destroy(previewObject);
                previewObject = null;
            }
        }
    }

    void UpdatePreview()
    {
        if (heldObject != null && previewObject != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange, dropPointLayer);
            if (hitColliders.Length > 0)
            {
                // Encontrar el punto de entrega m�s cercano
                Collider nearestDropPoint = null;
                float nearestDistance = float.MaxValue;

                foreach (Collider dropPoint in hitColliders)
                {
                    float distance = Vector3.Distance(transform.position, dropPoint.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestDropPoint = dropPoint;
                    }
                }

                if (nearestDropPoint != null)
                {
                    currentDropPoint = nearestDropPoint.transform;

                    // Obtener el centro del Collider del punto de entrega
                    Collider dropPointCollider = currentDropPoint.GetComponent<Collider>();
                    Vector3 previewPosition = dropPointCollider != null ? dropPointCollider.bounds.center : currentDropPoint.position;

                    // Colocar la previsualizaci�n exactamente en el punto de entrega
                    previewObject.SetActive(true);
                    previewObject.transform.position = previewPosition;
                    previewObject.transform.rotation = currentDropPoint.rotation;

                    // Ajustar escala si es necesario para que coincida con el dropPoint
                    previewObject.transform.localScale = currentDropPoint.localScale;
                }
            }
            else
            {
                // Desactivar la previsualizaci�n si no hay puntos cercanos
                currentDropPoint = null;
                previewObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar el rango de interacci�n en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        // Visualizar todos los puntos de entrega en el editor
        Gizmos.color = Color.green;
        Collider[] dropPoints = Physics.OverlapSphere(transform.position, interactionRange, dropPointLayer);
        foreach (Collider dropPoint in dropPoints)
        {
            Gizmos.DrawWireSphere(dropPoint.transform.position, 0.5f);
        }
    }
}

public class DropPoint : MonoBehaviour
{
    public bool isOccupied = false;
}
