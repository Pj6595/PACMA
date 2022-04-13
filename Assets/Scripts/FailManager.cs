using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailManager : MonoBehaviour
{
    AudioSource[] audios;
    public float timeToExit = 7f;
    private float timePassed;
    void Start()
    {
        audios = GetComponents<AudioSource>();
        foreach (AudioSource a in audios)
            a.PlayDelayed(0.7f);
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= timeToExit)
            SceneManager.LoadSceneAsync("MenuPrincipal");
    }
}
