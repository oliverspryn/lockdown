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

	public override void Open() {
		base.Open();

	//Save information about the glass door
		Vector3 pos = Door.transform.position;
		Quaternion rot = Door.transform.rotation;
		Vector3 scale = Door.transform.localScale;

		pos.x -= 4.75f;
		pos.y -= 0.5f;
		pos.z += 6.3f;

	//Destroy the old door
		Destroy(Door);

	//And replace it with the broken door
		BrokenDoor = Instantiate(BrokenDoor) as GameObject;
		BrokenDoor.transform.localScale = scale;
		BrokenDoor.transform.position = pos;
		BrokenDoor.transform.rotation = rot;

	//Orient the door in the correct direction
		BrokenDoor.transform.Rotate(-15.0f, 90.0f, 0.0f);
	}

	#endregion
}