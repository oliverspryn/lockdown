/// <summary>
/// An integer-type vector for holding the X and Y position of a 
/// <c>Cell</c> in a maze.
/// </summary>
public class IVector2 {
	#region Constructors

/// <summary>
/// Create an <c>IVector2</c> with pre-assigned values.
/// </summary>
/// 
/// <param name="x">The X position of the <c>Cell</c> in the maze</param>
/// <param name="y">The Y position of the <c>Cell</c> in the maze</param>
	public IVector2(int x, int y) {
		X = x;
		Y = y;
	}

	#endregion

	#region Fields

/// <summary>
/// The X position of the <c>Cell</c> in the maze.
/// </summary>
	public int X { get; set; }

/// <summary>
/// The Y position of the <c>Cell</c> in the maze.
/// </summary>
	public int Y { get; set; }

	#endregion

	#region Overloaded Methods

/// <summary>
/// Overloaded equals (!=) operator.
/// </summary>
/// 
/// <param name="left"><c>IVector2</c> to the left of the != operator</param>
/// <param name="right"><c>IVector2</c> to the right of the != operator</param>
/// <returns>Whether or not the two <c>IVector2</c> objects are unequal</returns>
	public static bool operator !=(IVector2 left, IVector2 right) {
		return left.X != right.X || left.Y != right.Y;
	}

/// <summary>
/// Overloaded equals (==) operator.
/// </summary>
/// 
/// <param name="left"><c>IVector2</c> to the left of the == operator</param>
/// <param name="right"><c>IVector2</c> to the right of the == operator</param>
/// <returns>Whether or not the two <c>IVector2</c> objects are equal</returns>
	public static bool operator ==(IVector2 left, IVector2 right) {
		return left.X == right.X && left.Y == right.Y;
	}

/// <summary>
/// Overloaded Equals() function.
/// </summary>
/// 
/// <param name="left">The <c>IVector2</c> object to compare</param>
/// <returns>Whether or not the two <c>IVector2</c> objects are equal</returns>
	public override bool Equals(object compare) {
		IVector2 obj = compare as IVector2;

		return X == obj.X && Y == obj.Y;
	}

/// <summary>
/// Overloaded GetHashCode() function.
/// </summary>
/// 
/// <returns>The hash code of the class instance</returns>
	public override int GetHashCode() {
		return base.GetHashCode();
	}

	#endregion
}