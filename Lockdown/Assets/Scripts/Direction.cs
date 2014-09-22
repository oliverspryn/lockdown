/// <summary>
/// The <c>Direction</c> class functions like a C-sytle struct,
/// and is used to hold pointers to objects which are to the North,
/// South, East, and West of a particular <c>Cell</c>.
/// </summary>

public class Direction<T> {
	#region Fields

/// <summary>
/// A pointer to an object which is to the East of the current
/// <c>Cell</c>. A null value indicates that an object in this 
/// location does not exist.
/// </summary>
	public T East { get; set; }

/// <summary>
/// A pointer to an object which is to the North of the current
/// <c>Cell</c>. A null value indicates that an object in this 
/// location does not exist.
/// </summary>
	public T North { get; set; }

/// <summary>
/// A pointer to an object which is to the South of the current
/// <c>Cell</c>. A null value indicates that an object in this 
/// location does not exist.
/// </summary>
	public T South { get; set; }

/// <summary>
/// A pointer to an object which is to the West of the current
/// <c>Cell</c>. A null value indicates that an object in this 
/// location does not exist.
/// </summary>
	public T West { get; set; }

	#endregion
}