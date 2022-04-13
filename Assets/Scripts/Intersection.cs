using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    public List<Utility.Sentido> salidas;
    private Utility.Sentido correcta;
    private bool chosen = false;
    void Start()
    {
        if (!chosen)
        {
            correcta = salidas[Random.Range(0, salidas.Count)];
            chosen = true;
        }
    }
    public Utility.Sentido getCorrect()
    {
        if (!chosen)
        {
            correcta = salidas[Random.Range(0, salidas.Count)];
            chosen = true;
        }
        return correcta;
    }
}
