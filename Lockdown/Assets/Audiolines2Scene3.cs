using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audiolines2Scene3 : MonoBehaviour {
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
	}
	
	// Update is called once per frame
	void Update () {

		//continue explosions
		if(stage >= 1)
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
			Transform child = dict["Explode"];
			child.audio.Play();
			}
		if (stage == 1) {
			Transform child = dict["Explode"];

		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(stage == -1)
		{
			gameObject.audio.Play ();
			stage = 0;
		}
	}

}
