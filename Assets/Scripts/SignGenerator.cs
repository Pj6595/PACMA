using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class SignGenerator : MonoBehaviour
{
    public List<GameObject> vecPostes;
    public float extraDistance = 50f;
    private PlayerCollisionHandler playerCollision; 

    /// <summary>
    ///  ID [0 - n� se�ales) de la imagen del sitio al que vamos.
    ///  Posiblemente acabar� estando en el PartidaManager.
    /// </summary>
    public int imgID;

    /// <summary>
    /// COMENTARIOS
    /// </summary>
    void Start()
    {
        playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionHandler>();
        if (!playerCollision)
            Debug.Log("No pille la colision xd");
    }

    /// <summary>
    /// </summary>
    /// <returns> una direccion aleatoria de entre las disponibles en inter.salidas </returns>
    Utility.Sentido GetRandomSentidoAvailable(Intersection inter)
    {
        Utility.Sentido s;
        do {
            s = Random.Range((int)Utility.Sentido.Derecha, (int)Utility.Sentido.Recto + 1) + Utility.Sentido.Derecha;
        } 
        while (!inter.salidas.Contains(s));
        return s;
    }
    /// <summary>
    /// Coloca un poste con numero de carteles aleatorio
    /// </summary>
    /// <param name="inter"> Las intersecciones del siguiente cruce </param>
    public Poste PlacePoste(GameObject intersectionGO)
    {
        //Estupido orden de inicializacion
        if(playerCollision==null)
            playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionHandler>();

        Intersection inter = intersectionGO.GetComponent<Intersection>();
        // el numero de direcciones del cartel es aleatorio entre las direcciones posibles y el maximo de carteles
        int numCarteles = inter.salidas.Count;

        Poste poste = Instantiate(vecPostes[numCarteles - 1]).GetComponent<Poste>();
        List<Utility.Sentido> dirsList = new List<Utility.Sentido>();

        for (int i = 0; i < numCarteles; i++)
        {
            int j;
            do {
                j = Random.Range(0, inter.salidas.Count);
            }
            while (dirsList.Contains(inter.salidas[j]));
            dirsList.Add(inter.salidas[j]);
        }

        Vector3 pos = intersectionGO.GetComponent<PosCartel>().GetPos() - playerCollision.getLogicF() * extraDistance;
        poste.Init(pos, dirsList, numCarteles, inter.getCorrect());
        poste.transform.Rotate(new Vector3(0.0f, intersectionGO.transform.rotation.eulerAngles.y, 0.0f));
        poste.transform.SetParent(intersectionGO.transform);

        return poste;
    }
}