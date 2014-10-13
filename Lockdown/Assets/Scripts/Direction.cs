/// <summary>
/// The <c>Direction</c> class functions like a C-sytle struct,
/// and is used to hold pointers to objects which are to the North,
/// South, East, and West of a particular <c>Cell</c>.
/// </summary>

public class Direction<T> {
	#region Fields

/// <summary>
/// A pointer to an object which is to the East of the current
/// <c>Cell</c>.
/// </summary>
	public T East { get; set; }

/// <summary>
/// A pointer to an object which is to the North of the current
/// <c>Cell</c>.
/// </summary>
	public T North { get; set; }

/// <summary>
/// A pointer to an object which is to the South of the current
/// <c>Cell</c>.
/// </summary>
	public T South { get; set; }

/// <summary>
/// A pointer to an object which is to the West of the current
/// <c>Cell</c>.
/// </summary>
	public T West { get; set; }

	#endregion
}