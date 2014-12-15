using UnityEngine;
using System.Collections;

public class Audiolines : MonoBehaviour {
	bool first;
	// Use this for initialization
	void Start () {
		first = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (first) {
						gameObject.audio.Play ();
			first = false;
				}
	}
}
