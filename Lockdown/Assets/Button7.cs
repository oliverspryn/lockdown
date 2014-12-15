using UnityEngine;
using System.Collections;

public class Button7 : MonoBehaviour {
	
	public LinkedPlatformCenter platform;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void OnTriggerEnter(Collider other)
	{
		platform.Platformswitch ();
	}
}
