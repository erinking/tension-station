using UnityEngine;
using System.Collections;

public class InteractableLight : InteractableComponent{

	public Light brightLight;

	void Start()
	{
		brightLight = GetComponent<Light> ();
		brightLight.enabled = false;
	}

	public override void OnInteract()
	{
		brightLight.enabled = !brightLight.enabled;
	}
}
