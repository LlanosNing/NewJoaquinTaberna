using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanel : MonoBehaviour
{
    #region Variables

    public GameObject uiPanelE; // Panel de la UI para cuando se recoge un objeto con la tecla E
    public GameObject uiPanelQ; // Panel de la UI para cuando se recoge un objeto con la tecla Q

    #endregion

    public void ShowUIPanel(Transform holdPosition)
    {
        if (holdPosition == null) return;

        if (holdPosition.CompareTag("HoldPointE"))
        {
            uiPanelE.SetActive(true); // Mostrar el panel cuando se recoge un objeto en el holdPointE
        }
        else if (holdPosition.CompareTag("HoldPointQ"))
        {
            uiPanelQ.SetActive(true); // Mostrar el panel cuando se recoge un objeto en el holdPointQ
        }
    }

    public void HideUIPanel(Transform holdPosition)
    {
        if (holdPosition == null) return;

        if (holdPosition.CompareTag("HoldPointE"))
        {
            uiPanelE.SetActive(false); // Ocultar el panel cuando se suelta un objeto en el holdPointE
        }
        else if (holdPosition.CompareTag("HoldPointQ"))
        {
            uiPanelQ.SetActive(false); // Ocultar el panel cuando se suelta un objeto en el holdPointQ
        }
    }
}
