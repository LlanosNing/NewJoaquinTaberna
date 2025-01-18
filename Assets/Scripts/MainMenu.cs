using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private FadeScreen _fd;
    void Start()
    {
        _fd = GameObject.Find("FSmanager").GetComponent<FadeScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Que el selector vaya a la posición. Coge la posición de este objeto en el transform y la cambia por el Vector que le pasamos 
            transform.position = new Vector3(38f, 243.1f, 0f);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego");
    }
}
