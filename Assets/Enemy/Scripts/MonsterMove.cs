using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {

	public float speed;
	public Transform targetObj;
	public GameObject tendrilPrefab;
	public Light lantern;
	public NavMeshAgent agent;

	private Vector3 targetPosition;

	void Start(){
		agent = GetComponent<NavMeshAgent> ();
		for (int i = 0; i < 16; i++) {
			GameObject tendril = Instantiate (tendrilPrefab, transform.position, transform.rotation) as GameObject;
			Tendril script = tendril.GetComponent<Tendril> ();
			script.root = transform;
			script.monster = this;
			script.Randomize ();
		}
	}

	// Update is called once per frame
	void Update () {
		/*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float rayDistance;
		if (groundPlane.Raycast(ray, out rayDistance))
			targetPosition = ray.GetPoint(rayDistance);
		targetObj.position = targetPosition;*/

		/*
		if (lantern.enabled) {
			targetPosition = targetObj.position - groundPlane.normal * groundPlane.GetDistanceToPoint (targetObj.position);
		}
		transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * (Tendril.GetRand(transform.position,0.5f,100).magnitude + 0.4f) * speed);
		*/
	}
}
