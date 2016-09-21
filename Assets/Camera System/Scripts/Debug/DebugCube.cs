using UnityEngine;
using System.Collections;

public class DebugCube : MonoBehaviour {

	public float speed;

	private float inputHorizontal, inputVertical;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		inputHorizontal = Input.GetAxisRaw("Horizontal");
		inputVertical = Input.GetAxisRaw("Vertical");
		Vector3 movement = new Vector3(inputHorizontal, 0f, inputVertical);
		movement.Normalize();
		movement *= speed;
		movement = transform.TransformDirection(movement);
		transform.Translate(movement);
	}
}
