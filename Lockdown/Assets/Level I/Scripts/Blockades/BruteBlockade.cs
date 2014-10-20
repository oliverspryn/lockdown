using UnityEngine;

/// <summary>
/// This is the controller class to allow passage through a
/// blockade designed specifically for the Brute role to 
/// open.
/// </summary>
public class BruteBlockade : Blockade {
	#region Fields

/// <summary>
/// The door to display whenever the original glass door has
/// been opened, by breaking it.
/// </summary>
	public GameObject BrokenDoor;

	#endregion

	#region Public Methods

/// <summary>
/// Open the door by removing the glass door and replacing it with
/// a smashed version of the door, which looks like it is hanging
/// ont its hinges.
/// </summary>
	public override void Open() {
		base.Open();

	//Save information about the glass door
		Vector3 pos = Door.transform.position;
		Quaternion rot = Door.transform.rotation;
		Vector3 scale = Door.transform.localScale;

		if(gameObject.transform.rotation.y == 0.0f) {
			pos.x -= 4.75f;
			pos.y -= 0.5f;
			pos.z += 6.3f;
		} else { // Algorithm will rotate by 90 degrees
			pos.x += 6.41f;
			pos.y -= 0.5f;
			pos.z += 4.71f;
		}

	//Destroy the old door
		Destroy(Door);

	//And replace it with the broken door
		BrokenDoor = Instantiate(BrokenDoor) as GameObject;
		BrokenDoor.transform.parent = gameObject.transform;

		BrokenDoor.transform.localScale = scale;
		BrokenDoor.transform.position = pos;
		BrokenDoor.transform.rotation = rot;

	//Orient the door in the correct direction
		BrokenDoor.transform.Rotate(-15.0f, 90.0f, 0.0f);
	}

	#endregion
}