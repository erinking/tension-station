using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip[] ambienceClips;

	private GameObject ambience1;
	private GameObject ambience2;

	void Start(){
		playAmbience();
	}

	/**
	 * Make an ambient noise object and start it playing a random clip from ambienceClips.
	 * Then call this function again when this clip is halfway done or the previously
	 * playing clip is guaranteed to be done, whichever happens later.
	 */
	void playAmbience(){
		float otherLength = 0;
		if (ambience1 != null) {
			Destroy (ambience2);
			ambience2 = ambience1;
			otherLength = ambience2.GetComponent<AudioSource> ().clip.length;
		}
		ambience1 = new GameObject("Ambience");
		AudioSource source = ambience1.AddComponent<AudioSource> ();
		source.clip = ambienceClips [Random.Range (0, ambienceClips.Length)];
		source.spatialBlend = 0;
		source.volume = 0.2f;
		source.bypassReverbZones = true;
		source.Play ();
		Invoke ("playAmbience", Mathf.Max(source.clip.length,otherLength) / 2);
	}
}
