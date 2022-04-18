using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pacmetricas_G01;


public class PantallaVictoria : MonoBehaviour
{
    public Image imagen;
    
    public float threshHold;
    
    public void setMicVol(float v)
    {
        if (v > threshHold)
        {
            Tracker.GetInstance().TrackEvent(new PlayerWonEvent());
            SceneManager.LoadSceneAsync("MenuPrincipal");
        }
    }
}
