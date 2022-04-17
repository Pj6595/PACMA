using Pacmetricas_G01;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class VoiceDetection : MonoBehaviour
{
    [Serializable]
    public struct Command
    {
        public string[] m_Keywords;
        public string command;
    }

    public Command[] commands;

    protected DictationRecognizer m_Dictation;

    public bool isInMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        StartDictation();

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene current)
    {
        StopDictation();
    }

    protected virtual void OnPhraseRecognized(string phrase, string command)
    {
        Debug.Log(phrase);       

        GameManager.instance.SendCommand(command, phrase);
    }

    void OnDictationResult(string text, ConfidenceLevel confidence)
    {
        bool found = false;
        foreach (Command c in commands)
        {
            if (Array.FindIndex(c.m_Keywords, x => x.ToLower() == text.ToLower()) != -1)
            {
                found = true;
                break;
            }
        }
        //si ningún comando lo reconoce, creamos un evento
        if (!found)
        {
            //if menu
            if (isInMenu)
                Tracker.GetInstance().TrackEvent(new PhraseMenuEvent(text));
            else
                Tracker.GetInstance().TrackEvent(new PhraseTaxiEvent(text));
            Debug.LogFormat("Dictation result: {0}", text);
        }
    }

    void OnDictationHypothesis(string text)
    {
        Debug.LogFormat("Dictation hypothesis: {0}", text);
        foreach (Command c in commands)
        {
            if (Array.FindIndex(c.m_Keywords, x => x.ToLower() == text.ToLower()) != -1)
            {
                OnPhraseRecognized(text, c.command);
                m_Dictation.Stop();
                m_Dictation.Start();
                break;
            }
        }
    }

    private void OnDictationComplete(DictationCompletionCause completionCause)
    {
        switch (completionCause)
        {
            case DictationCompletionCause.TimeoutExceeded:
            case DictationCompletionCause.PauseLimitExceeded:
            case DictationCompletionCause.Canceled:
            case DictationCompletionCause.Complete:
                // Restart required
                StopDictation();
                StartDictation();
                break;
            case DictationCompletionCause.UnknownError:
            case DictationCompletionCause.AudioQualityFailure:
            case DictationCompletionCause.MicrophoneUnavailable:
            case DictationCompletionCause.NetworkFailure:
                // Error
                StopDictation();
                break;
        }
    }
    private void OnDictationError(string error, int hresult)
    {
        //Debug.Log("Dictation error: " + error);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    protected void StartDictation()
    {
        m_Dictation = new DictationRecognizer();
        m_Dictation.DictationResult += OnDictationResult;
        m_Dictation.DictationHypothesis += OnDictationHypothesis;
        m_Dictation.DictationComplete += OnDictationComplete;
        m_Dictation.DictationError += OnDictationError;
        m_Dictation.Start();
    }

    protected void StopDictation()
    {
        if (m_Dictation != null && m_Dictation.Status == SpeechSystemStatus.Running)
        {
            m_Dictation.DictationHypothesis -= OnDictationHypothesis;
            m_Dictation.DictationResult -= OnDictationResult;
            m_Dictation.DictationError -= OnDictationError;
            m_Dictation.DictationComplete -= OnDictationComplete;
            m_Dictation.Stop();
            m_Dictation.Dispose();
        }
    }

    protected void OnApplicationQuit()
    {
        StopDictation();
    }
}
