using UnityEngine;
using System.Collections;

public class MonsterMove : MonoBehaviour {

	public Transform targetObj;
	public GameObject tendrilPrefab;
	public Light lantern;
	public EnemyVision vision;
	public Color glowColor;
	public Material tendrilMaterial;

	private Material tendrilMaterialInstance;

	void Start(){
		tendrilMaterialInstance = new Material (tendrilMaterial);
		for (int i = 0; i < 16; i++) {
			GameObject tendril = Instantiate (tendrilPrefab, transform.position, transform.rotation) as GameObject;
			tendril.GetComponent<Renderer> ().material = tendrilMaterialInstance;
			Tendril script = tendril.GetComponent<Tendril> ();
			script.root = transform;
			script.monster = this;
			script.Randomize ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (vision == null) {
			tendrilMaterialInstance.SetColor ("_DetailColor", glowColor);
			return;
		}
		tendrilMaterialInstance.SetColor("_DetailColor",
			Color.Lerp (tendrilMaterialInstance.GetColor("_DetailColor"), vision.canSeePlayer ? glowColor : Color.black, 0.1f)
		);
	}
}
