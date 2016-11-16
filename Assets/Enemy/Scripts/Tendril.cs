using UnityEngine;
using System.Collections;

public class Tendril : MonoBehaviour {

	const float segmentLength = 0.25f;
	const float maxDegrees = 10;

	public Transform root;
	public MonsterMove monster;

	private Vector3 offset;
	private bool leading = false;
	private LineRenderer line;
	private Vector3[] points;
	private Vector3 oldPos;
	private Vector3 vel = Vector3.zero;
	private Vector3 startAngle;
	private int seed;
	private int pointcount;
	private Vector3 xVector;
	private Vector3 yVector;

	public void Randomize () {
		Vector3 upVector = Vector3.up;
		xVector = Vector3.Cross(Random.insideUnitSphere,upVector).normalized;
		yVector = Vector3.Cross(xVector,upVector);
		seed = Random.Range(int.MinValue,int.MaxValue);
		pointcount = Random.Range (5, 20);
		points = new Vector3[pointcount];
		offset = Random.onUnitSphere;
		offset = Vector3.Dot (xVector, offset) * xVector + Vector3.Dot (yVector, offset) * yVector;
		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (pointcount);
		line.SetWidth (pointcount / 80f, 0);
		oldPos = root.position;
		offset = offset.normalized * pointcount * segmentLength * 0.5f;
		startAngle = (offset.normalized + (Vector3)Random.insideUnitCircle).normalized;
		Cursor.visible = false;
	}
		
	void Update() {
		Vector3 upVector = Vector3.up;
		if (root.position != oldPos) {
			vel = (root.position - oldPos).normalized;
			leading = Vector3.Angle (vel, offset) <= 90;
		}
		if (leading) {
			Vector3 end = points [pointcount - 1];
			Vector3 dir = root.position + offset - end + GetRand (Vector3.Dot(root.position + offset, xVector),Vector3.Dot(root.position + offset, yVector), 0.5f) * 4 + vel * 2;
			points [pointcount - 1] += dir.normalized * Mathf.Clamp01(dir.magnitude) * Time.deltaTime * Mathf.Max((100 - Vector3.Angle (vel, offset)),0) * 0.2f;
			for (int i = pointcount-2; i >= 0; i--) {
				points [i] = (points [i] - points [i + 1]).normalized * segmentLength + points [i + 1];
			}
		}
		for (int i = 1; i < pointcount; i++) {
			RaycastHit hit = new RaycastHit();
			Vector3 origin = root.position + upVector * 4;
			if (Physics.Raycast (origin, points[i] - origin, out hit)) {
				points [i] += Vector3.ClampMagnitude(hit.point + hit.normal * 0.1f - points[i],Time.deltaTime * 5);
			}
		}

		/*for (int i = 1; i < pointcount; i++) {
			float distFromFloor = Vector3.Dot (points [i], upVector);
			if (distFromFloor < 0) {
				points [i].y = 0;
			}
		}*/

		Vector3 angleOffset = GetRand (Vector3.Dot(root.position, xVector),Vector3.Dot(root.position, yVector), 0.5f);
		Vector3 prevDir = startAngle + angleOffset * Vector3.Dot(startAngle,angleOffset) * 4;
		points [0] = root.position;
		for (int i = 1; i < pointcount; i++) {
			Vector3 dir = points [i] - points [i - 1];
			float angle = Vector3.Angle (dir, prevDir);
			float localMaxDegrees = Mathf.Lerp (maxDegrees, 50, i / (float)pointcount);
			if (angle > localMaxDegrees) {
				dir = Vector3.RotateTowards (dir, prevDir, (angle - localMaxDegrees) * Mathf.Deg2Rad, 0);
			}
			points [i] = dir.normalized * segmentLength + points [i - 1];
			prevDir = dir;
		}
		line.SetPositions (points);
		oldPos = root.position;
	}

	public static Vector3 GetRand(Vector3 pos,float perlinScale,int seed){
		return new Vector3 (Mathf.PerlinNoise (pos.x * perlinScale + seed, pos.y * perlinScale) * 2 - 1,
			Mathf.PerlinNoise (pos.x * perlinScale, pos.y * perlinScale + seed) * 2 - 1, 0);
	}

	public Vector3 GetRand(float x, float y,float perlinScale){
		return (Mathf.PerlinNoise (x * perlinScale + seed, y * perlinScale) * 2 - 1) * xVector +
			(Mathf.PerlinNoise (x * perlinScale, y * perlinScale + seed) * 2 - 1) * yVector;
	}
}
