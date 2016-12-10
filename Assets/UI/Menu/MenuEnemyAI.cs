using UnityEngine;
using System.Collections;

public class MenuEnemyAI : MonoBehaviour {

	public float walkSpeed = 2f;
	public float chaseSpeed = 3f;
	public float chaseTimeout;
	public float[] patrolWaitTimes;
	public Transform[] waypoints;

	private EnemyVision enemyVision;
	private NavMeshAgent nav;
	private GameObject player;
	private PlayerHealthManager phm;
	private float endChaseTimer;
	private float patrolTimer;
	private int waypointIndex = 0;

	void Awake()
	{
		nav = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update()
	{
		Chase ();
	}

	void Chase()
	{
		nav.speed = chaseSpeed;
		nav.destination = player.transform.position;
	}
}
