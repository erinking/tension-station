using UnityEngine;
using System.Collections;

public class InteractableComponent : MonoBehaviour {

	public virtual void OnInteract(){
		throw new UnityException("Interactable component on "+this.gameObject+" hit, but the specific interactable component subclass "+
		                         "has not overridden the OnInteract() method.");
	}
}
