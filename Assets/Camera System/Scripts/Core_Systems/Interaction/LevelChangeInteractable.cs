using UnityEngine;
using System.Collections;

public class LevelChangeInteractable : InteractableComponent {

	// === inspector vars ===
	public string nextLevelStr = "";
	public string nextLevelMainCameraStr = "";
	public string nextLevelSpawnPointStr = "";
	public AudioClip interactAudioClip;

	// === internal vars ===
	private AudioSource persistantAudioSrc;

	// === unity functions ===
	void Start(){
		persistantAudioSrc = GameObject.FindWithTag("PersistantAudio").GetComponent<AudioSource>();
	}

	// === interaction functions ===
	public override void OnInteract(){
		if (nextLevelStr == "" || nextLevelMainCameraStr == "" || nextLevelSpawnPointStr == ""){
			throw new UnityException("LevelChangeInteractable on "+gameObject+" is not fully set up, this could cause loading errors.");
		}
		if (interactAudioClip != null){
			persistantAudioSrc.PlayOneShot(interactAudioClip);
		}
		GameStatics.LevelManager.nextMainCameraName = nextLevelMainCameraStr;
		GameStatics.LevelManager.nextSpawnPointName = nextLevelSpawnPointStr;
		GameStatics.LevelManager.LoadLevel(nextLevelStr);
	}
}
