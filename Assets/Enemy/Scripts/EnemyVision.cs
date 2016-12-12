using UnityEngine;
using System.Collections;

public class EnemyVision : MonoBehaviour {

	public bool canSeePlayer;
	public float distToPlayer;
	public Vector3 lastSightingLoc;

	private GameObject player;
	private SphereCollider visionVolume;
	private NavMeshAgent nav;
	public PlayerMovementController playerMov;
	[HideInInspector]
	public Vector3 resetLoc;

	void Awake()
	{
		resetLoc = new Vector3 (999f, 999f, 999f);

		nav = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		visionVolume = GetComponent<SphereCollider> ();
		playerMov = player.GetComponent<PlayerMovementController> ();
		lastSightingLoc = resetLoc;
	}

	void Update()
	{
		distToPlayer = Vector3.Magnitude (transform.position - player.transform.position);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == player) 
		{
			canSeePlayer = false;

			if (distToPlayer < 1) 
			{
				canSeePlayer = true;
			}

			Vector3 dir = other.transform.position - transform.position;

			RaycastHit hitInfo;
			float visibleDistance = playerMov.flashlight.enabled ? visionVolume.radius : visionVolume.radius / 3.5f;

			if (Physics.Raycast (transform.position + Vector3.up, dir, out hitInfo, visibleDistance)) 
			{
				if (hitInfo.collider.gameObject == player) 
				{
					canSeePlayer = true;
					lastSightingLoc = player.transform.position;
				}
			}

		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == player)
		{
			canSeePlayer = false;
		}
	}

	float CalculatePathLength(Vector3 targetPosition)
	{
		NavMeshPath path = new NavMeshPath ();
		if (nav.enabled)
		{
			nav.CalculatePath (targetPosition, path);
		}

		Vector3[] allWaypoints = new Vector3[path.corners.Length + 2];

		allWaypoints [0] = transform.position;
		allWaypoints [allWaypoints.Length - 1] = targetPosition;

		for (int i = 0; i < path.corners.Length; i++)
		{
			allWaypoints [i + 1] = path.corners [i];
		}

		float pathLength = 0;

		for (int i = 0; i < allWaypoints.Length - 1; i++)
		{
			pathLength += Vector3.Distance (allWaypoints [i], allWaypoints [i + 1]);
		}

		return pathLength;
	}
}
