using UnityEngine;

/// <summary>
/// A C-style struct object which is used to hold a wall for a 
/// particular direction in a <c>Cell</c> object is visible, or
/// should be hidden beneath the floor.
/// </summary>
public class Walls {
	#region Constructors

/// <summary>
/// Creates a default instance of a <c>Walls</c> object, with the
/// object enabled, by default.
/// </summary>
/// 
/// <param name="wall">A <c>GameObject</c> which represents a wall</param>
	public Walls(GameObject wall) {
		Wall = wall;
		Enabled = true;
	}

	#endregion

	#region Fields

/// <summary>
/// Whether or not a wall object should be visible or enabled for 
/// use within the maze. If the wall is not visible, then it
/// should be hidden benieth the floor.
/// </summary>
	public bool Enabled {
		get {
			return _enabled;
		}

		set {
			Vector3 location = Wall.transform.position;
			location.y *= value ? 1 : -1;

			Wall.transform.position = location;
			_enabled = value;
		}
	}

	private bool _enabled;

/// <summary>
/// This field holds a <c>GameObject</c> which represents a wall
/// for a <c>Cell</c> object.
/// </summary>
	public GameObject Wall { get; set; }

	#endregion
}