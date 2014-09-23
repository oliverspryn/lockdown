using UnityEngine;
using System.Collections;

public class Level1Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay(Collider other)
	{
		Application.LoadLevel(1);
	}
}
