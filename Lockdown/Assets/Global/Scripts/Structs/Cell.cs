using UnityEngine;

/// <summary>
/// A <c>Cell</c> object functions more like a C-style struct, and is
/// merely used to hold information about a particular <c>Cell</c>,
/// such as which walls are up, its position in the grid, and whether
/// or not this <c>Cell</c> has been visited by the generation algorithm.
/// </summary>

public abstract class Cell {
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
/// Whether or not this <c>Cell</c> is the starting, root <c>Cell</c>
/// as determined by the generation algorithm.
/// </summary>
	public bool IsRoot { get; set; }

/// <summary>
/// Obtain the 3D location and sizing parameters of this <c>Cell</c>
/// object.
/// </summary>
	public Parameters Parameters {
		get {
			return new Parameters(this);
		}
	}

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
/// A pointer to another <c>Cell</c> object which will take you closer
/// to the root <c>Cell</c>.
/// </summary>
	public Cell ToRoot { get; set; }

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
	public Direction<Walls> Walls { get; set; }

	#endregion

	#region Constructors

/// <summary>
/// Create a new <c>Cell</c> object, with all of the walls intact
/// and set the cell as unvisited by the generation algorithm.
/// </summary>
	public Cell() {
		IsRoot = false;
		Position = new IVector2(0, 0);
		Tangent = new Direction<Cell>();
		Visited = false;
		Walls = new Direction<Walls>();
	}

	#endregion

	#region Public Methods

/// <summary>
/// Get several points of interest on the floor, ceiling, and various walls
/// which surround a <c>Cell</c>.
/// </summary>
/// 
/// <param name="direction">Whether to measure a wall, floor, or ceiling</param>
/// <returns>Several points of interest on the selected object</returns>
	public POI3D GetPOI(Compass selection) {
		return new POI3D(this, selection);
	}

	#endregion
}