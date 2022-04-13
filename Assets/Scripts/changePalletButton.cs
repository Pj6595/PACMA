using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePalletButton : MonoBehaviour
{
    private colorPalletSelector palletSelector;
    public int dir = -1;
    // Start is called before the first frame update
    void Start()
    {
        palletSelector = GetComponentInParent<colorPalletSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changePallet()
    {
        palletSelector.changePallet(dir);
    }
}
