using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManger : MonoBehaviour
{
	public static MaterialManger instance = null;

	public Material[] arrayMateriales;

	public TextAsset archivoPaletas;

	private Color[][] Paletas;

	public int paletaActual = 0;

	int nPaletas;

	private int currentPallet = 0;

    private void Awake()
    {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
			Destroy(this.gameObject);
	}

    // Start is called before the first frame update
    void Start()
	{
		string[] lineasArchivo = archivoPaletas.text.Split('\n');

		nPaletas = int.Parse(lineasArchivo[0]);
		int nColores = int.Parse(lineasArchivo[1]);
		Paletas = new Color[nPaletas][];

		for (int i = 0; i < nPaletas; i++)
		{
			Paletas[i] = new Color[nColores];

			for (int j = 0; j < nColores; j++)
			{
				Paletas[i][j] = vectorFromString(lineasArchivo[2 + i * (nColores+1) + j + 1].Substring(5));
			}
		}

		currentPallet = Random.Range(0, 8); //Hardcodeado porque Unity es horrible y no hay tiempo
		setCurrentPallet(currentPallet);
	}

	public int getCurrentPallet()
    {
		return currentPallet;
    }

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < arrayMateriales.Length; i++)
		{
			arrayMateriales[i].color = Paletas[paletaActual%nPaletas][i];
		}
	}

	public Color vectorFromString(string s)
	{

		string[] sF = s.Split(',');

		Vector4 ret = new Vector4(float.Parse(sF[0]) / 255, float.Parse(sF[1]) / 255, float.Parse(sF[2]) / 255, 1.0f);

		return ret;
	}

	public void setCurrentPallet(int nPallet)
    {
		paletaActual = nPallet;
    }
}
