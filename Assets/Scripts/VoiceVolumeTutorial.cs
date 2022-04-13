using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceVolumeTutorial : MonoBehaviour
{
	public float micLoudness;
	public ControlsTeacher controlsTeach;

	private AudioClip clipRecord = null;
	int sampleWindow = 128; //Numero de samples que se restan
	private float stressDifference;

	float LevelMax()
	{
		float levelMax = 0;

		float[] waveData = new float[sampleWindow];

		int micPosition = Microphone.GetPosition(null) - (sampleWindow + 1);
		if (micPosition < 0) return 0;

		clipRecord.GetData(waveData, micPosition);
		//Get peak on the last 128 samples
		for (int i = 0; i < sampleWindow; i++)
		{
			float wavePeak = waveData[i] * waveData[i];
			if (levelMax < wavePeak)
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
	}

	void StopMicrophone()
	{
		Microphone.End(null);
	}

	// Update is called once per frame
	void Update()
	{
		micLoudness = LevelMax();

		controlsTeach.actualizaVolumen(micLoudness);
		//stress.UpdateEstres(-StressIncrement()*Time.deltaTime);

	}

}
