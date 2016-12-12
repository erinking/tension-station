using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public Camera endCam;

	void OnTriggerEnter()
	{
		endCam.GetComponent<FadeBehaviour> ().FadeOut ();
	}
}
