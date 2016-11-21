using UnityEngine;
using System.Collections;

public class FollowCamera : SiphonCamera {
	// === main functions ===
	void Start(){
		m_type = CameraType.FOLLOW;
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

		//(target.transform.position + Vector3.up) - transform.position;
	}

}
