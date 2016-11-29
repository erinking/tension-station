using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackingCamera : SiphonCamera {
	//follows the target along a defined path
	
	// === constants ===
	private const float GIZMO_STEPS = 100f;
	private Color GIZMO_COLOR = Color.red;
	private float T_INCREMENT = 0.01f;
	private float IMPROVEMENT_EPSILON = .1f;	//at what distance of target from the viewport center do we consider it worth altering T
	private float VALID_IMPROVEMENT_THRESHOLD = .25f;	//at what dot value of movement do we consider it good enough to make the move

	// === inspector vars ===
	public Transform[] splinePoints;	//must have a length that is a multiple of (3n+1) not including 1
	
	// === internal vars ===
	private List<BezierNode> m_nodes;
	private float curT, desiredT;
	private Camera cam;
	
	// === main functions ===
	void Start(){
		m_type = CameraType.TRACKING;
		SetupNodes();
		cam = GetComponent<Camera>();
		transform.position = splinePoints[0].position;
		curT = 0f;
		desiredT = 0f;
	}
	
	void Update(){
		if (!isActive){
			return;
		}

		curT = Mathf.Clamp(curT, 0.001f, .999f);
		Vector3 curPos = GetBezierPos(curT);
		Vector3 targetVecFromCenter = cam.WorldToViewportPoint(target.transform.position);
		targetVecFromCenter.z = 0f;
		targetVecFromCenter -= new Vector3(.5f, .5f);

		//see if going forward or backward would appreciably improve the shot
		if (Vector3.SqrMagnitude(targetVecFromCenter) > Mathf.Pow(IMPROVEMENT_EPSILON, 2)){
			Vector3 possibleForwardTVec = GetBezierPos(Mathf.Min(1f, curT + T_INCREMENT)) - curPos;
			Vector3 possibleBackTVec = GetBezierPos(Mathf.Max(0f, curT - T_INCREMENT)) -  curPos;
			float improvementDotForward = Vector3.Dot(possibleForwardTVec.normalized, targetVecFromCenter.normalized);
			float improvementDotBack = Vector3.Dot(possibleBackTVec.normalized, targetVecFromCenter.normalized);
			if (improvementDotForward >= VALID_IMPROVEMENT_THRESHOLD || improvementDotBack >= VALID_IMPROVEMENT_THRESHOLD){
				desiredT = improvementDotForward > improvementDotBack ? curT+T_INCREMENT : curT-T_INCREMENT;
			}
		}

		if (Mathf.Abs(curT-desiredT) > T_INCREMENT/2f){
			curT = Mathf.Lerp(curT, desiredT, .5f);
			transform.position = GetBezierPos(curT);
		}
	}
	
	protected override void OnTrackingDrawGizmos(){
		SetupNodes();

		Gizmos.color = GIZMO_COLOR;
		float t;
		Vector3 lastPos = splinePoints[0].position;
		Vector3 curPos;
		for (int count=1; count<GIZMO_STEPS+1; count++){
			t = count/GIZMO_STEPS;
			//pos lines
			curPos = GetBezierPos(t);
			Gizmos.DrawLine(lastPos, curPos);
			lastPos = curPos;
		}
		foreach (Transform trans in splinePoints){
			Gizmos.DrawSphere(trans.position, .5f);
		}
	}

	// === spline calculations ===
	internal class BezierNode {
		internal Transform[] points;
		internal float timeFraction;

		internal BezierNode(Transform[] points, float timeFraction){
			this.points = new Transform[4];
			for (int i=0; i<4; i++){
				this.points[i] = points[i];
			}
			this.timeFraction = timeFraction;
		}
	}

	private Vector3 GetBezierPos(float t){
		BezierNode node = GetRelevantNode(t);
		t = AdjustTForWindow(t);

		float basis0 = Mathf.Pow((1f-t), 3);
		float basis1 = 3f * Mathf.Pow((1f-t), 2) * t;
		float basis2 = 3f * (1f-t) * Mathf.Pow(t, 2);
		float basis3 = Mathf.Pow(t, 3);

		return node.points[0].position*basis0 + node.points[1].position*basis1 + node.points[2].position*basis2 + node.points[3].position*basis3;
	}

	// === helper functions ===
	private void SetupNodes(){
		m_nodes = new List<BezierNode>();
		if (splinePoints.Length < 4){
			throw new UnityException("invalid spline points list length. Must be >= 4");
		}
		Transform[] temp = new Transform[4];
		float totalCurves = (splinePoints.Length-1)/3f;
		for (int j=0; j<totalCurves; j++){
			for (int i=0; i<4; i++){
				temp[i] = splinePoints[3*j+i];
			}
			m_nodes.Add(new BezierNode(temp, j/totalCurves));
		}
	}
	
	private BezierNode GetRelevantNode(float t){
		int firstIndex = 0;
		while (firstIndex < m_nodes.Count && m_nodes[firstIndex].timeFraction <= t){
			firstIndex++;
		}
		firstIndex--;
		return m_nodes[firstIndex];
	}
	
	private float AdjustTForWindow(float t){
		//dealing with the final step so it doesn't try to wrap back around to the beginning of the last bezier node
		if (t == 1f){
			t -= 0.001f;
		}
		t %= 1f/m_nodes.Count;
		t *= m_nodes.Count;
		return t;
	}
}
