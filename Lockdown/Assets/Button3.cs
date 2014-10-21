using UnityEngine;
using System.Collections;

public class Button3 : MonoBehaviour {
	
	public LinkedPlatformSide platform;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void OnTriggerEnter(Collider other)
	{
		platform.Platform1switch ();
	}
}
