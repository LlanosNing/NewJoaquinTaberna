using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoMouse : MonoBehaviour
{
    public Button miboton;

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetMouseButtonDown(0))
        {
            miboton.Select();
        }



    }
}
