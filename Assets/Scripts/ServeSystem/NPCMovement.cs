using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public NavMeshAgent agent; // NavMeshAgent for movement
    private Chair targetChair; // Chair the NPC will sit on
    private bool isSeated = false; // Whether the NPC is seated

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        // Look for a random available chair
        FindChair();
    }

    void Update()
    {
        // If already seated, do nothing
        if (isSeated) return;

        // If the NPC has reached the chair, "sit on it"
        if (targetChair != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SitOnChair();
        }
    }

    void FindChair()
    {
        // Find all tables in the scene
        Table[] tables = FindObjectsOfType<Table>();
        Chair[] availableChairs = GetAvailableChairs(tables);

        if (availableChairs.Length > 0)
        {
            // Select a random chair from the available ones
            targetChair = availableChairs[Random.Range(0, availableChairs.Length)];
            targetChair.Occupy(); // Mark the chair as occupied
            agent.SetDestination(targetChair.transform.position);
        }
        else
        {
            Debug.LogWarning("No available chairs for the NPC.");
        }
    }

    Chair[] GetAvailableChairs(Table[] tables)
    {
        var availableChairsList = new System.Collections.Generic.List<Chair>();

        foreach (Table table in tables)
        {
            foreach (Chair chair in table.chairs)
            {
                if (!chair.isOccupied)
                {
                    availableChairsList.Add(chair);
                }
            }
        }

        return availableChairsList.ToArray();
    }

    void SitOnChair()
    {
        isSeated = true;
        agent.isStopped = true;

        // Position the NPC directly on the chair and align rotation
        transform.position = targetChair.transform.position; // <-- NPC sits directly on the chair's position
        transform.rotation = targetChair.transform.rotation; // <-- NPC aligns with the chair's rotation

        Debug.Log("NPC is seated on the chair: " + targetChair.name);
    }
}
