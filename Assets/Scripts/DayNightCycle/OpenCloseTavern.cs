using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseTavern : MonoBehaviour
{
   public GameObject openPanel; // Panel que muestra "ABIERTO"
    public GameObject closedPanel; // Panel que muestra "CERRADO"

    private bool isOpened = false; // Estado inicial: cerrado

    void Start()
    {
        // Asegúrate de que el panel "CERRADO" esté activo y "ABIERTO" desactivado al inicio
        openPanel.SetActive(false);
        closedPanel.SetActive(true);
    }

    void Update()
    {
        // Detectar si se pulsa la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeOpenState();
        }
    }

    void ChangeOpenState()
    {
        // Cambiar el estado entre abierto y cerrado
        isOpened = !isOpened;

        // Actualizar la visibilidad de los paneles
        openPanel.SetActive(isOpened);
        closedPanel.SetActive(!isOpened);
    }
}
