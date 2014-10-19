using UnityEngine;

/// <summary>
/// This is the controller class to allow passage through a
/// blockade designed specifically for the Thief role to 
/// open.
/// </summary>
public class ThiefBlockade : Blockade {
	#region Public Methods

/// <summary>
/// Open the door through the blockade and destroy all indicators
/// use to show that this blockade must be unlocked.
/// </summary>
	public override void Open() {
		base.Open();

	//Open the door
		Door.transform.position = new Vector3(
			Door.transform.position.x - 1.45f,
			Door.transform.position.y + 0.25f,
			Door.transform.position.z - 0.37f
		);

		Door.transform.Rotate(0.0f, 90.0f, 0.0f);
	}

	#endregion
}