using UnityEngine;
using System.Collections;

public class Button1 : MonoBehaviour {

	public Platform1 platform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}
	void OnTriggerStay(Collider other)
	{
		platform.Platform1switch ();
	}
	void OnTriggerExit(Collider other)
	{
		platform.Platform1inverseswitch ();
	}
}
