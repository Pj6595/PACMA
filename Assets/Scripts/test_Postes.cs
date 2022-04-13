using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class test_Postes : MonoBehaviour
{
    public List<GameObject> vecPostes;
    // Start is called before the first frame update
    void Start()
    {
        //POSTE DE TAMAÑO 1

        Poste poste = Instantiate(vecPostes[0]).GetComponent<Poste>();
        List<Sentido> direcciones = new List<Sentido>();

        Sentido dir;
        dir = Sentido.Recto;
        direcciones.Add(dir);

        poste.Init(new Vector3(0, 0, 0), direcciones, 0,Sentido.Derecha);

        //POSTE DE TAMAÑO 2
        poste = Instantiate(vecPostes[1]).GetComponent<Poste>();
        direcciones = new List<Sentido>();

        dir = Sentido.Derecha;

        direcciones.Add(dir);

        dir = Sentido.Izquierda;

        direcciones.Add(dir);

        poste.Init(new Vector3(10, 0, 0), direcciones, 1);

        //POSTE DE TAMAÑO 3
        poste = Instantiate(vecPostes[2]).GetComponent<Poste>();
        direcciones = new List<Sentido>();

        dir = Sentido.Recto;

        direcciones.Add(dir);

        dir = Sentido.Derecha;

        direcciones.Add(dir);

        dir = Sentido.Recto;
 

        direcciones.Add(dir);

        poste.Init(new Vector3(20, 0, 0), direcciones, 2);

        //POSTE DE TAMAÑO 4
        poste = Instantiate(vecPostes[3]).GetComponent<Poste>();
        direcciones = new List<Sentido>();

        dir = Sentido.Izquierda;

        direcciones.Add(dir);

        dir = Sentido.Recto;


        direcciones.Add(dir);

        poste.Init(new Vector3(50, 0, 0), direcciones, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
