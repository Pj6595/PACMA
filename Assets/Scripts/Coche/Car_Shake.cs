using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Shake : MonoBehaviour
{
	private Quaternion originRotation;
	public float shake_decay = 0.002f;
	public float shake_intensity = .01f;

	public bool disableX = false;
	public bool disableY = false;
	public bool disableZ = false;

	public float rotationOffset = 6f;

	private float temp_shake_intensity = 0;
	void Start()
	{

	}

	void Update()
	{
		originRotation = transform.rotation;

		if (temp_shake_intensity > 0)
		{
			transform.position = transform.position + Random.insideUnitSphere * temp_shake_intensity;

			float rotX = originRotation.x;
			if (!disableX) rotX += Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f;

			float rotY = originRotation.y;
			if (!disableY) rotY += Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f;

			float rotZ = originRotation.z;
			if (!disableZ) rotZ += Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f;

			if (Mathf.Abs(rotX) > rotationOffset)
            {
				if (rotX < 0) rotX = -rotationOffset;
				else rotX = rotationOffset;
            }
			if (Mathf.Abs(rotY) > rotationOffset)
			{
				if (rotY < 0) rotY = -rotationOffset;
				else rotY = rotationOffset;
			}
			if (Mathf.Abs(rotZ) > rotationOffset)
			{
				if (rotZ < 0) rotZ = -rotationOffset;
				else rotZ = rotationOffset;
			}

			transform.rotation = new Quaternion(rotX, rotY, rotZ, originRotation.w);
			temp_shake_intensity -= shake_decay;
		}
		else temp_shake_intensity = shake_intensity;
	}
}
