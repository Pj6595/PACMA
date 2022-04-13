using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Bocadillos { izquierda, derecha, recto, confuso }

public class BocadilloConductor : MonoBehaviour
{
    //private Animator animator;
    private Animation anim;
    private Image bocadillo;
    public List<Image> imagenesBocadillo;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        anim = GetComponent<Animation>();
        bocadillo = GetComponent<Image>();
        disableBocadillo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) hideBocadillo();
    }

    public void showBocadillo(Bocadillos estado)
    {
        disableBocadillo();
        switch (estado)
        {
            case Bocadillos.izquierda:
                imagenesBocadillo[0].enabled = true;
                break;
            case Bocadillos.derecha:
                imagenesBocadillo[1].enabled = true;
                break;
            case Bocadillos.recto:
                imagenesBocadillo[2].enabled = true;
                break;
            case Bocadillos.confuso:
                imagenesBocadillo[3].enabled = true;
                break;
        }
        bocadillo.enabled = true;
        anim.Play("BocadilloConductor");
    }

    public void hideBocadillo()
    {
        if (bocadillo.enabled)
            anim.Play("BocadilloConductorHide");
    }

    void disableBocadillo()
    {
        foreach (Image im in imagenesBocadillo) im.enabled = false;
        bocadillo.enabled = false;
    }
}
