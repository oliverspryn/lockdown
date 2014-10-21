using UnityEngine;

/// <summary>
/// The controller script for counting how many players have
/// entered the elevator.
/// </summary>
public class CollisionDetection : MonoBehaviour {
	#region Fields

/// <summary>
/// A reference to the elevator, so that the occupant counter can
/// be incremented when a player runs into the counter.
/// </summary>
	public GameObject Elevator;

	#endregion

	#region Private Members

/// <summary>
/// Decrement the number of people required to shut the door whenever
/// a new player enters the elevator
/// </summary>
/// 
/// <param name="c">The object the player collides with</param>
	private void OnTriggerEnter(Collider c) {
		--Elevator.GetComponent<Elevator>().PlayerCount;
	}

	#endregion
}