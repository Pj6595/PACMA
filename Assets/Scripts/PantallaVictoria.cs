using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PantallaVictoria : MonoBehaviour
{
    public Image imagen;
    
    public float threshHold;
    
    public void setMicVol(float v)
    {
        if (v > threshHold)
        {
            SceneManager.LoadSceneAsync("MenuPrincipal");
        }
    }
}
