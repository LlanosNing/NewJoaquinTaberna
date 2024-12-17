using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public NavMeshAgent agent; // El agente de navegación que se moverá hacia el destino
    public LayerMask dropPointLayer; // Capa de los puntos de entrega (drop points)
    public float searchRadius = 50f; // Radio para buscar puntos de entrega
    public string targetObjectName = "Silla"; // El nombre del objeto hijo que queremos que el NPC encuentre

    private Transform targetDropPointChild; // El objeto hijo del punto de entrega al que se moverá el NPC

    void Start()
    {
        // Si el NavMeshAgent no está asignado, lo asignamos desde el componente del NPC
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Buscar un punto de entrega aleatorio
        FindRandomDropPoint();
    }

    void Update()
    {
        // Si el agente ha llegado a su destino, buscar otro punto de entrega aleatorio
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            FindRandomDropPoint();
        }
    }

    // Método para buscar un punto de entrega aleatorio en la escena
    void FindRandomDropPoint()
    {
        // Buscar todos los puntos de entrega dentro del radio
        Collider[] dropPoints = Physics.OverlapSphere(transform.position, searchRadius, dropPointLayer);

        if (dropPoints.Length > 0)
        {
            // Seleccionar un punto de entrega aleatorio de la lista
            Collider randomDropPoint = dropPoints[Random.Range(0, dropPoints.Length)];

            // Buscar el objeto hijo con el nombre específico (por ejemplo, "Silla")
            Transform randomDropPointChild = randomDropPoint.transform.Find(targetObjectName);

            if (randomDropPointChild != null)
            {
                targetDropPointChild = randomDropPointChild;
                agent.SetDestination(targetDropPointChild.position);
                Debug.Log("NPC se dirige a: " + targetDropPointChild.name);
            }
            else
            {
                Debug.LogWarning("El punto de entrega no tiene un hijo con el nombre: " + targetObjectName);
            }
        }
        else
        {
            Debug.LogWarning("No se encontraron puntos de entrega cerca.");
        }
    }
}
