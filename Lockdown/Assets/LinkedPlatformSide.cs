using UnityEngine;
using System.Collections;

public class LinkedPlatformSide : MonoBehaviour {

	public LinkedPlatformSide platform1;
	public LinkedPlatformCenter platform2;
	public LinkedPlatformSide platform3;
	public LinkedPlatformCorner platform4;

	public float start_x = -2f;
	public float start_y = 0f;
	public float start_z = -58f;
	public float Velocity = 3.0f;
	public bool switchflipped = true;
	public float pause = 5.0f;
	public float counter = 0.0f;
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
				if (switchflipped && gameObject.transform.position.y < 54.4f) {
				
						tempvel.y = Velocity;
				} else if (gameObject.rigidbody.position.y > start_y && !switchflipped) {
						tempvel.y = -Velocity;
				} else {
						counter = 0;
						tempvel.y = 0;
				
				}
				gameObject.rigidbody.velocity = tempvel;
		}
		public void Platformswitch ()
		{
			switchflipped = !switchflipped;
			counter = 0;
		}
	public void Platform1switch ()
	{
		switchflipped = !switchflipped;
		counter = 0;
	}
	void OnTriggerExit(Collider other)
	{
		if (switchflipped) {
						switchflipped = !switchflipped;
						platform1.Platformswitch ();
						platform2.Platformswitch ();
						platform3.Platformswitch ();
						platform4.Platformswitch ();
				}
	}
}
