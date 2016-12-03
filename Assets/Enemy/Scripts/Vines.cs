using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vines : MonoBehaviour {

	public float spread = 30;
	public float hang = 10;
	public float startPosition = 1; //0 = start at anchor1, 1 = start at anchor2
	public Transform anchor1;
	public Transform anchor2;
	public Light target;
	public Light overrideTarget;
	public Color glowColor;

	const int loops = 20;
	const int subdivisions = 15;

	private Vector3[] hangingPoints = new Vector3[loops];
	private int[] loopOrder = new int[loops];
	private LineRenderer line;
	private Vector3[] points;
	private Vector3 center;
	private Material matInstance;

	// Use this for initialization
	void Start () {
		List<int> numbers = new List<int>();
		for (int i = 0; i < loops; i++) {
			numbers.Add(i);
		}
		for (int i = 0; i < loops; i++) {
			int index = Random.Range (0, loops - i);
			loopOrder [i] = numbers [index];
			numbers.RemoveAt (index);
		}
		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (loops * subdivisions + 1);
		points = new Vector3[loops * subdivisions + 1];
		for (int i = 0; i < points.Length; i++) {
			points [i] = transform.position;
		}
		Vector3 lineVector = (anchor2.position - anchor1.position).normalized;
		transform.rotation = Quaternion.LookRotation (lineVector);
		BoxCollider collider = GetComponent<BoxCollider> ();
		collider.size = new Vector3 (2, hang, spread);
		collider.center = new Vector3 (0, -hang / 2, 0);
		center = anchor1.position + lineVector * Mathf.Lerp (spread / 2, Vector3.Distance (anchor1.position, anchor2.position) - spread / 2, startPosition);
		matInstance = new Material (line.material);
		line.material = matInstance;
		//matInstance.SetTextureScale ("_Detail", new Vector2(10 * loops, 1));
	}
	
	// Update is called once per frame
	void Update () {
		//**** Movement code ****
		Vector3 lineVector = (anchor2.position - anchor1.position).normalized;
		Light currentTarget = (overrideTarget != null && overrideTarget.enabled) ? overrideTarget : target;
		if (currentTarget.enabled) {
			center = anchor1.position + Mathf.Clamp (Vector3.Dot (currentTarget.transform.position - anchor1.position, lineVector), spread * 0.5f, Vector3.Distance (anchor1.position, anchor2.position) - spread * 0.5f) * lineVector;
		}
		transform.position = center;
		//Calculate attachment point positions
		for (int i = 0; i < loops; i++) {
			hangingPoints [loopOrder[i]] = center + lineVector * (i / (loops - 1.0f) - 0.5f) * spread;
		}
		Vector3 normal = Vector3.Cross (Vector3.up, lineVector).normalized;
		for (int i = 0; i < loops; i++) {
			Vector3 prevPoint = hangingPoints [i];
			Vector3 nextPoint = hangingPoints [(i + 1) % loops];
			float prevLerp = Mathf.Clamp01 (Mathf.Sin (Time.time * 8 + i) * 0.1f);
			float nextLerp = Mathf.Clamp01 (Mathf.Sin (Time.time * 8 + (i + 1) % loops) * 0.1f);
			for (int j = 0; j < subdivisions; j++) {
				float lerp = j / (float)subdivisions;
				Vector3 point = Vector3.Lerp (prevPoint, nextPoint, lerp);
				point += normal * (i % 2 - 0.5f) * 0.3f;
				point = Vector3.Lerp(points[i * subdivisions + j], point, Mathf.Lerp(prevLerp,nextLerp,lerp));
				point.y = anchor1.position.y - (1 - Mathf.Pow (lerp * 2 - 1, 2)) *
					(hang - Vector3.Distance (prevPoint, nextPoint) * loops / spread * 0.1f + Mathf.Sin(Time.time + i));
				points [i * subdivisions + j] = point;
			}
		}
		points [subdivisions * loops] = points [0];
		line.SetPositions (points);

		//**** Light Changes ****
		matInstance.SetColor("_DetailColor",
			Color.Lerp (matInstance.GetColor("_DetailColor"), currentTarget.enabled ? glowColor : Color.black, 0.1f)
		);
		matInstance.SetTextureOffset ("_Detail", new Vector2(Time.time * 0.1f, 0));
	}
}
