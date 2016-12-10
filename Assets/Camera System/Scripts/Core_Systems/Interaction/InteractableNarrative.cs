using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractableNarrative : InteractableComponent{

	public string message;
	public Text text;
	public GameObject mask;

	private bool isDisplayingText = false;
	private bool justInteracted = false;

	void Start(){
		text.fontSize = Screen.height / 20;
		message = message.Replace ("\\", "\n");
	}

	void LateUpdate(){
		if (isDisplayingText && !justInteracted && Input.anyKeyDown) {
			KillText ();
			isDisplayingText = false;
		}
		justInteracted = false;
	}

	public override void OnInteract()
	{
		if (isDisplayingText) {
			KillText ();
		}else{
			text.text = message;
			mask.SetActive (true);
		}
		isDisplayingText = !isDisplayingText;
		justInteracted = true;
	}

	void KillText()
	{
		text.text = "";
		mask.SetActive (false);
	}
}
