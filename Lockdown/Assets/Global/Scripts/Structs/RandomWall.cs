/// <summary>
/// A struct which holds information about which <c>Wall</c> object
/// is selected when the Maze.GetRandomWall() function is called.
/// </summary>
public class RandomWall {
	#region Fields

/// <summary>
/// The direction of the randomly selected <c>Wall</c> object within
/// the <c>Cell</c> object.
/// </summary>
	public Compass Direction { get; set; }

/// <summary>
/// The <c>Wall</c> object which was randomly selected.
/// </summary>
	public Walls Wall { get; set; }

	#endregion
}