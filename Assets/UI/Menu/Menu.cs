using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public Transform[] menuTargets;
	public Transform aCircle;
	public Transform light;

	private int currentTarget = 0;
	private float timeBuffer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float vert = Input.GetAxis ("Vertical") + Input.GetAxis ("VerticalController");
		if (vert == 0) {
			timeBuffer = 0;
		}
		if (timeBuffer > 0) {
			timeBuffer -= Time.deltaTime;
		} else {
			if (vert != 0) {
				timeBuffer = 0.2f;
				currentTarget += vert < 0 ? 1 : menuTargets.Length - 1;
				currentTarget = currentTarget % menuTargets.Length;
				menuUpdate ();
			}
		}
		if (Input.GetButtonDown ("Interact")) {
			menuAction ();
		}
	}

	void menuUpdate(){
		aCircle.position = menuTargets [currentTarget].position;
		light.position = aCircle.position + new Vector3 (-3, 1, 0);
	}

	void menuAction(){
		switch (currentTarget) {
		case 0:
			Application.LoadLevel ("Intro");
			break;
		case 1:
			Application.LoadLevel ("Credits");
			break;
		case 2:
			Application.Quit ();
			break;
		}
	}
}