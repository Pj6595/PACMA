using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarSound : MonoBehaviour
{
    public AudioSource motorSoundPlayer;
    public AudioSource girosSoundPlayer;

    public List<AudioClip> listGiroSounds;
    private AudioClip currentGiroSound;

    [Range(0.0f,1.0f)]
    public float maxVolume;
    public float pitchVariance;
    [Range(0.0f,1.0f)]
    public float startingSpeed;

    [Range(0.0f,1.5f)]
    public float minPitch;
    [Range(1.5f,3.0f)]
    public float maxPitch;

    public float targetPitch;

    public bool playOnStart;
    
    private bool _starting;
    

    private void Start()
    {
        if(playOnStart)StartEngine();
    }

    private void Update()
    {
        if (_starting)
        {
            motorSoundPlayer.volume = LeanSmooth.linear(motorSoundPlayer.volume, maxVolume, startingSpeed);
            if (motorSoundPlayer.volume >= maxVolume) _starting = false;
        }
        
        //variamos de manera aleatoria el pitch para que no se siempre constante
        motorSoundPlayer.pitch += Random.Range(-pitchVariance, pitchVariance);

        motorSoundPlayer.pitch = LeanSmooth.linear(motorSoundPlayer.pitch, targetPitch, startingSpeed/10);
        if(motorSoundPlayer.pitch == targetPitch) targetPitch = Random.Range(minPitch, maxPitch);
    }

    public void StartEngine()
    {
        _starting = true;
        motorSoundPlayer.Play();
        motorSoundPlayer.volume = 0;
        motorSoundPlayer.pitch = 1f;
        targetPitch = Random.Range(minPitch, maxPitch);
    }

    public void giroSound()
    {
        currentGiroSound = listGiroSounds[Random.Range(0, listGiroSounds.Count)];
        girosSoundPlayer.clip = currentGiroSound;
        girosSoundPlayer.Play();
    }
}
