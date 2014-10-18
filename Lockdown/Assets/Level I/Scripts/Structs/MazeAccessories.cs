using UnityEngine;

/// <summary>
/// This struct is used to hold various accessories which is 
/// associated with a <c>Maze</c>, not the individual <c>Cell</c>
/// objects within the <c>Maze</c>. This includes the maze itself,
/// its script, and the wall slider trigger.
/// </summary>
public class MazeAccessories {
	#region Fields

/// <summary>
/// The child maze within the super maze.
/// </summary>
	public GameObject Maze;

/// <summary>
/// The script associated with the child maze.
/// </summary>
	public LOneMaze Script;

/// <summary>
/// The trigger which will case the walls of the maze to rise
/// out of the ground when a player comes in contact with it.
/// </summary>
	public GameObject Trigger;

	#endregion
}