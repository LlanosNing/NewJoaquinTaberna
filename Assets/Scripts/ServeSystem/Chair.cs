using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool isOccupied = false; // Whether the chair is occupied

    // Mark the chair as occupied
    public void Occupy()
    {
        isOccupied = true;
    }

    // Free up the chair (e.g., when the NPC is attended)
    public void Release()
    {
        isOccupied = false;
    }
}


