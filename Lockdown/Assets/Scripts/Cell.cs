using UnityEngine;

/// <summary>
/// A <c>Cell</c> object functions more like a C-style struct, and is
/// merely used to hold information about a particular <c>Cell</c>,
/// such as which walls are up, its position in the grid, and whether
/// or not this <c>Cell</c> has been visited by the generation algorithm.
/// </summary>

public class Cell {
	#region Constructors

/// <summary>
/// Create a new <c>Cell</c> object, with all of the walls intact
/// and set the cell as unvisited by the generation algorithm.
/// </summary>
	public Cell(IVector2 position) {
		Position = position;
		Tangent = new Direction<Cell>();
		Visited = false;
		Walls = new Direction<GameObject>();
	}

	#endregion

	#region Fields

/// <summary>
/// A reference to the <c>GameObject</c> which represents the ceiling
/// of a particular <c>Cell</c>.
/// </summary>
	public GameObject Ceiling { get; set; }

/// <summary>
/// A reference to the <c>GameObject</c> which represents the floor
/// of a particular <c>Cell</c>.
/// </summary>
	public GameObject Floor { get; set; }

/// <summary>
/// The position of the <c>Cell</c> within the maze.
/// </summary>
	public IVector2 Position { get; set; }

/// <summary>
/// A collection of <c>Cell</c> objects to the North, South, East, and
/// West of the current <c>Cell</c>.
/// </summary>
	public Direction<Cell> Tangent { get; set; }

/// <summary>
/// Whether or not this <c>Cell</c> has been visited by the generaion
/// algorithm.
/// </summary>
	public bool Visited { get; set; }

/// <summary>
/// A collection of <c>GameObject</c> objects to the North, South, East,
/// and West of the current <c>Cell</c> which represent the walls which
/// divide each of the maze cells.
/// </summary>
	public Direction<GameObject> Walls { get; set; }

	#endregion
}