using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource altavoz;
    public List<AudioClip> sound;
    AudioClip nowPlaying;
    void Start()
    {
        altavoz = GetComponent<AudioSource>();
        StartCoroutine(changeSongs());

    }

    IEnumerator changeSongs()
    {
        AudioClip clip;
        do
        {
            clip = sound[Random.Range(0, sound.Count)];
        } 
        while (clip == nowPlaying);
        nowPlaying = clip;
        float length = nowPlaying.length;
        altavoz.clip = nowPlaying;
        while (true)
        {
            altavoz.Play();
            yield return new WaitForSeconds(length);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
