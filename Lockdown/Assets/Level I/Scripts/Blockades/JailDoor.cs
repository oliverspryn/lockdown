using UnityEngine;

/// <summary>
/// This blockade is the jail door, the first obsticle which a player
/// will encounter when he or she is playing the game,
/// </summary>
public class JailDoor : Blockade {
	#region Fields

/// <summary>
/// Indicate which direction the steel prison doors will swing when
/// opening.
/// </summary>
	public OpeningDirection OpeningDirection = OpeningDirection.Positive;
	
	#endregion

	#region Public Methods

/// <summary>
/// Open the door by removing the glass door and replacing it with
/// a smashed version of the door, which looks like it is hanging
/// ont its hinges.
/// </summary>
	public override void Open() {
		if(IsOpen) {
			return;
		}

	//Orient the door in the correct direction
		int direction = (int)OpeningDirection;
		Vector3 pos = Door.transform.position;
		pos.x += 4.21f * direction;
		pos.z += 4.29f * direction;

		Door.transform.position = pos;
		Door.transform.Rotate(0.0f, 90.0f, 0.0f);
		base.Open();
	}

	#endregion
}