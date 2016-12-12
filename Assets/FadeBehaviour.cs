using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeBehaviour : MonoBehaviour 
{
	public float fadeSpeed = 0.3f;

	public Texture fadeOutTexture;
	private int drawDepth = -1000;
	public float alpha = .99f;
	public float fadeDir = -1.0f;
	private Color alphaColor;

	// Use this for initialization
	void OnGUI() 
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		alphaColor = GUI.color;
		alphaColor.a = alpha;
		GUI.color = alphaColor;
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

		if (alpha == 1)
		{
			SceneManager.LoadScene (1);
		}
	}
	
	public void FadeIn()
	{
		fadeDir = -1;
	}

	public void FadeOut()
	{
		fadeDir = 1;
	}
}
