using UnityEngine;

/// <summary>
/// Create a custom <c>Cell</c> object which main contain a light or
/// graffiti markings.
/// </summary>
public class LOneCell : Cell {
	#region Fields

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
		Graffiti = null;
		Light = null;
	}

	#endregion
}