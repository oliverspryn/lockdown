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
/// The object to rotate.
/// </summary>
	public GameObject Item;

	#endregion

	#region Public Methods

/// <summary>
/// Rotate the object along a given axis.
/// </summary>
	public void Update() {
		Item.transform.Rotate(
			Axis == RotatorAxis.X ? 1.0f : 0.0f,
			Axis == RotatorAxis.Y ? 1.0f : 0.0f,
			Axis == RotatorAxis.Z ? 1.0f : 0.0f
		);
	}

	#endregion
}