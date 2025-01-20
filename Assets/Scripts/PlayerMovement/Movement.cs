using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Start()
    {

    }

    void Update()
    {
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");
        //transform.Translate(new Vector3(_horizontal, 0, _vertical) * moveSpeed * Time.deltaTime); //el vertical es la Z aunque ponga vertical. El delta time es para que no vaya loco
        transform.Translate(new Vector3(_horizontal, 0, _vertical).normalized * moveSpeed * Time.deltaTime);
        //en la linea de arriba en diagonal te mueves mas rapido. para arreglarlo poner el .normalized (normalizar vector)
    }
}
