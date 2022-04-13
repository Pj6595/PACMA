using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class CityGenerator : MonoBehaviour
{
    //Parametros de generacion
    public List<GameObject> straights;
    public List<GameObject> intersections;
    public List<GameObject> straightIntersections;
    public GameObject hospitalTile;
    public int tileSize = 5;
    public int maxStraight = 3;
    public int minStraight = 1;

    [Range(0, 1)]
    public float failChance = 0.5f;
    private bool playerDecision = false;
    //Jugador
    public GameObject player;
    public GameObject carRotationPivot;
    public float playerSpeed = 1;
    public float playerSpeedIncreaseMultiplier = 0.1f;
    private bool moving = false;
    private Sentido playerNextDir = Sentido.Recto;
    public PlayerCollisionHandler playerColHandler;
    public float tiempoAnimGirar = 1f;
    public estres EstresScript;
    public float stressIncreaseMultiplier = 0.1f;

    //Listas para guardar las carreteras que vamos generando
    private List<GameObject> currentCarretera;
    GameObject inters = null, oldInters = null, interRecta = null,
        lastTile = null, tileOptIzq = null, tileOptDer = null;
    Vector3 facingVec = new Vector3(0, 0, 1);

    private SignGenerator signGenerator;

    public GameObject GetInters() { return inters; }
    //Gamemanager
    GameManager gM = GameManager.instance;

    //Bocadillo conductor
    public BocadilloConductor bocadillo;

    //particulas anime gottagofast deja vu 
    public ParticleSystem animeParticles;
    public Color colorParticulasMinimo;
    public Color incrementoColorParticulas;

    GameObject PlaceTile(GameObject tile, Vector3 direccionVec, Vector3 previousTilePos)
    {
        return Instantiate(tile, previousTilePos + tileSize * direccionVec, Quaternion.LookRotation(direccionVec, Vector3.up));
    }

    Vector3 rotaVector(Vector3 vec, Sentido dir)
    {
        if (dir == Sentido.Derecha)
            return Quaternion.AngleAxis(90, Vector3.up) * vec;
        else if (dir == Sentido.Izquierda)
            return Quaternion.AngleAxis(-90, Vector3.up) * vec;
        else
            return vec;
    }

    public void StartMoving()
    {
        moving = true;
    }

    void generaTilesSalidaInterseccion(Vector3 direccionVec)
    {
        //Generar una tile mas a cada lado de la interseccion
        Intersection interActual = inters.GetComponent<Intersection>();
        //Salida izquierda
        if (interActual.salidas.Contains(Sentido.Izquierda))
        {
            tileOptIzq = PlaceTile(straights[Random.Range(0, straights.Count)], rotaVector(direccionVec, Sentido.Izquierda), inters.transform.position);
        }
        if (interActual.salidas.Contains(Sentido.Derecha))
        {
            tileOptDer = PlaceTile(straights[Random.Range(0, straights.Count)], rotaVector(direccionVec, Sentido.Derecha), inters.transform.position);
        }
        if (interActual.salidas.Contains(Sentido.Recto))
        {
            //Generar otro tramo
            GeneraTramo(direccionVec, inters.transform.position, true);
        }
    }

    void GeneraTramo(Vector3 direccionVec, Vector3 startPos, bool generatingStraightExtra)
    {
        lastTile = PlaceTile(straights[Random.Range(0, straights.Count)], direccionVec, startPos);
        currentCarretera.Add(lastTile);

        //Coloca una cantidad de rectas aleatorias
        int nTiles = Random.Range(minStraight, maxStraight);
        for (int i = 1; i < nTiles; i++)
        {
            lastTile = PlaceTile(straights[Random.Range(0, straights.Count)], direccionVec, lastTile.transform.position);
            currentCarretera.Add(lastTile);
        }

        Debug.Log("Puntos actuales : " + gM.GetPoints() + ". Puntos necesarios :" + gM.GetPointsForWin());

        if (gM.GetPoints() >= gM.GetPointsForWin())
        {
            PlaceTile(hospitalTile, direccionVec, lastTile.transform.position);
        }
        else
        {
            //Coloca una interseccion
            if (!generatingStraightExtra)
            {
                //50% de que salga recta con algun variante
                if (Random.Range(0, 100) % 2 == 0)
                {
                    Debug.Log("Spawning straight");
                    //Crear recta
                    inters = PlaceTile(straightIntersections[Random.Range(0, straightIntersections.Count)], direccionVec, lastTile.transform.position);
                }
                else
                {
                    //Crear normal
                    inters = PlaceTile(intersections[Random.Range(0, intersections.Count)], direccionVec, lastTile.transform.position);

                }
                signGenerator.PlacePoste(inters);
            }
            else
            {
                interRecta = PlaceTile(intersections[Random.Range(0, intersections.Count)], direccionVec, lastTile.transform.position);
                signGenerator.PlacePoste(interRecta);
            }
        }

        if (!generatingStraightExtra)
            generaTilesSalidaInterseccion(direccionVec);
    }

    void cleanCarretera()
    {
        foreach (GameObject item in currentCarretera)
        {
            Destroy(item);
        }
        currentCarretera.Clear();
    }

    Sentido pickRandomDir(Intersection inter, Sentido correcta)
    {
        Debug.Log("Random para " + correcta);
        if (Random.Range(0f, 1f) > failChance)
        {
            Debug.Log("Devuelve correcta " + correcta);
            return correcta;
        }
        int i = 0;
        //Si peta aqui es culpa del que ha hecho la interseccion, no del codigo
        while (inter.salidas[i] == correcta)
            i++;
        Debug.Log("Devuelve falsa" + inter.salidas[i]);
        return inter.salidas[i];
    }

    void initMovement(Sentido dir, Intersection inter)
    {
        Destroy(oldInters);
        oldInters = inters;

        bool conductorDecidio = false;
        if (!playerDecision || !inter.salidas.Contains(dir))
        {
            //A menos que se pueda ir recto coge una aleatoria a otra direccion
            if (inter.salidas.Contains(Sentido.Recto))
                dir = Sentido.Recto;
            else
                dir = pickRandomDir(inter, inter.getCorrect());

            conductorDecidio = true;
        }

        if (dir == Sentido.Izquierda && inter.salidas.Contains(Sentido.Izquierda))
        {
            playerColHandler.prepareRotation(Sentido.Izquierda);
            playerColHandler.rotatationTiltAnimation(Sentido.Izquierda);

            //El jugador decide girar a la izquierda y puede
            playerColHandler.logicFRotate(-90);
            LeanTween.rotateAroundLocal(carRotationPivot, new Vector3(0, 1, 0), -90, tiempoAnimGirar).setOnComplete(playerColHandler.endRotation);


            Destroy(tileOptDer);
            cleanCarretera();
            currentCarretera.Add(interRecta);
            currentCarretera.Add(tileOptIzq);
            GeneraTramo(playerColHandler.getLogicF(), tileOptIzq.transform.position, false);
        }
        else if (dir == Sentido.Derecha && inter.salidas.Contains(Sentido.Derecha))
        {
            playerColHandler.prepareRotation(Sentido.Derecha);
            playerColHandler.rotatationTiltAnimation(Sentido.Derecha);

            //El jugador decide girar a la derecha y puede
            playerColHandler.logicFRotate(90);
            LeanTween.rotateAroundLocal(carRotationPivot, new Vector3(0, 1, 0), 90, tiempoAnimGirar).setOnComplete(playerColHandler.endRotation);


            Destroy(tileOptIzq);
            cleanCarretera();
            currentCarretera.Add(interRecta);
            currentCarretera.Add(tileOptDer);
            GeneraTramo(playerColHandler.getLogicF(), tileOptDer.transform.position, false);
        }
        else if (dir == Sentido.Recto && inter.salidas.Contains(Sentido.Recto))
        {
            //El jugador decide no girar y puede
            Destroy(tileOptDer);
            Destroy(tileOptIzq);
            inters = interRecta;
            generaTilesSalidaInterseccion(player.transform.forward);
        }
        else
        {
            Debug.LogError("Esto no debería ocurrir, sorry eksdi");
        }

        if (dir == inter.getCorrect())
        {
            if (!conductorDecidio)
            {
                gM.AddPoint();
                //Subir la velocidad
                playerSpeed *= playerSpeedIncreaseMultiplier;
                tiempoAnimGirar /= playerSpeedIncreaseMultiplier;
                animeParticles.startColor += incrementoColorParticulas; 
            }
        }
        else
        {
            gM.RemovePoint();
            //Soniditos y vainas de pj
            EstresScript.stressIncrement *= stressIncreaseMultiplier;
            if (playerSpeed > 0.1)
            {
                tiempoAnimGirar *= playerSpeedIncreaseMultiplier;
                playerSpeed /= playerSpeedIncreaseMultiplier;

            }
        }

        //Reset stuff
        playerNextDir = Sentido.Recto;
        playerDecision = false;
        bocadillo.hideBocadillo();
    }

    void Start()
    {
        signGenerator = GetComponent<SignGenerator>();
        currentCarretera = new List<GameObject>();
        facingVec = playerColHandler.getLogicF();
        GeneraTramo(facingVec, new Vector3(0, 0, 0), false);
        //gM = FindObjectOfType<GameManager>(); //I know. Let me be.
        animeParticles.startColor = colorParticulasMinimo;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            float p = 1.0f;
            if (Input.GetKey(KeyCode.Space))
            {
                p = 5.0f;
            }

            player.transform.position += player.transform.forward * playerSpeed * p;
        }
    }

    //Para ser llamado por el jugador cuando entre a una interseccion
    public void EnteringIntersection(Intersection inter)
    {
        Debug.Log("enteringIntersection");
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerNextDir = Sentido.Derecha;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerNextDir = Sentido.Izquierda;
        }
        initMovement(playerNextDir, inter);
    }

    public void playerTurn(string direction)
    {
        playerDecision = true;
        if (direction == "Derecha")
        {
            playerNextDir = Sentido.Derecha;
            bocadillo.showBocadillo(Bocadillos.derecha);
        }
        else if (direction == "Izquierda")
        {
            playerNextDir = Sentido.Izquierda;
            bocadillo.showBocadillo(Bocadillos.izquierda);
        }
        else if (direction == "Recto")
        {
            playerNextDir = Sentido.Recto;
            bocadillo.showBocadillo(Bocadillos.recto);
        }
    }
}
