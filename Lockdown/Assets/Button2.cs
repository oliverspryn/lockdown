using UnityEngine;
using System.Collections;

public class Button2 : MonoBehaviour {
	public Platform1 platform1;
	public Platform1 platform2;
	public Platform1 platform3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerStay(Collider other)
	{
		platform1.Platform1switch ();
		platform2.Platform1switch ();
		platform3.Platform1switch ();
	}
}
