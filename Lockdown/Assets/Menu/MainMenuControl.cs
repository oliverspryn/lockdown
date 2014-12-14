﻿using UnityEngine;
using System.Collections;

public class MainMenuControl : MonoBehaviour {

	public GameObject optionsObj;
	public Camera C1;
	public Camera C2;
	public Camera C3;
	public Camera HostScreen;
	public Camera ClientScreen;
	public Camera LocalScreen;

	float lastDir;
	TextMesh[] options;
	int num;
	int cur;
	// Use this for initialization
	void Start () {
		lastDir = 0f;
		cur = 0;
		num = optionsObj.transform.childCount;
		options = new TextMesh[num];
		foreach (Transform child in optionsObj.transform)
			options[cur++] = child.GetComponent<TextMesh>();
		cur = 0;
		options [cur].color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw("Vertical P1") > 0 && lastDir <= 0)//Input.GetButtonDown ("LB P1"))
		{
			options[cur].color = Color.white;
			cur--;
		}
		else if (Input.GetAxisRaw("Vertical P1") < 0 && lastDir >= 0)//Input.GetButtonDown ("RB P1"))
		{
			options[cur].color = Color.white;
			cur++;
		}

		lastDir = Input.GetAxisRaw ("Vertical P1");
		cur = (cur + num) % num;
		options [cur].color = Color.red;

		if (Input.GetButtonDown ("X P1"))
		{
			//perform actions associated with selected option

			gameObject.SetActive(false);
			C1.gameObject.SetActive(true);
			C2.gameObject.SetActive(true);
			C3.gameObject.SetActive(true);

			if(options[cur].name == "New Local Game"){
				LocalScreen.gameObject.SetActive(true);
			} else if(options[cur].name == "Host Game 1"){
				HostScreen.gameObject.SetActive(true);
				HostScreen.GetComponent<LobbyCodeManager>().showStaticSequence(8);
			} else if(options[cur].name == "Join Game 1"){
				ClientScreen.gameObject.SetActive(true);
				ClientScreen.GetComponent<LobbyCodeManager>().enterStaticSequence(8);
			}
			gameObject.SetActive(false);

		}

	}
}
