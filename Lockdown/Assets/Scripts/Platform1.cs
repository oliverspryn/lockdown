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
	public float maxY = 54.4f;

	// Use this for initialization
	void Start () {
		Vector3 startpos = gameObject.transform.position;
		start_x = startpos.x;
		start_y = startpos.y;
		start_z = startpos.z;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempvel = gameObject.rigidbody.velocity;
		tempvel.x = 0;
		tempvel.z = 0;
		if (switchflipped && gameObject.transform.position.y < maxY) {
						
						tempvel.y = Velocity;
				} else if (counter < pause && switchflipped) {
						counter += Time.deltaTime;
						tempvel.y = 0;
				} else if (gameObject.rigidbody.position.y > start_y) {
			switchflipped = false;
			tempvel.y = -Velocity;
		} else {
			counter = 0;
			tempvel.y = 0;

		}
		gameObject.rigidbody.velocity = tempvel;
	}
	public void Platform1switch ()
	{
		switchflipped = true;
		counter = 0;
	}
	public void Platform1inverseswitch ()
	{
		switchflipped = false;
		counter = 6;
	}
}
