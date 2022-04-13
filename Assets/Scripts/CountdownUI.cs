using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public List<Image> countdownNumbers;
    public List<Animation> countdownAnims;
    public CityGenerator cityGen;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        foreach (Image im in countdownNumbers) im.enabled = false;
        for (int i = 0; i < countdownNumbers.Count; i++)
        {
            countdownNumbers[i].enabled = true;
            countdownAnims[i].Play("NumberAnim");
            yield return new WaitForSeconds(1);
            countdownNumbers[i].enabled = false;
        }
        cityGen.StartMoving();
    }
}
