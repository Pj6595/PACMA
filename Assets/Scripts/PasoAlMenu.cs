using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasoAlMenu : MonoBehaviour
{
    public float retardo; //En segundos

    void Start()
    {
        //Debug.Log("Ding Ding ha llegado la hora");
        StartCoroutine(LoadLevelAfterDelay(retardo));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Debug.Log("Ding Ding ha llegado la hora");
        SceneManager.LoadScene("MenuPrincipal");
    }
}
