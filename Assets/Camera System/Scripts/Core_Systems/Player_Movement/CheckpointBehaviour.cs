using UnityEngine;
using System.Collections;

public class CheckpointBehaviour : MonoBehaviour {

	public int checkpointIndex;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerHealthManager> ().actives[checkpointIndex] = true;
		}
	}
}
