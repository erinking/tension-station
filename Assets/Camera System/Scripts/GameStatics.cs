using UnityEngine;
using System.Collections;

public class GameStatics {

	private static GameObject playerObj;
	public static GameObject PlayerObj{
		get{
			if(playerObj == null){
				playerObj = GameObject.FindWithTag("Player");
			}
			return playerObj;
		}
	}

	private static LevelManager levelManager;
	public static LevelManager LevelManager{
		get{
			if (levelManager == null){
				levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
			}
			return levelManager;
		}
	}

}
