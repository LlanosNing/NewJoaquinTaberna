using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image panelBrightness, panelBrightness2;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("brightness, 1f");//player prefabs hace que afecte a todos los objetos durante todo el juego
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
        panelBrightness2.color = new Color(panelBrightness2.color.r, panelBrightness2.color.g, panelBrightness2.color.b, slider.value);
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brightness", sliderValue);
        panelBrightness.color = new Color(panelBrightness.color.r, panelBrightness.color.g, panelBrightness.color.b, slider.value);
        panelBrightness2.color = new Color(panelBrightness2.color.r, panelBrightness2.color.g, panelBrightness2.color.b, slider.value);
    }
}
