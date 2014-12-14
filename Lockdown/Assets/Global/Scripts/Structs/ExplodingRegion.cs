using UnityEngine;

/// <summary>
/// A struct which is used to define the 3D space in which
/// an <c>Explosion</c> may occur.
/// </summary>
public class ExplodingRegion {
	#region Fields

/// <summary>
/// The backward side (-Z) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Backward {
		get;
		private set;
	}

/// <summary>
/// The down side (-Y) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Down {
		get;
		private set;
	}

/// <summary>
/// The forward side (+Z) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Forward {
		get;
		private set;
	}

/// <summary>
/// The left side (-X) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Left {
		get;
		private set;
	}

/// <summary>
/// The right side (+X) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Right {
		get;
		private set;
	}

/// <summary>
/// The up side (+Y) of the exploding region, in world
/// coordinates.
/// </summary>
	public float Up {
		get;
		private set;
	}

	#endregion

	#region Constructors

/// <summary>
/// Define the 3D boundaries in which an explosion may occur.
/// </summary>
/// 
/// <param name="marker1">A marker, in one of the corners of the 3D space</param>
/// <param name="marker2">Another marker, in one of the corners of the 3D space</param>
	public ExplodingRegion(GameObject marker1, GameObject marker2) {
		Vector3 pos1 = marker1.transform.position;
		Vector3 pos2 = marker2.transform.position;

		Backward = pos1.z < pos2.z ? pos1.z : pos2.z;
		Forward = pos1.z > pos2.z ? pos1.z : pos2.z;

		Down = pos1.y < pos2.y ? pos1.y : pos2.y;
		Up = pos1.y > pos2.y ? pos1.y : pos2.y;

		Left = pos1.x < pos2.x ? pos1.x : pos2.x;
		Right = pos1.x > pos2.x ? pos1.x : pos2.x;
	}

	#endregion
}