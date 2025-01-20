using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrev : MonoBehaviour
{
    public GameObject previewObjectPrefab;
    public float interactionRange = 5f;
    public LayerMask dropPointLayer;
    public Transform raycastOrigin;

    private GameObject previewObject;
    private Transform currentDropPoint;

    public void InitializePreview()
    {
        if (previewObjectPrefab != null)
        {
            previewObject = Instantiate(previewObjectPrefab);
            previewObject.SetActive(false);
        }
    }

    public void UpdatePreview(GameObject heldObject)
    {
        if (heldObject != null && previewObject != null)
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
                    previewObject.SetActive(true);
                    previewObject.transform.position = currentDropPoint.position;
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

    public void DestroyPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }
}
