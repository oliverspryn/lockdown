using UnityEngine;

/// <summary>
/// Manage the appearance of a digit on the countdown <c>Clock</c>.
/// </summary>
public class Digit : MonoBehaviour {
	#region Fields

/// <summary>
/// Set the value of the digit. The range is clamped between 0 - 9.
/// Values greater than 9 are clamped to 9, and values less than 0
/// are clamped to 0.
/// </summary>
	public int Value {
		get { return val; }
		set {
		//Set the value
			if(value < 0)
				val = 0;
			else if(value > 9)
				val = 9;
			else
				val = value;

		//Update the display value
			Material.mainTextureOffset = new Vector2(
				1.0f,
				(10 * Multiplier) - (val * Multiplier)
			);
		}
	}

	private int val = 0;

	#endregion

	#region Private Members

/// <summary>
/// Get the material associated with the digit.
/// </summary>
	private Material Material;

/// <summary>
/// The multiplier with which to apply to the texture when changing
/// the display of the number values.
/// </summary>
	private float Multiplier = 0.0909f;

	#endregion

	#region Constructors

/// <summary>
/// Initialized the digit to 0.
/// </summary>
	public void Start() {
		Material = gameObject.renderer.material;
		Value = 0;
	}

	#endregion

	#region Overloaded Operators

/// <summary>
/// Increment a <c>Digit</c> object, and its display value, by one.
/// Values are rolled over, so a 9 will be incremented to a 0.
/// </summary>
/// 
/// <param name="d">The Digit to increment</param>
/// <returns>The same Digit object, with an incremented value</returns>
	public static Digit operator ++(Digit d) {
		if(d.Value == 9)
			d.Value = 0;
		else
			d.Value = d.Value + 1;

		return d;
	}

/// <summary>
/// Decrement a <c>Digit</c> object, and its display value, by one.
/// Values are rolled over, so a 0 will be decremented to a 9.
/// </summary>
/// 
/// <param name="d">The Digit to decrement</param>
/// <returns>The same Digit object, with an decremented value</returns>
	public static Digit operator --(Digit d) {
		if(d.Value == 0)
			d.Value = 9;
		else
			d.Value = d.Value - 1;

		return d;
	}

	#endregion
}