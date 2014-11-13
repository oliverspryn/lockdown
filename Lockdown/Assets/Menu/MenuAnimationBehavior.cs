﻿using UnityEngine;
using System.Collections;

public class MenuAnimationBehavior : MonoBehaviour {
	public Animator Model1;
	public Animator Model2;
	public Animator Model3;

	public int Controller;


	Animator[] Models;
	int model;
	string playerInputSuffix;

	// Use this for initialization
	void Start () {
		model = 3;
		//do stuff
		Models = new Animator[4];
		Models [0] = Model1;
		Models [1] = Model2;
		Models [2] = Model3;
		Models [3] = null;
		playerInputSuffix = " P" + Controller;

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("LB" + playerInputSuffix))
		{
			if(model != 3)
				Models[model].SetBool ("Selected", false);
			model--;
		}
		else if (Input.GetButtonDown ("RB" + playerInputSuffix))
		{
			if(model != 3)
				Models[model].SetBool ("Selected", false);
			model++;
		}
		model = (model + 4) % 4;
		if(model != 3)
			Models[model].SetBool ("Selected", true);
	}
}