using UnityEngine;
using UnityEditor;
using System.Collections;

public class SiphonCamera : MonoBehaviour {
	//base class for the various camera types

	public enum CameraType {STATIONARY, SWIVEL, FOLLOW, TRACKING};

	public bool isActive {get{return m_isActive;}}
	private bool m_isActive = false;
	public GameObject target = null;
	public bool debug = false;

	public CameraType type {get{return m_type;}}
	protected CameraType m_type;
	protected Transform m_lastTransform;

	// === unity functions ===
	void OnDrawGizmos(){
		if (!debug) return;

		switch (m_type){
		case CameraType.STATIONARY:
			OnStationaryDrawGizmos();
			break;
		case CameraType.FOLLOW:
			OnFollowDrawGizmos();
			break;
		case CameraType.SWIVEL:
			OnSwivelDrawGizmos();
			break;
		case CameraType.TRACKING:
			OnTrackingDrawGizmos();
			break;
		}

		DrawFrustumGizmos();
	}

	private void DrawFrustumGizmos(){
		if (Selection.activeObject != (Object) gameObject){
			Camera cam = GetComponent<Camera>();
			Gizmos.color = new Color(1f, 0f, 0f, .25f);
			Matrix4x4 temp = Gizmos.matrix;
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
			Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
			Gizmos.matrix = temp;
		}
	}

	protected virtual void OnStationaryDrawGizmos(){}
	protected virtual void OnFollowDrawGizmos(){}
	protected virtual void OnSwivelDrawGizmos(){}
	protected virtual void OnTrackingDrawGizmos(){}

	// === camera control functions ===
	public void SetTarget(GameObject lookTarget){
		target = lookTarget;
	}

	public void Activate(){
		if (m_lastTransform != null){
			transform.rotation = m_lastTransform.rotation;
			transform.position = m_lastTransform.position;
		}
		m_isActive = true;
		GetComponent<Camera>().enabled = true;
		GetComponent<AudioListener>().enabled = true;
		gameObject.tag = "MainCamera";
	}

	public void Deactivate(){
		m_lastTransform = transform;
		m_isActive = false;
		GetComponent<Camera>().enabled = false;
		GetComponent<AudioListener>().enabled = false;
		gameObject.tag = "Camera";
	}
}
