using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gataPariendoSonidos : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioPlayer;
    private AudioClip currentSound;
    private bool isCoRoutineReady = true;

    public float minTiempoEntreMaullidos = 2;
    public float maxTiempoEntreMaullidos = 5;

    public List<AudioClip> sonidos;
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoRoutineReady)
        {
            StartCoroutine(sonidosGata());
        }
    }

    public IEnumerator sonidosGata()
    {
        isCoRoutineReady = false;
        currentSound = sonidos[Random.Range(0, sonidos.Count - 1)];
        audioPlayer.clip = currentSound;
        audioPlayer.Play();

        yield return new WaitForSeconds(currentSound.length + Random.Range(minTiempoEntreMaullidos, maxTiempoEntreMaullidos));
        isCoRoutineReady = true;
    }
}
