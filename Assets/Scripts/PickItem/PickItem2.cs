using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem2 : MonoBehaviour
{
    #region Variables

    public Transform holdPositionE; // Posición donde se sostendrán los objetos al presionar E
    public Transform holdPositionQ; // Posición donde se sostendrán los objetos al presionar Q
    public float interactionRange = 5f; // Rango de interacción
    public LayerMask interactableLayer; // Capa para los objetos interactuables
    public LayerMask dropPointLayer; // Capa para los puntos de entrega
    public GameObject previewObjectPrefab; // Prefab de previsualización del objeto
    public Transform raycastOrigin; // Punto de origen del raycast (por ejemplo, la mano del personaje)

    private GameObject heldObjectE; // Objeto sostenido en holdPositionE
    private GameObject heldObjectQ; // Objeto sostenido en holdPositionQ
    private GameObject previewObject; // Instancia del objeto de previsualización
    private Transform currentDropPoint; // Punto de entrega más cercano
    private Rigidbody heldObjectRb; // Rigidbody temporal para manejar los objetos

    // Referencias a los paneles de la UI
    public GameObject uiPanelE; // Panel de la UI para cuando se recoge un objeto con la tecla E
    public GameObject uiPanelQ; // Panel de la UI para cuando se recoge un objeto con la tecla Q

    #endregion

    #region Metodos Unity
    void Update()
    {
        // Detectar interacción con objetos al presionar las teclas E o Q
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandlePickupOrDrop(ref heldObjectE, holdPositionE, uiPanelE);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            HandlePickupOrDrop(ref heldObjectQ, holdPositionQ, uiPanelQ);
        }

        // Actualizar la previsualización
        UpdatePreview();
    }

    #endregion

    #region Metodos PickObject+


    void HandlePickupOrDrop(ref GameObject heldObject, Transform holdPosition, GameObject uiPanel)
    {
        if (heldObject == null)
        {
            TryPickupObject(ref heldObject, holdPosition, uiPanel);
        }
        else
        {
            TryDropObject(ref heldObject, uiPanel);
        }
    }

    void TryPickupObject(ref GameObject heldObject, Transform holdPosition, GameObject uiPanel)
    {
        // Detectar objetos cercanos usando un rayo esférico
        Collider[] hitColliders = Physics.OverlapSphere(raycastOrigin.position, interactionRange, interactableLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject objectToPickup = hitCollider.gameObject;

            // Verificar que no es el mismo objeto que ya se sostiene en otra posición
            if (objectToPickup != heldObjectE && objectToPickup != heldObjectQ)
            {
                PickupObject(ref heldObject, objectToPickup, holdPosition, uiPanel);
                return; // Salir del bucle después de recoger un objeto
            }
        }
    }

    void PickupObject(ref GameObject heldObject, GameObject obj, Transform holdPosition, GameObject uiPanel)
    {
        heldObject = obj; // Asignar el objeto recogido

        // Desactivar física del objeto mientras se sostiene
        heldObjectRb = heldObject.GetComponent<Rigidbody>();
        if (heldObjectRb != null)
        {
            heldObjectRb.isKinematic = true;
        }

        // Posicionar el objeto en la posición de sostén
        heldObject.transform.position = holdPosition.position;
        heldObject.transform.rotation = holdPosition.rotation;
        heldObject.transform.parent = holdPosition;

        // Crear la previsualización si no existe ya
        if (previewObjectPrefab != null && previewObject == null)
        {
            previewObject = Instantiate(previewObjectPrefab);
        }

        // Activar la previsualización
        if (previewObject != null)
        {
            previewObject.SetActive(true);
        }

        // Mostrar el panel correspondiente de la UI
        uiPanel.SetActive(true);
    }

    void TryDropObject(ref GameObject heldObject, GameObject uiPanel)
    {
        if (currentDropPoint != null)
        {
            DropObject(ref heldObject, uiPanel, currentDropPoint);
        }
        else
        {
            Debug.Log("No puedes soltar el objeto aquí. Debes estar cerca de un punto de entrega.");
        }
    }

    void DropObject(ref GameObject heldObject, GameObject uiPanel, Transform dropPoint)
    {
        if (heldObject != null)
        {
            DropPoint2 dropPointScript = dropPoint.GetComponent<DropPoint2>();
            if (dropPointScript != null && dropPointScript.isOccupied)
            {
                Debug.Log("Ya hay un objeto colocado en este punto.");
                if (previewObject != null)
                {
                    previewObject.SetActive(false);
                }
                return;
            }

            // Reactivar la física del objeto al soltarlo
            Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
            if (heldObjectRb != null)
            {
                heldObjectRb.isKinematic = false;
            }

            // Posicionar el objeto en el centro del punto de entrega
            Vector3 dropPosition = dropPoint.position;
            heldObject.transform.position = dropPosition;
            heldObject.transform.parent = dropPoint;

            // Congelar la posición y rotación del objeto para evitar que se mueva
            if (heldObjectRb != null)
            {
                heldObjectRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }

            // Marcar el punto como ocupado
            if (dropPointScript != null)
            {
                dropPointScript.isOccupied = true;
            }

            // Soltar el objeto
            heldObject = null;

            // Destruir la previsualización si no hay objetos sostenidos
            if (heldObjectE == null && heldObjectQ == null && previewObject != null)
            {
                Destroy(previewObject);
                previewObject = null;
            }

            // Ocultar el panel correspondiente de la UI
            uiPanel.SetActive(false);
        }
    }

    void UpdatePreview()
    {
        if ((heldObjectE != null || heldObjectQ != null) && previewObject != null)
        {
            Vector3 originPosition = raycastOrigin != null ? raycastOrigin.position : transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(originPosition, interactionRange, dropPointLayer);

            if (hitColliders.Length > 0)
            {
                Collider nearestDropPoint = null;
                float nearestDistance = float.MaxValue;

                foreach (Collider dropPoint in hitColliders)
                {
                    if (dropPoint.CompareTag("DropPoint"))
                    {
                        float distance = Vector3.Distance(originPosition, dropPoint.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestDropPoint = dropPoint;
                        }
                    }
                }

                if (nearestDropPoint != null)
                {
                    currentDropPoint = nearestDropPoint.transform;

                    Vector3 previewPosition = currentDropPoint.position;
                    previewObject.SetActive(true);
                    previewObject.transform.position = previewPosition;
                    previewObject.transform.rotation = currentDropPoint.rotation;
                    previewObject.transform.localScale = currentDropPoint.localScale;
                }
            }
            else
            {
                currentDropPoint = null;
                previewObject.SetActive(false);
            }
        }
        else
        {
            currentDropPoint = null;
            if (previewObject != null)
            {
                previewObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);

        Gizmos.color = Color.green;
        Collider[] dropPoints = Physics.OverlapSphere(transform.position, interactionRange, dropPointLayer);
        foreach (Collider dropPoint in dropPoints)
        {
            Gizmos.DrawWireSphere(dropPoint.transform.position, 0.5f);
        }
    }
}

#endregion

#region DropPoint
public class DropPoint2 : MonoBehaviour
{
    public bool isOccupied = false;
    public bool canBePlaced = true;
}

#endregion




