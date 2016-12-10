using UnityEngine;
using System.Collections;

public class LightMove : MonoBehaviour {

	Plane plane = new Plane(Vector3.up, new Vector3(0,1,0));

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float rayDistance;
		if (plane.Raycast(ray, out rayDistance)){
			transform.position = ray.GetPoint(rayDistance);
		}
	}
}
