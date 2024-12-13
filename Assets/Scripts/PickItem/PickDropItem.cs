using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDropItem : MonoBehaviour
{
    [SerializeField] private Transform _detectItemPoint;
    [SerializeField] private LayerMask pickupLayerMask;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float pickupDistance = 2f;
            if (Physics.Raycast(_detectItemPoint.position, _detectItemPoint.forward, out RaycastHit raycastHit, pickupDistance))
            {
                Debug.Log(raycastHit.transform);
            }
                
            
        }
    }
}
