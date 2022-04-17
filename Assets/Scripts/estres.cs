using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Pacmetricas_G01;

public class estres : MonoBehaviour
{
    public float nivelEstres = 0;
    public RawImage imagen;
    public Animator animador;
    public float stressIncrement;
    public ShaderEstresScript shaderEstressScript;

    private bool isBlackOut = false;

    private void Update()
    {
        UpdateEstres(stressIncrement*Time.deltaTime);
        if (nivelEstres >= 100 && !isBlackOut)
        {
            isBlackOut = true;  //Marcamos que el jugador se ha desmayado

            //Mandamos el evento de que el jugador se desmayó y perdió la partida
            Tracker.GetInstance().TrackEvent(new PlayerDeadEvent());
            Tracker.GetInstance().FlushAllEvents();

            //Cargamos la escena de perder la partida
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Perdida");
            
        }

    }

    public void UpdateEstres(float nivel)
    {
        nivelEstres += nivel;
        nivelEstres = Mathf.Clamp(nivelEstres, 0, 100);
        //Debug.Log("Nivel de estres: " + nivelEstres);
        Color color = imagen.color;
        color.a = Mathf.Pow(nivelEstres / 100, 2);
        imagen.color = color;
        //Debug.Log("Alfa de la imagen: " + imagen.color.a);
        animador.speed = 1 + (nivelEstres / 10);
        shaderEstressScript.updateIntensityVignete(nivelEstres / 100);
        //Debug.Log("Velocidad de la animación " + animador.speed);


        //Mandamos el evento del tamaño de la mancha si corresponde
        Tracker.GetInstance().TrackEvent(new BlackoutIntensityVolume(0.5f));
    }
}
