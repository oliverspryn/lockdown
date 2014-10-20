using UnityEngine;
using System.Collections;

/// <summary>
/// This class will control all of the features of the elevator, such
/// as keeping track of players when they enter, closing the doors,
/// simulating movement, and transitioning to the next level.
/// </summary>
public class Elevator : MonoBehaviour {
	#region Fields

/// <summary>
/// A reference to the object a player musty collide with in order to 
/// be considered "in" the elevator.
/// </summary>
	public GameObject CollisionTracker;

/// <summary>
/// When looking at the doors, into the elevator from outside, which
/// door is on the left.
/// </summary>
	public GameObject DoorLeft;

/// <summary>
/// When looking at the doors, into the elevator from outside, which
/// door is on the right.
/// </summary>
	public GameObject DoorRight;

/// <summary>
/// The lights which should be dimmed out when the level is
/// transitioning.
/// </summary>
	public GameObject[] Lights;

/// <summary>
/// The point light which should be moved in order to simulate movement
/// when the elevator is "going up".
/// </summary>
	public GameObject MovementLight;

/// <summary>
/// The number of players which must be in the elevator before the
/// doors will close (basically, all the players).
/// </summary>
	public int PlayerCount;

	#endregion

	#region Private Members

/// <summary>
/// Whether or not the elevator should be moving.
/// </summary>
	private bool Active = false;

/// <summary>
/// The initial position of the movement light.
/// </summary>
	private Vector3 MovementLightStart;

	#endregion


	// Use this for initialization
	void Start () {
		MovementLightStart = MovementLight.transform.position;

		//Vector3 bottom = MovementLightStart;
		//bottom.y = gameObject.transform.position.y - DoorLeft.transform.localScale.y;
		//MovementLight.transform.position = bottom;
	}
	
	// Update is called once per frame
	void Update () {
	//Is the elevator moving?
		if(!Active)
			return;

	//Dim the lights
		for(int i = 0; i < Lights.Length; ++i) {
			Lights[i].light.intensity -= 0.01f;
		}

	//Simulate movement with the movement light
		if(MovementLight.transform.position.y > MovementLightStart.y + (2.0f * DoorLeft.transform.localScale.y)) {
			MovementLight.transform.position = MovementLightStart;
		}

		Vector3 top = MovementLightStart;
		top.y += 3.0f * DoorLeft.transform.localScale.y;

		MovementLight.transform.position = Vector3.Lerp(MovementLight.transform.position, top, Time.deltaTime / 3.0f);
	}
}
