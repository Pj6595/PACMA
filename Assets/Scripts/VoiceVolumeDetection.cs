using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pacmetricas_G01;

public class VoiceVolumeDetection : MonoBehaviour
{
    public float micLoudness;
    public estres stress;

    [Range(0.0f,1.0f)]
    public float volToRecoverStress;
    public float minStressIncrement;
    public float maxStressIncrement;
    public float eventTime = 5f;

    private float time = 0f;
    private AudioClip clipRecord = null;
    int sampleWindow = 128; //Numero de samples que se restan
    private float stressDifference;

    float LevelMax()
    {
        float levelMax = 0;

        float[] waveData = new float[sampleWindow];

        int micPosition = Microphone.GetPosition(null) - (sampleWindow+1);
        if (micPosition < 0) return 0;

        clipRecord.GetData(waveData, micPosition);
        //Get peak on the last 128 samples
        for(int i = 0; i < sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if(levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        return levelMax;
    }


    // Start is called before the first frame update
    void Start()
    {
        //foreach(var device in Microphone.devices)
        //{
        //    Debug.Log("Name " + device);
        //}

        clipRecord = Microphone.Start(null, true, 10, 44100);
        Debug.Log(clipRecord);
        stressDifference = maxStressIncrement - minStressIncrement;
    }

    void StopMicrophone()
    {
        Microphone.End(null);
    }

    // Update is called once per frame
    void Update()
    {
        micLoudness = LevelMax();

        if (micLoudness >= volToRecoverStress)
        {
            stress.UpdateEstres(-StressIncrement()*Time.deltaTime);
        }

        //Mandamos el volumen del micro si es tiempo para hacerlo
        time += Time.deltaTime;
        if (time >= eventTime)
        {
            Tracker.GetInstance().TrackEvent(new MicrophoneVolume(micLoudness));
            time = 0f;
        }
    }

    float StressIncrement()
    {
        float interpolation = (micLoudness-volToRecoverStress)/(1-volToRecoverStress);
        return minStressIncrement + (stressDifference * interpolation);
    }
}
