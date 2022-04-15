using Pacmetricas_G01;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class VoiceDetection : MonoBehaviour
{

    public string[] m_Keywords = new string[] { "izquierda", "derecha","recto","sigue" };

    public string command;

    protected KeywordRecognizer m_Recognizer;


    // Start is called before the first frame update
    void Start()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        if (m_Recognizer != null && m_Recognizer.IsRunning)
        {
            m_Recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_Recognizer.Stop();
        }
    }

    protected virtual void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);

        //Mandamos las palabras dichas si era para empezar
        if(command == "Start")
        {
            Tracker.GetInstance().TrackEvent(new PhraseMenuEvent(args.text));
        }

        GameManager.instance.SendCommand(command, args.text);
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected void OnApplicationQuit()
    {
        if(m_Recognizer != null && m_Recognizer.IsRunning)
        {
            m_Recognizer.OnPhraseRecognized -= OnPhraseRecognized;
            m_Recognizer.Stop();
        }
    }
}
