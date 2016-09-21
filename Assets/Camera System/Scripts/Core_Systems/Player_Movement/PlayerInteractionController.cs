using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {
	// === inspector vars ===
	public float interactionDistance;
	public LayerMask interactionMask;

	// === internal vars ===
	private bool debug;

	// === unity functions ===
	void Start () {
		debug = Application.isEditor;
	}

	void Update () {
		if (PlayerInput.GetButtonDown("Interact")){
			if (debug){
				Debug.DrawRay(transform.position, transform.forward*interactionDistance, Color.green);
			}
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionMask)){
				InteractableComponent intComp = hit.collider.gameObject.GetComponent<InteractableComponent>();
				if (intComp != null){
					intComp.OnInteract();
				}
			}
		}
	}
}
