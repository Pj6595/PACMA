using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class tutVoice : VoiceDetection
{
    public ControlsTeacher controlsTeacher;

    protected override void OnPhraseRecognized(string phrase, string command)
    {
        Debug.Log(phrase);
        controlsTeacher.sendCommand(command);
    }
}
