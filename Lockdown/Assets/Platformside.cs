using UnityEngine;
using System.Collections;

public class Platformside : MonoBehaviour {
	Vector3 startpos;

	// Use this for initialization
	void Start () {

		Physics.IgnoreLayerCollision(11, 12, true);
		startpos = gameObject.transform.position;
	
	}


	// Update is called once per frame
	void Update () {
		gameObject.transform.localPosition = new Vector3(0.0f,-200.5f,0.0f);
	}
}
