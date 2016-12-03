using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	// === inspector vars ===
	public bool debug;
	public GameFlowUI gameFlowUI;

	// === internal vars ===
	[HideInInspector]
	public bool busyLoading = false;
	[HideInInspector]
	public string lastLevel = "";
	[HideInInspector]
	public string nextMainCameraName = "";
	private Camera nextMainCamera;
	[HideInInspector]
	public string nextSpawnPointName = "";
	private Transform nextSpawnPoint;

	// === unity functions ===
	void Start () {
		if (Application.isEditor) {
			if (debug)
				Debug.Log ("running in editor, starting camera manager");
		}
		CameraManager.Get().UpdateOnLevelSwitch();
	}

	// === internal functions ===
	public Camera GetNewLevelMainCamera(){
		// first load from menu, there was no last level connection, so just use the labeled main camera in the scene
		if (nextMainCameraName == ""){
			return GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		return nextMainCamera;
	}

	public void LoadLevel(string name){
		if (busyLoading) return;	//sanity check: don't begin loading a scene while another is mid load
		lastLevel = Application.loadedLevelName;
		StartCoroutine(CoLoadLevel(name));
	}
	
	private IEnumerator CoLoadLevel(string name){
		busyLoading = true;
		if (debug) Debug.Log("beginning level load");

		//cache start time
		float startTime = Time.time;
		//start UI transition sequence
		PlayerInput.LockGlobalInput();
		gameFlowUI.StartLevelTransitionUI();
		yield return new WaitForSeconds(1.25f);
		//wait until object load is complete
		AsyncOperation asyncOp = Application.LoadLevelAsync(name);
		if (!asyncOp.isDone){
			yield return asyncOp;
		}
		//load camera and spawn point
		if (GameObject.FindWithTag("MainCamera").name == nextMainCameraName){
			nextMainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		} else {
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Camera")){
				if (obj.name == nextMainCameraName){
					nextMainCamera = obj.GetComponent<Camera>();
					break;
				}
			}
		}
		if (nextMainCamera == null) throw new UnityException("could not find camera with name "+nextMainCameraName+" in the loaded scene.");
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SpawnPoint")){
			if (obj.name == nextSpawnPointName){
				nextSpawnPoint = obj.GetComponent<Transform>();
				break;
			}
		}
		if (nextSpawnPoint == null) throw new UnityException("could not find spawn point with name "+nextSpawnPointName+" in the loaded scene.");
		//switch to the new camera so we can fade into that view
		gameFlowUI.GameFlowUICam.enabled = false;
		gameFlowUI.GameFlowUICam.gameObject.GetComponent<AudioListener>().enabled = false;
		CameraManager.Get().UpdateOnLevelSwitch();
		GameStatics.PlayerObj.transform.position = nextSpawnPoint.position;
		GameStatics.PlayerObj.transform.rotation = nextSpawnPoint.rotation;
		//sleep any extra time if needed
		if (Time.time - startTime < gameFlowUI.minLevelTransitionDelay){
			float delayTime = gameFlowUI.minLevelTransitionDelay-(Time.time-startTime);
			Debug.Log("loaded quickly, waiting for "+(delayTime)+" seconds to meet minimum transition time.");
			yield return new WaitForSeconds(delayTime);
		}
		//end UI transition sequence
		gameFlowUI.StopLevelTransitionUI();
		PlayerInput.UnlockGlobalInput();

		if(debug) Debug.Log("level loaded in "+(Time.time-startTime)+" seconds.");
		busyLoading = false;
	}
}