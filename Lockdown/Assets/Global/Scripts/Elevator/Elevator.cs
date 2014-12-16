using UnityEngine;

/// <summary>
/// This class will control all of the features of the elevator, such
/// as keeping track of players when they enter, closing the doors,
/// simulating movement, and transitioning to the next level.
/// </summary>
public class Elevator : MonoBehaviour {
	#region Fields

/// <summary>
/// Whether or not this elevator is active, and has its lights on.
/// </summary>
	public bool Active = true;

/// <summary>
/// A reference to the object a player musty collide with in order to 
/// be considered "in" the elevator.
/// </summary>
	public GameObject CollisionTracker;

/// <summary>
/// The quad which is used to contain the player within the elevator.
/// </summary>
	public GameObject Container;

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
/// The name of the level to transition to after the elevator has 
/// "completed" its movement.
/// </summary>
	public string Level;

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
/// The initial position of the movement light.
/// </summary>
	private Vector3 MovementLightStart;

	#endregion

	#region Constructors

/// <summary>
/// Keep track of the movement light's original position so that it
/// can transition back to this original spot as it keeps moving 
/// vertically through the cabin of the elevator. Also, decide whether
/// or not this elevator is active.
/// </summary>
	void Start () {
		MovementLightStart = MovementLight.transform.position;

	//Deactivate some components
		if(!Active) {
			foreach(GameObject l in Lights) {
				Destroy(l);
			}
		}

		Destroy(Container);
	}

	#endregion

	#region Public Methods

/// <summary>
/// Close the doors, dim the lights, and simulate movement.
/// </summary>
	public void Update () {
	//Is the elevator moving?
		if(PlayerCount != 0 || !Active)
			return;

	//Close the doors
		Vector3 scale = DoorLeft.transform.localScale;

		if(scale.z < 13.58f) {
			scale.z += 0.2f;
			DoorLeft.transform.localScale = DoorRight.transform.localScale = scale;
			
			if (scale.z >= 13.58f) {
				Vector3 pos = DoorLeft.transform.position;
				pos.z += 0.09f;
				DoorLeft.transform.position = pos;
			}
		}

	//Dim the lights, and transition the level after they have gone out
		for(int i = 0; i < Lights.Length; ++i) {
			Lights[i].light.intensity -= 0.01f;

			if(Lights[i].light.intensity < 0.01f) {
				LevelManager levelMgr = GameObject.Find("Level Manager").GetComponent<LevelManager>();
				levelMgr.TransitionLevel(Level);
				return;
			}
		}

	//Simulate movement with the movement light
		MovementLight.SetActive(true);

		if(MovementLight.transform.position.y > MovementLightStart.y + (2.0f * DoorLeft.transform.localScale.y)) {
			MovementLight.transform.position = MovementLightStart;
		}

		Vector3 top = MovementLightStart;
		top.y += 3.0f * DoorLeft.transform.localScale.y;

		MovementLight.transform.position = Vector3.Lerp(MovementLight.transform.position, top, Time.deltaTime / 3.0f);
	}

	#endregion
}