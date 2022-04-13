using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMainMenu : MonoBehaviour
{
    public string gameSceneName;

    public Transform checkpoint;

    public float speed;
    

    private AudioSource[] audioSources;
    private CarSound _carSound;
    
    private void Start()
    {
        this.enabled = false;
        audioSources = GetComponents<AudioSource>();
        _carSound = GetComponents<CarSound>()[0];
        _carSound.motorSoundPlayer = audioSources[1];
    }

    public void StartCar()
    {
        _carSound.StartEngine();
        audioSources[0].Play();
        StartCoroutine(StartMoving(1.5f));
    }
    
    IEnumerator StartMoving(float time)
    {
        yield return new WaitForSeconds(time);

        this.enabled = true;

       LeanTween.moveX(this.gameObject, checkpoint.position.x, speed)
           .setEaseInQuad()
           .setOnComplete(ChangeScene);
    }

    private void ChangeScene()
    {
        //GameManager.instance.LoadSceneAsync(gameSceneName); //aiuda esteban, no se por que esto no me va
        SceneManager.LoadSceneAsync(gameSceneName);
    }
}
