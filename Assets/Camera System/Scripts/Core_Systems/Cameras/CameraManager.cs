using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager {
	// === singleton logic ===
	private static CameraManager instance = null;
	public static CameraManager Get(){
		if (instance == null){
			instance = new CameraManager();
		}
		return instance;
	}

	// === internal vars ===
	public Camera main;
	public SiphonCamera mainSiphonCam;

	// === main functions ===
	//make sure to do this on new level load
	public void UpdateOnLevelSwitch(){
		main = GameStatics.LevelManager.GetNewLevelMainCamera();
		mainSiphonCam = main.gameObject.GetComponent<SiphonCamera>();
		if (mainSiphonCam.target == null){
			mainSiphonCam.SetTarget(GameStatics.PlayerObj);
		}
		mainSiphonCam.Activate();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Camera")){
			Camera cam = obj.GetComponent<Camera>();
			if (cam != main && cam.enabled){
				cam.gameObject.GetComponent<SiphonCamera>().Deactivate();
			}
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainCamera")){
			Camera cam = obj.GetComponent<Camera>();
			if (cam != main && cam.enabled){
				cam.gameObject.GetComponent<SiphonCamera>().Deactivate();
			}
		}
		GameStatics.PlayerObj.GetComponent<PlayerMovementController>().curMoveSpaceCameraTransform = main.transform;
	}

	public void SwitchCameras(Camera newCam, GameObject target){
		//it is possible to go from a trigger for a camera into a trigger for the same camera; don't switch to the camera we already have active
		//additionally, don't worry about the trigger which will be hit when a new level is loaded, wait for UpdateOnLevelSwitch to deal with that
		if (newCam == main || mainSiphonCam == null){
			return;
		}
		mainSiphonCam.Deactivate();
		main = newCam;
		mainSiphonCam = main.GetComponent<SiphonCamera>();
		mainSiphonCam.SetTarget(target);
		mainSiphonCam.Activate();
	}
}
