using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	public float walkSpeed = 2f;
	public float chaseSpeed = 3f;
	public float chaseTimeout;
	public float[] patrolWaitTimes;
	public Transform[] waypoints;
	public float slowDist = 3f;

	private EnemyVision enemyVision;
	public NavMeshAgent nav;
	private GameObject player;
	private PlayerHealthManager phm;
	private PlayerMovementController playerMov;
	private float endChaseTimer;
	private float patrolTimer;
	private int waypointIndex = 0;
	private AudioSource myAudioSrc;

	void Awake()
	{
		enemyVision = GetComponent<EnemyVision> ();
		nav = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMov = player.GetComponent<PlayerMovementController> ();
		phm = player.GetComponent<PlayerHealthManager> ();
		myAudioSrc = GetComponent<AudioSource> ();
	}

	void Update()
	{
		if (enemyVision.canSeePlayer || enemyVision.lastSightingLoc != enemyVision.resetLoc)
		{
			Chase ();
		} 
		else
		{
			Patrol ();
		}

		if (enemyVision.distToPlayer > slowDist) 
		{
			playerMov.curSpeed = playerMov.speed;
		}
	}

	void Chase()
	{
		myAudioSrc.volume = 1.0f;

		Vector3 vecToPlayer = player.transform.position - transform.position;

		nav.speed = chaseSpeed;

		nav.destination = enemyVision.lastSightingLoc;

		if (nav.remainingDistance <= nav.stoppingDistance)
		{
			endChaseTimer += Time.deltaTime;

			if (endChaseTimer > chaseTimeout)
			{
				enemyVision.lastSightingLoc = enemyVision.resetLoc;
				endChaseTimer = 0f;
			}
		}

		if (enemyVision.distToPlayer < slowDist) 
		{
			playerMov.curSpeed = playerMov.speed * (enemyVision.distToPlayer / slowDist);	
		} 

		if (Vector3.SqrMagnitude (vecToPlayer) < 2 && enemyVision.canSeePlayer)
		{
			phm.TakeDamage ();
		}
	}

	void Patrol()
	{
		myAudioSrc.volume = 0.3f;
		nav.speed = walkSpeed;
		if (nav.destination == enemyVision.resetLoc || nav.remainingDistance < nav.stoppingDistance)
		{
			patrolTimer += Time.deltaTime;

			if (patrolTimer >= patrolWaitTimes [waypointIndex])
			{
				if (waypointIndex == waypoints.Length - 1)
				{
					waypointIndex = 0;
				} 
				else
				{
					waypointIndex++;
				}

				patrolTimer = 0;
			} 
		}
		else
		{
			patrolTimer = 0;
		}

		nav.destination = waypoints [waypointIndex].position;
	}
}
