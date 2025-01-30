using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public bool isLogoScreen;
    //Referencia al FadeScreen
    public Image fadeScreenB; /*fadeScreenW;*/
    //Variable para la velocidad de transición al FadeScreen
    public float fadeSpeed;
    //Variables para conocer cuando hacemos fundido a negro o vuelta a transparente
    [SerializeField] private bool shouldFadeToBlack, shouldFadeFromBlack; /*shouldFadeToWhite, shouldFadeFromWhite;*/

    [SerializeField] private GameManager _gm;

    
    void Start()
    {
        if (isLogoScreen == true)
        {
            FSLogoScreen();
        }
        else
        {
            FadeFromBlack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Si hay que hacer fundido a negro
        if (shouldFadeToBlack)
        {
            fadeScreenB.color = new Color(fadeScreenB.color.r, fadeScreenB.color.g, fadeScreenB.color.b, Mathf.MoveTowards(fadeScreenB.color.a, 1f, fadeSpeed * Time.deltaTime));
            //Mathf.MoveTowards (Moverse hacia) -> el valor que queremos cambiar, valor al que lo queremos cambiar, velocidad a la que lo queremos cambiar
            //Si el color ya es totalmente opaco
            if (fadeScreenB.color.a == 1f)
                //Paramos de hacer fundido a negro
                shouldFadeToBlack = false;
        }

        if (shouldFadeFromBlack)
        {
            //Cambiar la transparencia del color a transparente
            fadeScreenB.color = new Color(fadeScreenB.color.r, fadeScreenB.color.g, fadeScreenB.color.b, Mathf.MoveTowards(fadeScreenB.color.a, 0f, fadeSpeed * Time.deltaTime));
            //Mathf.MoveTowards (Moverse hacia) -> el valor que queremos cambiar, valor al que lo queremos cambiar, velocidad a la que lo queremos cambiar
            //Si el color ya es totalmente transparente
            if (fadeScreenB.color.a == 0f)
                //Paramos de hacer fundido a trasnparente
                shouldFadeFromBlack = false;
        }

        //if (shouldFadeToWhite)
        //{
        //    fadeScreenW.color = new Color(fadeScreenW.color.r, fadeScreenW.color.g, fadeScreenW.color.b, Mathf.MoveTowards(fadeScreenW.color.a, 1f, fadeSpeed * Time.deltaTime));
            
        //    if (fadeScreenW.color.a == 1f)
        //        shouldFadeToWhite = false;
        //}

        //if (shouldFadeFromWhite)
        //{
        //    fadeScreenW.color = new Color(fadeScreenW.color.r, fadeScreenW.color.g, fadeScreenW.color.b, Mathf.MoveTowards(fadeScreenW.color.a, 0f, fadeSpeed * Time.deltaTime));
           
        //    if (fadeScreenW.color.a == 0f)
        //        shouldFadeFromWhite = false;
        //}
    }


    public void FSLogoScreen()
    {
        StartCoroutine(FadeLogoScreenCo());
    }
    public IEnumerator FadeLogoScreenCo()
    {
        FadeFromBlack();
        yield return new WaitForSeconds(2);
        FadeToBlack();
        yield return new WaitForSeconds(1);
        _gm.ExitLevel();
    }

    public void FadeToBlack()
    {
        //Activamos la booleana de fundido a negro
        shouldFadeToBlack = true;
        //Desactivamos la booleana de fundido a transparente
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }

    //public void FadeToWhite()
    //{
    //    shouldFadeToWhite = true;
    //    shouldFadeFromWhite = false;
    //}

    //public void FadeFromWhite()
    //{
    //    shouldFadeFromWhite = true;
    //    shouldFadeToWhite = false;
    //}
}
