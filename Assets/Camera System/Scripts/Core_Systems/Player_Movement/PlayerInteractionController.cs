using UnityEngine;
using System.Collections;

public class PlayerInteractionController : MonoBehaviour {
	// === inspector vars ===
	public float interactionDistance;
	public LayerMask interactionMask;
	private const int PLAYER_LAYER = 8;
	private InteractableComponent curInteractable;

	// === internal vars ===
	private bool debug;
	private SphereCollider interactColl;

	// === unity functions ===
	void Start () {
		debug = Application.isEditor;
		interactColl = GetComponentInChildren<SphereCollider> ();
		curInteractable = null;
	}

	void Update ()
	{
		if (PlayerInput.GetButtonDown ("Interact"))
		{
			if (curInteractable != null)
			{
				curInteractable.OnInteract ();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.layer == PLAYER_LAYER)
		{
			InteractableComponent intComp = other.gameObject.GetComponent<InteractableComponent> ();
			if (intComp != null)
			{
				curInteractable = intComp;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == PLAYER_LAYER)
		{
			curInteractable = null;
		}
	}
}
