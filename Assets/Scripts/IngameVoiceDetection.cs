using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class IngameVoiceDetection : VoiceDetection
{
    public CityGenerator cityManager;
    public List<AudioClip> sonidosGata;
    public AudioSource soundPlayer;

    private AudioClip currentSound;

    protected override void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);

        cityManager.playerTurn(command,args.text);

        currentSound = sonidosGata[Random.Range(0, sonidosGata.Count - 1)];
        soundPlayer.clip = currentSound;
        soundPlayer.Play();
    }
}
