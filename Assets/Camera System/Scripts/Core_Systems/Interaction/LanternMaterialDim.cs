using UnityEngine;
using System.Collections;

public class LanternMaterialDim : MonoBehaviour {

	public Light controlLight;

	private Renderer rend;
	private Color litColor;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		litColor = rend.material.GetColor ("_EmissionColor");
	}

	// Update is called once per frame
	void Update () {
		Color col = Color.Lerp (rend.material.GetColor ("_EmissionColor"), controlLight.enabled ? litColor : Color.black, 0.2f);
		rend.material.SetColor ("_EmissionColor", col);
	}
}
