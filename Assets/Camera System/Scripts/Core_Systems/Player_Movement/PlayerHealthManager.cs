using UnityEngine;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour {

	private float lifeTime;
	public const float MAX_LIFE_TIME = 2.0f;
	public Transform[] checkpoints;
	public bool[] actives;

	private bool recentDamage = false;
	private bool isDying = false;

	public void Start()
	{
		actives = new bool[checkpoints.Length];

		for (int i = 0; i < actives.Length; i++) 
		{
			actives [i] = false;
		}

		lifeTime = MAX_LIFE_TIME;
	}

	public void Update()
	{
		if (!recentDamage) 
		{
			lifeTime = Mathf.Min (lifeTime += Time.deltaTime, MAX_LIFE_TIME);
		}
	}

	public void TakeDamage()
	{
		lifeTime -= Time.deltaTime;

		if (!recentDamage) 
		{
			StartCoroutine ("DelayRegen");
		}

		if (!isDying && lifeTime <= 0f) 
		{
			isDying = true;
			StartCoroutine ("Die");
		}
	}

	private IEnumerator DelayRegen()
	{
		recentDamage = true;
		yield return new WaitForSeconds (3f);
		recentDamage = false;
	}

	private IEnumerator Die()
	{
		Transform targetSpawnPoint = checkpoints[0];

		for (int i = actives.Length - 1; i > 0; i--) 
		{
			if (actives[i]) 
			{
				targetSpawnPoint = checkpoints [i];
				break;
			}
		}

		GetComponent<PlayerMovementController> ().curState = PlayerMovementController.state.AUTO;
		GetComponent<Animator> ().SetBool ("rip", true);

		yield return new WaitForSeconds (3f);

		GetComponent<Animator> ().SetBool ("rip", false);
		GetComponent<PlayerMovementController> ().curState = PlayerMovementController.state.WALKING;
		transform.position = targetSpawnPoint.position;
		lifeTime = MAX_LIFE_TIME;
		recentDamage = false;
		isDying = false;
	}
}
