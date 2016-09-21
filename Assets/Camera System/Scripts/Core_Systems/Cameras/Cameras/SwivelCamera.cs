using UnityEngine;
using System.Collections;

public class SwivelCamera : SiphonCamera {
	//rotates on a local axis(axes) to follow the target

	// === inspector vars ===
	public bool rotateHorizontal;	//rotate y axis
	public bool rotateVertical;		//rotate x axis

	// === internal vars ===
	private Vector3 lookRot;
	private Vector3 originalRot;
	private Vector3 newEulerAngles; //so we aren't calling "new Vector3" every frame

	// === main functions ===
	void Start(){
		m_type = CameraType.SWIVEL;
		originalRot = transform.localEulerAngles;
		newEulerAngles = new Vector3();
	}

	void Update(){
		if (!isActive){
			return;
		}

		lookRot = target.transform.position - transform.position;
		Quaternion goalRot = Quaternion.LookRotation(lookRot, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, .5f);
		if (!rotateHorizontal){
			newEulerAngles = transform.localEulerAngles;
			newEulerAngles.y = originalRot.y;
			transform.localEulerAngles = newEulerAngles;
		}
		if (!rotateVertical){
			newEulerAngles = transform.localEulerAngles;
			newEulerAngles.x = originalRot.x;
			transform.localEulerAngles = newEulerAngles;
		}
	}

	// === helper functions ===

}


