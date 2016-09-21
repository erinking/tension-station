using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {

	public Camera cam;
	public GameObject cameraTarget;
	public bool debug;

	// Use this for initialization
	void Start () {
		if (cam == null){
			throw new UnityException("Camera trigger set up without a connecting camera defined.");
		}
		if (cameraTarget == null){
			cameraTarget = GameObject.FindWithTag("Player");
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Player")){
			CameraManager.Get().SwitchCameras(cam, cameraTarget);
		}
	}

	void OnDrawGizmos(){
		if (!debug) return;

		BoxCollider coll = GetComponent<BoxCollider>();
		Gizmos.color = new Color(1f, .5f, 0f, .5f);
		Gizmos.DrawWireCube(transform.position + coll.center, coll.size);
		Gizmos.DrawLine(transform.position, cam.transform.position);
	}
}
