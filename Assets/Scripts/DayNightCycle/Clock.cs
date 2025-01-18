using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public float dayDurationInRealSeconds = 15f * 60f; // 15 minutos en segundos
    public Text clockText; // El texto en la UI para mostrar la hora
    private float timeOfDay = 0f; // El tiempo del día (0 = amanecer, 1 = anochecer)
    private float hourInterval = 1f / 24f; // El intervalo de tiempo para avanzar 1 hora en el ciclo de 24 horas del juego

    void Update()
    {
        // Avanzar el tiempo en función del tiempo real
        timeOfDay += (Time.deltaTime / dayDurationInRealSeconds);

        if (timeOfDay >= 1f)
        {
            timeOfDay = 0f; // Reiniciar el ciclo de día
        }

        // Solo actualizamos el reloj cada vez que pasa una hora dentro del juego
        if (timeOfDay % hourInterval < Time.deltaTime / dayDurationInRealSeconds)
        {
            // Actualizar el reloj en pantalla
            UpdateClockUI();
        }
    }

    void UpdateClockUI()
    {
        // Convertir el tiempo del día en horas y minutos
        int hours = Mathf.FloorToInt(timeOfDay * 24f);
        int minutes = Mathf.FloorToInt((timeOfDay * 24f - hours) * 60f);

        // Actualizar el texto del reloj en la UI
        clockText.text = string.Format("{0:D2}:{1:D2}", hours, minutes);
    }
}
