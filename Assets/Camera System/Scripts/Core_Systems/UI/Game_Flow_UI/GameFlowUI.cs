using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameFlowUI : MonoBehaviour {

	// === inspector vars ===
	public float minLevelTransitionDelay;	//includes the fade in and fade out time, so make sure this is at least long enough to do both
	public Image blackScreen;

	// === internal vars ===
	private Camera gameFlowUICam;
	public Camera GameFlowUICam {get{return gameFlowUICam;}}
	private bool inLevelTransition = false;

	// === unity functions ===
	void Start(){
		gameFlowUICam = GetComponentInChildren<Camera>();
		if (gameFlowUICam.enabled){
			gameFlowUICam.enabled = false;
			gameFlowUICam.GetComponent<AudioListener>().enabled = false;
		}
	}

	// === external access UI functions ===
	public void StartLevelTransitionUI(){
		inLevelTransition = true;
		StartCoroutine(LevelTransitionUI());
	}

	public void StopLevelTransitionUI(){
		inLevelTransition = false;
	}

	// === internal UI control functions ===
	private IEnumerator LevelTransitionUI(){
		//setup / fade in
		Color blackScreenColor = blackScreen.color;
		for (float t=0; t<1f; t+=Time.unscaledDeltaTime){
			blackScreenColor.a = Mathf.Lerp(0f, 1f, t);
			blackScreen.color = blackScreenColor;
			yield return null;
		}
		blackScreenColor.a = 1f;
		blackScreen.color = blackScreenColor;
		yield return new WaitForSeconds(.25f);
		CameraManager.Get().mainSiphonCam.Deactivate();
		//losing the other camera while we switch, but we still need a camera to render our loading UI, so we use the always existant gameFlowUICam
		gameFlowUICam.enabled = true;
		gameFlowUICam.gameObject.GetComponent<AudioListener>().enabled = true;


		//updating logic
		while (inLevelTransition){
			//TODO: loading screen
			yield return null;
		}
		//now we have loaded the level, since StopLevelTransitionUI is not called until the async load is complete

		//cleanup / fade out
		//we are on the new camera now (the cameras are switched in level manager before StopLevelTransitionUI is called), so fade in to that view
		for (float t=0; t<1f; t+=Time.unscaledDeltaTime){
			blackScreenColor.a = Mathf.Lerp(1f, 0f, t);
			blackScreen.color = blackScreenColor;
			yield return null;
		}
		blackScreenColor.a = 0f;
		blackScreen.color = blackScreenColor;
	}
}
