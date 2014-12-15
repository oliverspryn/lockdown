using UnityEngine;
using System.Collections;

public class LevelLoadingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		LevelManager levelMan = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

		levelMan.TransitionLevel ("Level 1");
	}
}
