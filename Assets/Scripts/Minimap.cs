using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject panel;
    private bool isOn;
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOn)
            {
                ResumeFromKM();
            }
            else
            {
                ShowKeyboardMenu();
            }
        }
    }
    public void ShowKeyboardMenu()
    {
        //Cursor.visible = true;
        panel.SetActive(true);
        Time.timeScale = 0f;
        isOn = true;
    }

    public void ResumeFromKM()
    {
        //Cursor.visible = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
        isOn = false;
    }
}
