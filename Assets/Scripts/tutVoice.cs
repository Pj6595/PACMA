using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class tutVoice : VoiceDetection
{
    public ControlsTeacher controlsTeacher;

    protected override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        controlsTeacher.sendCommand(command);
    }
}
