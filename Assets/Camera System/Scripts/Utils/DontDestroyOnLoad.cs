using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

	//shortcut to also adding the DestroyIfDuplicate component, since it is common to want that functionality when adding this component
	public bool destroyIfDuplicate = false;

	void Start(){
		if (destroyIfDuplicate && GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1){
			Destroy(gameObject);
		}

		foreach (Transform trans in GetComponentsInChildren<Transform>(true)){
			DontDestroyOnLoad(trans.gameObject);
		}
	}
}
