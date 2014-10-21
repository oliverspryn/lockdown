using UnityEngine;
using System.Collections;

public class ForcefieldTrigger : MonoBehaviour {

	public bool autoflip = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Forcefieldswitch ()
	{
		gameObject.transform.Rotate (new Vector3 (0, 180, 0));

	}
	void OnTriggerEnter(Collider other)
	{
		if(autoflip)
			gameObject.transform.Rotate (new Vector3 (0, 180, 0));
	}
}
