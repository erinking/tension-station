using UnityEngine;
using System.Collections;

public class FollowCamera : SiphonCamera {

	private Vector3 offset, originalPosition;
	public Vector3 axes;
	private Vector3 axesInverse;

	// === main functions ===
	void Start(){
		m_type = CameraType.FOLLOW;
		offset = transform.position - target.transform.position;
		originalPosition = transform.position;
		axes.Normalize ();
		axesInverse = (Vector3.one - axes).normalized;
	}

	void DelayedInit()
	{
	}

	void Update()
	{
		if (!isActive)
		{
			return;
		}

		Vector3 newPosition = Vector3.Lerp (transform.position, target.transform.position + offset, 0.1f);
		newPosition.y = originalPosition.y;
		newPosition.x = originalPosition.x;
		transform.position = newPosition;
	}

}
