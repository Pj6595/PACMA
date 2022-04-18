using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsTeacher : MonoBehaviour
{
	public CanvasRenderer flechaDer;
	public CanvasRenderer flechaIz;
	public CanvasRenderer flechaRecto;

	public RectTransform BarraVolumen;
	public float minValueBarra = 0.1f;
	public float sensibilidadBarra = 20;
	public float volMaxmoNecesario = 0.95f;
	public CanvasRenderer[] numeros;

	public CanvasRenderer Tickverde;
	private int progreso = 0;
	private float volActual = 0;
	private bool dejaDeGritar = false;

	public string MainMenusceneName;
	public float timeToChangeScene = 1f;

	void Start()
	{
		flechaDer.SetAlpha(0);
		flechaIz.SetAlpha(0);
		flechaRecto.SetAlpha(0);
		BarraVolumen.anchorMin = new Vector2(BarraVolumen.anchorMin.x, minValueBarra);
		foreach (CanvasRenderer num in numeros)
		{
			num.SetAlpha(0);
		}
		Tickverde.SetAlpha(0);
	}

	void Update()
	{
		foreach (CanvasRenderer num in numeros)
		{
			num.SetAlpha(0);
		}
		numeros[progreso].SetAlpha(1);

		if (progreso >= 4)
		{
			Tickverde.SetAlpha(1);

			foreach (CanvasRenderer num in numeros)
			{
				num.SetAlpha(0);
			}
			numeros[4].SetAlpha(1);

			StartCoroutine(cambiarEscena(timeToChangeScene));
			Debug.Log("Cambiando Escena");
		}

		if (!dejaDeGritar) BarraVolumen.anchorMin = new Vector2(BarraVolumen.anchorMin.x, (volActual * sensibilidadBarra / (volMaxmoNecesario - minValueBarra)) + minValueBarra);

		if (!dejaDeGritar && volActual >= volMaxmoNecesario)
		{
			progreso++;
			dejaDeGritar = true; //por favor para ya
		}
	}

	IEnumerator cambiarEscena(float time)
	{
		yield return new WaitForSeconds(time);

		this.enabled = true;

		SceneManager.LoadSceneAsync(MainMenusceneName);
	}

	public void sendCommand(string command)
	{
		switch (command)
		{
			case "derecha":
				if (flechaDer.GetAlpha() < 1f) progreso++;
				flechaDer.SetAlpha(1);
				break;
			case "izquierda":
				if (flechaIz.GetAlpha() < 1f) progreso++;
				flechaIz.SetAlpha(1);
				break;
			case "recto":
				if (flechaRecto.GetAlpha() < 1f) progreso++;
				flechaRecto.SetAlpha(1);
				break;
			default:
				break;
		}
	}

	public void actualizaVolumen(float vol)
	{
		volActual = vol;
	}
}
