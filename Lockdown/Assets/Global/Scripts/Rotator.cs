using UnityEngine;

/// <summary>
/// This class will rotate a <c>GameObject</c> along a given axis
/// to make it stand out. This is best for collectible objects in
/// order to make them stand out from the environment.
/// </summary>
public class Rotator : MonoBehaviour {
	#region Fields

/// <summary>
/// The axis around which to rotate.
/// </summary>
	public RotatorAxis Axis = RotatorAxis.X;

/// <summary>
/// Set the direction of rotation.
/// </summary>
	public Rotation Rotation = Rotation.CounterClockwise;

/// <summary>
/// The object to rotate.
/// </summary>
	public GameObject Item;

	#endregion

	#region Public Methods

/// <summary>
/// Rotate the object along a given axis.
/// </summary>
	public void Update() {
		float rotation = (Rotation == Rotation.CounterClockwise) ? 1.0f : -1.0f;

		Item.transform.Rotate(
			Axis == RotatorAxis.X ? rotation : 0.0f,
			Axis == RotatorAxis.Y ? rotation : 0.0f,
			Axis == RotatorAxis.Z ? rotation : 0.0f
		);
	}

	#endregion
}