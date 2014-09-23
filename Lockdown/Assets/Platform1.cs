using UnityEngine;
using System.Collections;



public class Platform1 : MonoBehaviour {

	public float start_x = -2f;
	public float start_y = 0f;
	public float start_z = -58f;
	public float Velocity = 3.0f;
	public bool switchflipped = true;
	public float pause = 5.0f;
	public float counter = 0.0f;

	// Use this for initialization
	void Start () {
		Vector3 startpos;
		startpos.x = start_x;
		startpos.y = start_y;
		startpos.z = start_z;
		gameObject.rigidbody.position = startpos;
	}
	
	// Update is called once per frame
	void Update () {
		if (switchflipped && gameObject.rigidbody.position.y < 2) {
						Vector3 tempvel = gameObject.rigidbody.velocity;
						tempvel.y = Velocity;
						gameObject.rigidbody.velocity = tempvel;
				} else if (counter < pause && switchflipped) {
						counter += Time.deltaTime;
						Vector3 tempvel = gameObject.rigidbody.velocity;
						tempvel.y = 0;
						gameObject.rigidbody.velocity = tempvel;
				} else if (gameObject.rigidbody.position.y > start_y) {
			switchflipped = false;
			Vector3 tempvel = gameObject.rigidbody.velocity;
			tempvel.y = -Velocity;
			gameObject.rigidbody.velocity = tempvel;
		} else {
			switchflipped = false;
			counter = 0;
			Vector3 tempvel = gameObject.rigidbody.velocity;
			tempvel.y = 0;
			gameObject.rigidbody.velocity = tempvel;
		}
		
	}
	public void Platform1switch ()
	{
		switchflipped = true;
		counter = 0;
	}
}
