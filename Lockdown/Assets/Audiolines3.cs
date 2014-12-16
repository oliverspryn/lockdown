﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audiolines3 : MonoBehaviour {
	int stage;
	Dictionary<string, Transform> dict = new Dictionary<string, Transform>();
	// Use this for initialization
	void Start () {
		stage = -1;
		foreach(Transform t in transform)
		{
			dict.Add(t.name, t);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameObject.audio.isPlaying && stage == 0) {
			stage = 1;
			Transform child = dict["VonStoiker11"];
			child.audio.Play();
		}
		if (stage == 1) {
			Transform child = dict["VonStoiker11"];
			if(!child.audio.isPlaying)
			{
				stage = 2;
				child = dict["Mia12"];
				child.audio.Play();
			}
			
		}
		if (stage == 2) {
			Transform child = dict["Mia12"];
			if(!child.audio.isPlaying)
			{
				stage = 3;
				child = dict["Carl13"];
				child.audio.Play();
			}
			
		}
		if (stage == 3) {
			Transform child = dict["Carl13"];
			if(!child.audio.isPlaying)
			{
				stage = 4;
				child = dict["Vincent14"];
				child.audio.Play();
			}
			
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
