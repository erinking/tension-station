using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

	public GameObject uiElement;
	private Collider collider;

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider> ();
	}
	
	void OnTriggerEnter(Collider other){
		if (other.name == "player") {
			uiElement.SetActive (true);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.name == "player") {
			uiElement.SetActive (false);
		}
	}
}
