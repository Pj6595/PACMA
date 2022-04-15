using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesScript : MonoBehaviour
{
    public string Comando;
    // Start is called before the first frame update
    public void sendCommand()
    {
        GameManager.instance.SendCommand(Comando,"");
    }
}
