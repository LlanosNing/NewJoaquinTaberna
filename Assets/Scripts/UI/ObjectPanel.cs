using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanel : MonoBehaviour
{
    #region Variables

    public GameObject uiPanelE;
    public GameObject uiPanelQ;

    private PickItem _pI;

    private void Start()
    {
        _pI = GameObject.Find("Player").GetComponent<PickItem>();
    }

    #endregion

    private void Update()
    {
        ShowUIPanel();
        HideUIPanel();
    }

    public void ShowUIPanel()
    {
        //if (holdPosition == null) return;

        if (_pI.isObjHeld == true)
        {
            uiPanelE.SetActive(true); // Mostrar el panel cuando se recoge un objeto en el holdPointE
        }
        //else if (holdPosition.CompareTag("HoldPointQ"))
        //{
        //    uiPanelQ.SetActive(true); // Mostrar el panel cuando se recoge un objeto en el holdPointQ
        //}
    }

    public void HideUIPanel()
    {
        //if (holdPosition == null) return;

        if (_pI.isObjHeld == false)
        {
            uiPanelE.SetActive(false); // Ocultar el panel cuando se suelta un objeto en el holdPointE
        }
        //else if (holdPosition.CompareTag("HoldPointQ"))
        //{
        //    uiPanelQ.SetActive(false); // Ocultar el panel cuando se suelta un objeto en el holdPointQ
        //}
    }
}
