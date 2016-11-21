using UnityEngine;
using System.Collections;

public class InteractableLight : InteractableComponent{

	public Light brightLight;
	public bool startState;

	void Start()
	{
		brightLight = GetComponent<Light> ();
		brightLight.enabled = startState;
	}

	public override void OnInteract()
	{
		brightLight.enabled = !brightLight.enabled;
	}
}
