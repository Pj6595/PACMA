using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiaColor : MonoBehaviour
{

    public Material material;
    public VoiceVolumeDetection vol;
    public float minLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vol.micLoudness >= minLevel)
        {
            material.color = Color.red;
        }
        else
            material.color = Color.green;
    }
}
