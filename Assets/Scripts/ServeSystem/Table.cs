using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Chair[] chairs; // List of chairs associated with this table

    void Awake()
    {
        // Automatically find all child chairs if none are assigned
        if (chairs == null || chairs.Length == 0)
        {
            chairs = GetComponentsInChildren<Chair>();
        }
    }
}

