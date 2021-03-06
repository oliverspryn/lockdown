﻿using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {
	public int buttons = 3;
	public float speed = .2f;
	// Use this for initialization
	void Start () {
		//spawn ();
	}

	void OnTriggerEnter(Collider col)
	{
		//scoreText1.text = col.gameObject.name;
		if (col.gameObject.tag == "Player 1" && gameObject.tag == "Key")
		{
			col.gameObject.GetComponent<PlayerInfo> ().items++;
			Destroy (gameObject);
		}
		else if (col.gameObject.tag == "Player 3" && gameObject.tag == "Floppy")
		{
			col.gameObject.GetComponent<PlayerInfo> ().items++;
			Destroy (gameObject);
		}
		else if (col.gameObject.tag == "Player 2" && gameObject.tag == "Hammer")
		{
			col.gameObject.GetComponent<PlayerInfo> ().items++;
			Destroy (gameObject);
		}

		else if(col.gameObject.tag == "Player 1" && gameObject.tag == "Thief")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			col.gameObject.GetComponent<GUIScript>().beginSequence(buttons, speed, gameObject);
			//gameObject.GetComponent<ThiefBlockade>().Open();
			//col.gameObject.GetComponent<PlayerInfo> ().items--;
			//Destroy(GetComponent<BoxCollider>());
		}
		else if (col.gameObject.tag == "Player 3" && gameObject.tag == "Hacker")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			col.gameObject.GetComponent<GUIScript>().beginSequence(buttons, speed, gameObject);
			//gameObject.GetComponent<HackerBlockade>().Open();
			//col.gameObject.GetComponent<PlayerInfo> ().items--;
			//Destroy(GetComponent<BoxCollider>());
		}
		else if (col.gameObject.tag == "Player 2" && gameObject.tag == "Brute")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			col.gameObject.GetComponent<GUIScript>().beginSequence(buttons, speed, gameObject);
			//gameObject.GetComponent<BruteBlockade>().Open();
			//col.gameObject.GetComponent<PlayerInfo> ().items--;
			//Destroy(GetComponent<BoxCollider>());
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
