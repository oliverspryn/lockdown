using UnityEngine;

public class VideoPlayer : MonoBehaviour {
	public void Start() {
		MovieTexture mt = renderer.material.mainTexture as MovieTexture;
		audio.clip = mt.audioClip;

		audio.Play();
		mt.Play();
	}
}