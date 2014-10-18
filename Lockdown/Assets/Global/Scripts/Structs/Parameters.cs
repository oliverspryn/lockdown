using UnityEngine;

/// <summary>
/// A <c>Parameters</c> object functions more like a C-style struct,
/// and is used to hold the 3D location and size parameters of a
/// particular <c>Cell</c> object.
/// </summary>

public class Parameters {
	#region Fields

/// <summary>
/// The center of the cell, as looked down from the top.
/// </summary>
	public Vector2 Center2D {
		get {
			return new Vector2(Target.Floor.transform.position.x, Target.Floor.transform.position.z);
		}
	}

/// <summary>
/// The center of the cell, as described in 3D space.
/// </summary>
	public Vector3 Center3D {
		get {
			return new Vector3(
				Target.Floor.transform.position.x,
				(Target.Ceiling.transform.position.y - Target.Floor.transform.position.y) / 2.0f,
				Target.Floor.transform.position.z
			);
		}
	}

/// <summary>
/// The height (Y-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerHeight {
		get {
			return Target.Ceiling.transform.position.y - (Target.Ceiling.transform.localScale.y / 2.0f) - (Target.Floor.transform.localScale.y / 2.0f);
		}
	}

/// <summary>
/// The length (Z-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerLength {
		get {
			return Target.Ceiling.transform.position.z - (!Target.Walls.North.Enabled ? 0 : (Target.Walls.North.Wall.transform.localScale.z / 2.0f)) - (!Target.Walls.South.Enabled ? 0 : (Target.Walls.South.Wall.transform.localScale.z / 2.0f));
		}
	}

/// <summary>
/// The width (X-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerWidth {
		get {
			return Target.Ceiling.transform.position.x - (!Target.Walls.East.Enabled ? 0 : (Target.Walls.East.Wall.transform.localScale.z / 2.0f)) - (!Target.Walls.West.Enabled ? 0 : (Target.Walls.West.Wall.transform.localScale.z / 2.0f));
		} 
	}

/// <summary>
/// The height (Y-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the height of the wall prefab).
/// </summary>
	public float OuterHeight {
		get {
			return Target.Ceiling.transform.position.y;
		}
	}

/// <summary>
/// The length (Z-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the length of the floor prefab).
/// </summary>
	public float OuterLength {
		get {
			return Target.Ceiling.transform.position.z;
		}
	}

/// <summary>
/// The width (X-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the width of the floor prefab).
/// </summary>
	public float OuterWidth {
		get {
			return Target.Ceiling.transform.position.x;
		}
	}

	#endregion

	#region Private Members

/// <summary>
/// The <c>Cell</c> object to measure.
/// </summary>
	private Cell Target;

	#endregion

	#region Constructors

/// <summary>
/// Build an object which can measure various aspects and the size of 
/// a <c>Cell</c>.
/// </summary>
/// 
/// <param name="target">The Cell object to measure</param>

	public Parameters(Cell target) {
		Target = target;
	}

	#endregion
}