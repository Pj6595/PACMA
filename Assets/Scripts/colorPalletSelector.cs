using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorPalletSelector : MonoBehaviour
{
    public List<Sprite> nombresEmojificados;

    private int currentPallet = 0;
    private int nPallets;
    public Image imagenNombres;

    // Start is called before the first frame update
    void Start()
    {
        nPallets = nombresEmojificados.Count - 1;
        currentPallet = MaterialManger.instance.getCurrentPallet();

        imagenNombres.sprite = nombresEmojificados[currentPallet];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePallet(int dir) //Pa alante o pa atrá
    {
        currentPallet += dir;
        if (currentPallet < 0) currentPallet = nPallets;
        else if (currentPallet > nPallets) currentPallet = 0;
        Debug.Log(currentPallet);
        MaterialManger.instance.setCurrentPallet(currentPallet);
        imagenNombres.sprite = nombresEmojificados[currentPallet];
    }
}
