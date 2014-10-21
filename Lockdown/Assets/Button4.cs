using UnityEngine;
using System.Collections;

public class Button4 : MonoBehaviour {
	
	public ForcefieldTrigger forcefield;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void OnTriggerEnter(Collider other)
	{
		forcefield.Forcefieldswitch ();
	}
}
