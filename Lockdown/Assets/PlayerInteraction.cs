using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

	int keys;
	int floppies;
	int hammers;

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
		else if (col.gameObject.tag == "Player 2" && gameObject.tag == "Floppy Disk")
		{
			col.gameObject.GetComponent<PlayerInfo> ().items++;
			Destroy (gameObject);
		}
		else if (col.gameObject.tag == "Player 3" && gameObject.tag == "Hammer")
		{
			col.gameObject.GetComponent<PlayerInfo> ().items++;
			Destroy (gameObject);
		}

		else if(col.gameObject.tag == "Player 1" && gameObject.tag == "Thief")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			gameObject.GetComponent<ThiefBlockade>().Open();
			col.gameObject.GetComponent<PlayerInfo> ().items--;
			Destroy(GetComponent<BoxCollider>());
		}
		else if (col.gameObject.tag == "Player 2" && gameObject.tag == "Hacker")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			gameObject.GetComponent<HackerBlockade>().Open();
			col.gameObject.GetComponent<PlayerInfo> ().items--;
			Destroy(GetComponent<BoxCollider>());
		}
		else if (col.gameObject.tag == "Player 3" && gameObject.tag == "Brute")
		{
			if(col.gameObject.GetComponent<PlayerInfo> ().items <= 0)
				return;
			gameObject.GetComponent<BruteBlockade>().Open();
			col.gameObject.GetComponent<PlayerInfo> ().items--;
			Destroy(GetComponent<BoxCollider>());
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
