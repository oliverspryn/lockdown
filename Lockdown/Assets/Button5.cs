using UnityEngine;
using System.Collections;

public class Button5 : MonoBehaviour {
	
	public ForcefieldTrigger forcefield1;
	public ForcefieldTrigger forcefield2;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void OnTriggerEnter(Collider other)
	{
		forcefield1.Forcefieldswitch ();
		forcefield2.Forcefieldswitch ();
	}
}
