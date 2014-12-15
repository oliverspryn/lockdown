using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audiolines1FinalScene : MonoBehaviour {
	int stage;
	float[] seeds;
	float elapsedTime;
	Transform[] cloneExplosions;
	Dictionary<string, Transform> dict = new Dictionary<string, Transform>();
	// Use this for initialization
	void Start () {
		stage = -1;
		elapsedTime = 0;
		cloneExplosions = new Transform[10];
		seeds = new float[10];
		for(int i = 0; i < 10; ++i)
		{
			seeds[i] = Random.Range (.2f,.4f);
		}
		foreach(Transform t in transform)
		{
			dict.Add(t.name, t);
		}
		gameObject.audio.Play ();
		stage = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//continue explosions
		if(stage >= 4)
		{
			elapsedTime += Time.deltaTime;
			for(int i = 0; i < 10; ++i)
			{
				if(elapsedTime > seeds[i]*i && (cloneExplosions[i] == null))
				{
					cloneExplosions[i] = GameObject.Instantiate(dict["Explode"]) as Transform;
					cloneExplosions[i].audio.Play ();
					seeds[i] += Random.Range (.2f,.4f);
				}
				else if(cloneExplosions[i] != null && !cloneExplosions[i].audio.isPlaying)
				{
					GameObject.Destroy(cloneExplosions[i].gameObject);
					cloneExplosions[i] = null;
				}
			}
		}

		if (!gameObject.audio.isPlaying && stage == 0) {
			stage = 1;
			Transform child = dict["Carl14"];
			child.audio.Play();
			}
		if (stage == 1) {
			Transform child = dict["Carl14"];
			if(!child.audio.isPlaying)
			{
				stage = 2;
				child = dict["Von15"];
				child.audio.Play();
			}

		}
		if (stage == 2) {
			Transform child = dict["Von15"];
			if(!child.audio.isPlaying)
			{
				stage = 3;
				child = dict["Mia11"];
				child.audio.Play();
			}
			
		}
		if (stage == 3) {
			Transform child = dict["Mia11"];
			if(!child.audio.isPlaying)
			{
				stage = 4;
				child = dict["Explode"];
				child.audio.Play();
			}
			
		}
		if (stage == 4) {
			Transform child = dict["Explode"];
			if(!child.audio.isPlaying)
			{
				stage = 5;
				child = dict["Mia12"];
				child.audio.Play();
			}
			
		}
		if (stage == 5) {
			Transform child = dict["Mia12"];
			if(!child.audio.isPlaying)
			{
				stage = 6;
				child = dict["Carl15"];
				child.audio.Play();
			}
			
		}
		if (stage == 6) {
			Transform child = dict["Carl15"];
			if(!child.audio.isPlaying)
			{
				stage = 7;
				child = dict["Vincent8"];
				child.audio.Play();
			}
			
		}
	}

}
