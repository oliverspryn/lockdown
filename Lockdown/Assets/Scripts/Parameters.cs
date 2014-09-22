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
	public Vector2 Center2D { get; set; }

/// <summary>
/// The center of the cell, as described in 3D space.
/// </summary>
	public Vector3 Center3D { get; set; }

/// <summary>
/// The height (Y-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerHeight { get; set; }

/// <summary>
/// The length (Z-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerLength { get; set; }

/// <summary>
/// The width (X-direction) of the cell, when including the space as
/// limited by the walls.
/// </summary>
	public float InnerWidth { get; set; }

/// <summary>
/// The height (Y-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the height of the wall prefab).
/// </summary>
	public float OuterHeight { get; set; }

/// <summary>
/// The length (Z-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the length of the floor prefab).
/// </summary>
	public float OuterLength { get; set; }

/// <summary>
/// The width (X-direction) of the cell, when not including the space
/// as limited by the walls (i.e.: the width of the floor prefab).
/// </summary>
	public float OuterWidth { get; set; }

	#endregion
}