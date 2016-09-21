using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput {
	//input wrapper, handles blocking of input and input settings
	
	// === input wrapper functions ===
	public static float GetAxis(string axis){
		if (globalLock || axisLocks.Contains(axis)){
			return 0f;
		}
		return Input.GetAxis(axis);
	}

	public static bool GetButton(string btn){
		if (globalLock || buttonLocks.Contains(btn)){
			return false;
		}
		return Input.GetButton(btn);
	}

	public static bool GetButtonDown(string btn){
		if (globalLock || buttonLocks.Contains(btn)){
			return false;
		}
		return Input.GetButtonDown(btn);
	}

	public static bool GetButtonUp(string btn){
		if (globalLock || buttonLocks.Contains(btn)){
			return false;
		}
		return Input.GetButtonUp(btn);
	}


	// === locks ===
	private static bool globalLock = false;	//sometimes we just want to lock everything, so we use this as a shortcut to setting all other flags
	private static List<string> axisLocks = new List<string>();
	private static List<string> buttonLocks = new List<string>();


	// === locking/unlocking functions ===
	public static void LockGlobalInput(){
		globalLock = true;
	}
	public static void UnlockGlobalInput(){
		globalLock = false;
	}
	
	public static void LockAxis(string axis){
		if (axisLocks.Contains(axis)) return;
		axisLocks.Add(axis);
	}
	public static void UnlockAxis(string axis){
		axisLocks.Remove(axis);
	}
	
	public static void LockButton(string btn){
		if (buttonLocks.Contains(btn)) return;
		buttonLocks.Add(btn);
	}
	public static void UnlockButton(string btn){
		buttonLocks.Remove(btn);
	}
}
