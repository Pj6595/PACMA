using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class ShaderEstresScript : MonoBehaviour
{
	private PostProcessVolume volumenPostProcess;
	public float vigIntensity = 1.0f;
	Vignette vig;


	// Start is called before the first frame update
	void Start()
	{
		volumenPostProcess = GetComponent<PostProcessVolume>();

		if (volumenPostProcess.profile.TryGetSettings<Vignette>(out var vigtemp))
		{
			vig = vigtemp;
			vig.intensity.overrideState = true;			
		}
	}

	// Update is called once per frame
	void Update()
	{
		vig.intensity.value = vigIntensity;
	}

	public void updateIntensityVignete(float inte)
	{
		vigIntensity = inte;
	}
}
