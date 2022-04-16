using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pacmetricas_G01;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public CarMainMenu car;

    private SetSignImg papel;

    private int record;

    private int currentPoints = 0;

    public int pointsToWin = 3;

    private Text pointsText = null;
    private Animation pointsAnim;

    public AudioClip pointUpAudio;
    public AudioClip pointDownAudio;
    private AudioSource audioSource;

    [SerializeField]
    private List<Configuration> persistenceConfiguration;

    int hospitalDestino = 0;
    Color colorDestino;

    public int GetHospitalDestino() { return hospitalDestino; }
    public Color GetColorHospitalDestino() { return colorDestino; }

    public void SetPapel(SetSignImg papel) {
        this.papel = papel;
        hospitalDestino = Random.Range(0, SetSignImg.GetNumHospitales());
        colorDestino = SetSignImg.GetRandomColor();
        papel.SetImg(hospitalDestino, colorDestino);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("record")) record = PlayerPrefs.GetInt("record");
        else record = 0;

        Tracker.GetInstance().Init(persistenceConfiguration);
        Tracker.GetInstance().TrackEvent(new InitGameEvent());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) updateRecord(1);
    }

    public void updateRecord(int p)
    {
        record += p;
    }

    public void SendCommand(string command, string phraseDetected)
    {
        if (command == "Start")
        {
            Debug.Log("Vamo a jugal ... cuando llegue el coche");
            Tracker.GetInstance().TrackEvent(new MenuPassedEvent());
            car.StartCar();

        }
        else if (command == "Menu") {
            StartCoroutine(LoadSceneAsync("MenuPrincipal"));
        }
        else if (command == "Tutorial")
        {
            StartCoroutine(LoadSceneAsync("MenuTutorial"));
        }
        else if (command == "Introduccion")
        {
            StartCoroutine(LoadSceneAsync("Introduccion"));
        }
        else if (command == "Exit")
        {
            Tracker.GetInstance().TrackEvent(new PhraseMenuEvent(phraseDetected));
            Tracker.GetInstance().End(); //Cerramos el tracker
            Debug.Log("Saliendo del juego");
            Application.Quit();
        }
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("record", record);
    }

    public float GetPoints()
    {
        return currentPoints;
    }

    public float GetPointsForWin()
    {
        return pointsToWin;
    }

    public void AddPoint()
    {
        currentPoints += 1;
        pointsText.text = currentPoints.ToString("000");
        pointsAnim.Play("PointUpAnimation");
        audioSource.clip = pointUpAudio;
        audioSource.Play();
    }

    public void RemovePoint()
    {
        if (currentPoints > 0) currentPoints -= 1;
        pointsText.text = currentPoints.ToString("000");
        pointsAnim.Play("PointDownAnimation");
        audioSource.clip = pointDownAudio;
        audioSource.Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuPrincipal")
            car = GameObject.FindWithTag("FordfiestaInicio").GetComponent<CarMainMenu>();
        if (scene.name == "Juego")
        {
            currentPoints = 0;
            pointsText = GameObject.FindWithTag("TextoPuntuacion").GetComponent<Text>();
            pointsAnim = pointsText.gameObject.GetComponent<Animation>();
            audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        }
    }
}
