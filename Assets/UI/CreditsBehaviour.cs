using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsBehaviour : MonoBehaviour {

	public RawImage mask;
	public Text text;
	public string nextLevel;
	public float relativeFontSize = 0.03f;

	private float fadeTime = 0.5f;
	private float currentFade = 1.5f;
	private bool fadeOut = false;

	// Use this for initialization
	void Start () {
		text.fontSize = (int)(Screen.height * relativeFontSize);
		mask.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		float inputVert = Input.GetAxis ("Vertical") + Input.GetAxis ("VerticalController");
		text.transform.Translate (0, -inputVert * Time.deltaTime * 200, 0);

		if (fadeOut) {
			currentFade += Time.deltaTime / fadeTime;
			if (currentFade >= 1) {
				SceneManager.LoadScene (nextLevel);
			}
		} else if(currentFade > 0) {
			currentFade -= Time.deltaTime / fadeTime;
		}
		mask.color = new Color (0, 0, 0, currentFade);

		if (Input.GetButtonDown ("Interact")) {
			fadeOut = true;
		}
	}
}
