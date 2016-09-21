using UnityEngine;
using System.Collections;

public class DestroyIfDuplicate : MonoBehaviour {

	void Start(){
		if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1){
			Destroy(gameObject);
		}
	}

}
