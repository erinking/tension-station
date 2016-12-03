using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {
	// === inspector vars ===
	public float interactionDistance;
	public LayerMask interactionMask;
	private const int PLAYER_LAYER = 8;

	// === internal vars ===
	private bool debug;
	private SphereCollider interactColl;

	// === unity functions ===
	void Start () {
		debug = Application.isEditor;
		interactColl = GetComponent<SphereCollider> ();
	}

	void OnTriggerStay(Collider other)
	{
		if (PlayerInput.GetButtonDown ("Interact"))
		{
			if (other.gameObject.layer == PLAYER_LAYER)
			{
				InteractableComponent intComp = other.gameObject.GetComponent<InteractableComponent> ();
				if (intComp != null)
				{
					intComp.OnInteract ();
				}
			}
		}
	}
}
