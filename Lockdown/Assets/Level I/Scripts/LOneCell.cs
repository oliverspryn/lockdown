using UnityEngine;

/// <summary>
/// Create a custom <c>Cell</c> object which main contain a light,
/// graffiti markings, or various objects for a player to interact
/// with.
/// </summary>
public class LOneCell : Cell {
	#region Fields

/// <summary>
/// A reference to an object a player can pick up.
/// </summary>
	public GameObject Collectable;

/// <summary>
/// A reference to a graffiti object which is 
/// </summary>
	public GameObject Graffiti { get; set; }

/// <summary>
/// A reference to the <c>GameObject</c> which represents the light
/// within a particular <c>Cell</c>.
/// </summary>
	public GameObject Light { get; set; }

	#endregion

	#region Constructors

/// <summary>
/// Create a <c>LOneCell</c> with no lights or graffiti.
/// </summary>
	public LOneCell() : base() {
		Collectable = null;
		Graffiti = null;
		Light = null;
	}

	#endregion
}