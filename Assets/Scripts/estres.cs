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
    public float eventTime = 5f;

    private bool isBlackOut = false;
    private float time = 0f;

    private void Update()
    {
        //Mandamos el evento del tamaño de la mancha si corresponde
        time += Time.deltaTime;
        if (time >= eventTime)
        {
            Tracker.GetInstance().TrackEvent(new BlackoutIntensityVolume(nivelEstres / 100));
            time = 0f;
        }

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
    }
}
